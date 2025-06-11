using ImageMagick;
using ImageMagick.Factories;
using JK.ImageViewer.Attributes;
using JK.ImageViewer.Controls;
using JK.ImageViewer.Keymap;
using JK.ImageViewer.Theming;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace JK.ImageViewer
{
    public partial class MainForm : Form
    {
        public enum EditorTool
        {
            Default,
            Zoom,
            Crop,
            Rectangle,
            Ellipse,
            Line,
        }

        string baseTitle;
        string? currentPath;
        bool isUnsaved = false;

        string[]? folderFiles = null;
        int folderIndex = -1;

        Color drawingColor = Color.Red;

        EditorTool currentTool = EditorTool.Default;

        private record Command(string Name, WindowCommandAttribute Info, MethodInfo Method);

        ApplicationKeymap currentKeymap;
        Dictionary<string, Command> availableWindowCommands;
        Dictionary<string, ToolStripMenuItem> commandMenuItems = new();
        Dictionary<string, ToolStripButton> commandButtons = new();
        Dictionary<string, bool> commandEnabledState = new();
        Dictionary<EditorTool, bool> toolEnabledState = new();
        Dictionary<EditorTool, ToolStripButton> toolButtons = new();
        ToolStripNumericUpDown? zoomInput = null;
        ToolStripColorPicker? colorPicker = null;

        public void SetTool(EditorTool tool)
        {
            foreach (var buttonMapping in toolButtons)
            {
                buttonMapping.Value.Checked = buttonMapping.Key == tool;
            }

            currentTool = tool;
            switch (tool)
            {
                case EditorTool.Default:
                    imageViewControl1.CurrentToolMode = ImageViewControl.ToolMode.None;
                    break;
                case EditorTool.Crop:
                case EditorTool.Zoom:
                case EditorTool.Rectangle:
                case EditorTool.Ellipse:
                    imageViewControl1.CurrentToolMode = ImageViewControl.ToolMode.Marquee;
                    break;
                case EditorTool.Line:
                    imageViewControl1.CurrentToolMode = ImageViewControl.ToolMode.Line;
                    break;
                default:
                    throw new UnreachableException();
            }
        }

        public MainForm()
        {
            InitializeComponent();
            baseTitle = Text;

            availableWindowCommands = GetAvailableCommands();
            toolEnabledState = Enum.GetValues<EditorTool>().ToDictionary(v => v, _ => true);
            commandEnabledState = availableWindowCommands.ToDictionary(v => v.Key, _ => true);

            imageViewControl1.ShowCheckerboard = CurrentSettings.EnableTransparencyGrid;
            imageViewControl1.UpsampleMode = CurrentSettings.ImageUpsampleMode;
            imageViewControl1.DownsampleMode = CurrentSettings.ImageDownsampleMode;

            imageViewControl1.MarqueeSelectionCreated += ImageViewControl1_MarqueeSelectionCreated;
            imageViewControl1.LineCreated += ImageViewControl1_LineCreated;

            string filter =
                $"All supported files|{string.Join(";", MagickNET.SupportedFormats
                    .Where(format => format.SupportsReading)
                    .Select(format => $"*.{format.Format.ToString().ToLower()}")
                    .ToArray()
                )}|" +
                string.Join("|", MagickNET.SupportedFormats
                    .Where(format => format.SupportsReading)
                    .Select(format => $"{format.Format.ToString()} files|*.{format.Format.ToString().ToLower()}")
                    .ToArray()
                ) + "|All files|*.*";
            openFileDialog1.Filter = filter;

            currentKeymap = ApplicationKeymap.LoadFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings", "Keymap.xml"));
            LoadAndBuildMenu();
            LoadAndBuildToolbar();

            SettingsManager.Instance.SettingChanged += Instance_SettingChanged;
            LocalizationManager.LanguageChanged += LocalizationManager_LanguageChanged;

            ClearImage(true);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.UserClosing && !ClearImage(true))
                e.Cancel = true;
        }

        private void ImageViewControl1_LineCreated(object? sender, (Point From, Point To) line)
        {
            switch (currentTool)
            {
                case EditorTool.Line:
                    DrawLineToCurrentImage(line);
                    break;
            }
        }

        private void ImageViewControl1_MarqueeSelectionCreated(object? sender, Rectangle rect)
        {
            switch (currentTool)
            {
                case EditorTool.Crop:
                    CropCurrentImage(rect);
                    break;
                case EditorTool.Zoom:
                    imageViewControl1.SetZoomRegion(rect);
                    break;
                case EditorTool.Rectangle:
                    DrawRectangleToCurrentImage(rect);
                    break;
                case EditorTool.Ellipse:
                    DrawEllipseToCurrentImage(rect);
                    break;
            }
        }

        private void DrawLineToCurrentImage((Point From, Point To) line)
        {
            if (imageViewControl1.ContentImage is null)
                return;

            var bmp = new Bitmap(imageViewControl1.ContentImage);
            using var g = Graphics.FromImage(bmp);
            using var pen = new Pen(drawingColor);
            g.DrawLine(pen, line.From, line.To);

            ReplaceCurrentImage(bmp);
            SetUnsaved(true);
        }

        private void DrawEllipseToCurrentImage(Rectangle rect)
        {
            if (imageViewControl1.ContentImage is null)
                return;

            var bmp = new Bitmap(imageViewControl1.ContentImage);
            using var g = Graphics.FromImage(bmp);
            using var brush = new SolidBrush(drawingColor);
            g.FillEllipse(brush, rect);

            ReplaceCurrentImage(bmp);
            SetUnsaved(true);
        }

        private void DrawRectangleToCurrentImage(Rectangle rect)
        {
            if (imageViewControl1.ContentImage is null)
                return;

            var bmp = new Bitmap(imageViewControl1.ContentImage);
            using var g = Graphics.FromImage(bmp);
            using var brush = new SolidBrush(drawingColor);
            g.FillRectangle(brush, rect);

            ReplaceCurrentImage(bmp);
            SetUnsaved(true);
        }

        private void CropCurrentImage(Rectangle rect)
        {
            if (imageViewControl1.ContentImage is null)
                return;

            var magickFactory = new MagickFactory();
            using var magickImage = (MagickImage)magickFactory.Image.Create((imageViewControl1.ContentImage as Bitmap)!);
            magickImage.Crop(new MagickGeometry(
                rect.X,
                rect.Y,
                (uint)rect.Width,
                (uint)rect.Height
            ));

            ReplaceCurrentImage(magickImage.ToBitmap());
            SetUnsaved(true);
            UpdateZoomText();
        }

        private void LocalizationManager_LanguageChanged(object? sender, EventArgs e)
        {
            LoadAndBuildMenu();
            LoadAndBuildToolbar();
        }

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);

            LoadAndBuildMenu();
            LoadAndBuildToolbar();
        }

        private void Instance_SettingChanged(object? sender, SettingsManager.SettingChangedEventArgs e)
        {
            switch (e.Key)
            {
                case "Language":
                    LocalizationManager.LoadLocale(e.NewValue);
                    break;
                case "EnableTransparencyGrid":
                    imageViewControl1.ShowCheckerboard = CurrentSettings.EnableTransparencyGrid;
                    break;
                case "ImageUpsampleMode":
                    imageViewControl1.UpsampleMode = CurrentSettings.ImageUpsampleMode;
                    break;
                case "ImageDownsampleMode":
                    imageViewControl1.DownsampleMode = CurrentSettings.ImageDownsampleMode;
                    break;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            foreach (var shortcut in currentKeymap.AllShortcuts)
            {
                if (keyData == shortcut.Value.ToKeys())
                {
                    if (shortcut.Key.StartsWith("Command."))
                    {
                        if (availableWindowCommands.TryGetValue(shortcut.Key.Substring(8), out Command? command))
                            if (commandEnabledState[command.Name])
                                command?.Method?.Invoke(this, []);
                        return true;
                    }
                    else if (shortcut.Key.StartsWith("Tool."))
                    {
                        Debug.WriteLine("Tool candidate: " + shortcut.Key);
                        if (Enum.TryParse<EditorTool>(shortcut.Key.Substring(5), out var tool))
                        {
                            if (toolEnabledState[tool])
                            {
                                SetTool(tool);
                            }
                            else
                                Debug.WriteLine("Tool not enabled");
                        }
                        else
                            Debug.WriteLine("Failed to parse");
                        return true;
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LoadAndBuildMenu()
        {
            menuStrip1.Items.Clear();
            menuStrip1.SuspendLayout();

            var doc = XDocument.Load(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Settings",
                "Menu.xml"
            ));

            if (doc.Root is null || doc.Root.Name.ToString() != "MenuRoot")
                throw new Exception("Malformed menu file: Incorrect root");

            var commandId = 0;

            void ProcessMenu(XElement menuElement, ToolStripItemCollection menuStripItems, string menuPath = "")
            {
                var name = menuElement.Name.ToString();
                switch (name)
                {
                    case "Menu":
                        {
                            var menuKey = menuElement.Attribute("Key")!.Value;
                            var fullMenuPath = menuPath + menuKey;
                            var parent = new ToolStripMenuItem(this._("Menu." + fullMenuPath));
                            foreach (var el in menuElement.Elements())
                                ProcessMenu(el, parent.DropDownItems, fullMenuPath + "/");
                            menuStripItems.Add(parent);
                            break;
                        }
                    case "Command":
                        {
                            var commandName = menuElement.Attribute("Command")?.Value;
                            if (commandName is null || !availableWindowCommands.ContainsKey(commandName))
                                throw new Exception($"Malformed menu file: Incorrect attribute \"Command\"");

                            var command = availableWindowCommands[commandName];
                            var image = ThemeManager.CurrentTheme.GetImage($"Command.{commandName}", this);
                            var shortcut = currentKeymap.GetShortcutForCommand(commandName);
                            var button = new ToolStripMenuItem()
                            {
                                Name = $"dynamicToolStripMenuItem__{commandId++}__{commandName}",
                                Text = this._($"Command.{commandName}"),
                                Image = image,
                                ShortcutKeyDisplayString = shortcut?.ToString(),
                            };
                            button.Click += (sender, e) => command.Method.Invoke(this, []);
                            menuStripItems.Add(button);
                            commandMenuItems[commandName] = button;
                        }
                        break;
                    case "Separator":
                        menuStripItems.Add(new ToolStripSeparator());
                        break;
                    default:
                        throw new Exception($"Malformed menu file: Incorrect leaf {name}");
                }
            }


            foreach (var el in doc.Root.Elements())
            {
                if (el.Name != "Menu")
                    throw new Exception("Malformed menu file: Incorrect root child");

                ProcessMenu(el, menuStrip1.Items);
            }

            menuStrip1.ResumeLayout(true);
        }

        private void LoadAndBuildToolbar()
        {
            toolStrip1.Items.Clear();
            toolStrip1.SuspendLayout();

            var doc = XDocument.Load(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Settings",
                "Toolbar.xml"
            ));

            if (doc.Root is null || doc.Root.Name.ToString() != "Toolbar")
                throw new Exception("Malformed toolbar file: Incorrect root");

            var commandId = 0;

            foreach (var el in doc.Root.Elements())
            {
                var name = el.Name.ToString();
                switch (name)
                {
                    case "Command":
                        {
                            var commandName = el.Attribute("Command")?.Value;
                            if (commandName is null || !availableWindowCommands.ContainsKey(commandName))
                                throw new Exception($"Malformed toolbar file: Incorrect attribute \"Command\"");

                            var command = availableWindowCommands[commandName];
                            var image = ThemeManager.CurrentTheme.GetImage($"Command.{commandName}", this);
                            var button = new ToolStripButton()
                            {
                                Name = $"dynamicToolStripButton__{commandId++}__{commandName}",
                                Text = this._($"Command.{commandName}"),
                                Image = image,
                                DisplayStyle = image is null ? ToolStripItemDisplayStyle.Text : ToolStripItemDisplayStyle.Image,
                            };
                            button.Click += (sender, e) => command.Method.Invoke(this, []);
                            toolStrip1.Items.Add(button);
                            commandButtons[commandName] = button;
                        }
                        break;
                    case "Tool":
                        {
                            var toolName = el.Attribute("Tool")?.Value;

                            var isToolValueValid = Enum.TryParse(toolName, out EditorTool toolValue);

                            if (toolName is null || !isToolValueValid)
                                throw new Exception($"Malformed toolbar file: Incorrect attribute \"Tool\"");

                            var image = ThemeManager.CurrentTheme.GetImage($"Tool.{toolName}", this);
                            var button = new ToolStripButton()
                            {
                                Name = $"dynamicToolStripButton__{commandId++}__{toolName}",
                                Text = this._($"Tool.{toolName}"),
                                Image = image,
                                DisplayStyle = image is null ? ToolStripItemDisplayStyle.Text : ToolStripItemDisplayStyle.Image,
                                Checked = toolName == "Default",
                            };
                            button.Click += (sender, e) => SetTool(toolValue);
                            toolStrip1.Items.Add(button);
                            toolButtons[toolValue] = button;
                        }
                        break;
                    case "Separator":
                        toolStrip1.Items.Add(new ToolStripSeparator());
                        break;
                    case "ZoomInput":
                        {
                            var item = new ToolStripNumericUpDown()
                            {
                                AutoSize = false,
                                Width = 80,
                                Minimum = (decimal)Constants.ZOOM_FACTOR_MIN * 100m,
                                Maximum = (decimal)Constants.ZOOM_FACTOR_MAX * 100m,
                                DecimalPlaces = 2,
                                Value = (decimal)imageViewControl1.ZoomFactor * 100m,
                            };
                            item.NumericUpDown.Suffix = " %";
                            item.NumericUpDown.ValueChanged += (sender, e) => SetZoomFactor((float)(item.Value / 100m));
                            imageViewControl1.ZoomFactorChanged += (sender, e) => item.Value = (decimal)imageViewControl1.ZoomFactor * 100m;
                            toolStrip1.Items.Add(item);
                            zoomInput = item;
                        }
                        break;
                    case "ColorPicker":
                        {
                            var item = new ToolStripColorPicker()
                            {
                                Color = drawingColor,
                            };
                            item.ColorChanged += (s, e) => drawingColor = item.Color;
                            toolStrip1.Items.Add(item);
                            colorPicker = item;
                        }
                        break;
                    default:
                        throw new Exception($"Malformed toolbar file: Incorrect leaf {name}");
                }
            }

            toolStrip1.ResumeLayout(true);
        }

        private Dictionary<string, Command> GetAvailableCommands()
        {
            return GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Select(mi =>
                {
                    var name = mi.Name;
                    if (!name.StartsWith("Command_"))
                        return null;
                    name = mi.Name.Substring("Command_".Length);
                    var info = mi.GetCustomAttribute<WindowCommandAttribute>();
                    if (info is null)
                        return null;
                    return new Command(name, info, mi);
                })
                .Where(c => c is not null)
                .Select(c => new KeyValuePair<string, Command>(c!.Name, c!))
                .ToDictionary(c => c.Key, c => c.Value);
        }

        private void imageViewControl1_ZoomFactorChanged(object sender, EventArgs e)
        {
            UpdateZoomText();

            var hasImage = currentPath is not null;
            SetCommandEnabled("ZoomIn", hasImage && imageViewControl1.ZoomFactor < Constants.ZOOM_FACTOR_MAX);
            SetCommandEnabled("ZoomOut", hasImage && imageViewControl1.ZoomFactor > Constants.ZOOM_FACTOR_MIN);
        }
    }
}
