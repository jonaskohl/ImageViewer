using ImageMagick;
using JK.ImageViewer.Attributes;
using JK.ImageViewer.Controls;
using JK.ImageViewer.Keymap;
using JK.ImageViewer.Theming;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace JK.ImageViewer
{
    public partial class MainForm : Form
    {
        string baseTitle;
        string? currentPath;

        string[]? folderFiles = null;
        int folderIndex = -1;

        private record Command(string Name, WindowCommandAttribute Info, MethodInfo Method);

        Dictionary<string, Command> availableWindowCommands;
        ApplicationKeymap currentKeymap;
        Theme currentTheme;

        public MainForm(Theme initialTheme)
        {
            InitializeComponent();
            baseTitle = Text;

            imageViewControl1.ShowCheckerboard = SettingsManager.Instance.GetPreference("EnableTransparencyGrid", true);

            //toolStripNumericUpDown1.NumericUpDown.ValueChanged += NumericUpDown_ValueChanged;

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

            currentTheme = initialTheme;
            availableWindowCommands = GetAvailableCommands();
            currentKeymap = ApplicationKeymap.LoadFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings", "Keymap.xml"));
            LoadAndBuildMenu();
            LoadAndBuildToolbar();

            SettingsManager.Instance.SettingChanged += Instance_SettingChanged;
        }

        private void Instance_SettingChanged(object? sender, SettingsManager.SettingChangedEventArgs e)
        {
            if (e.Key == "EnableTransparencyGrid")
                imageViewControl1.ShowCheckerboard = SettingsManager.Instance.GetPreference("EnableTransparencyGrid", true);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            foreach (var shortcut in currentKeymap.AllShortcuts)
            {
                if (keyData == shortcut.Value.ToKeys())
                {
                    if (availableWindowCommands.TryGetValue(shortcut.Key, out Command? command))
                        command?.Method?.Invoke(this, []);
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SelectTheme(string theme)
        {
            currentTheme = ThemeManager.LoadTheme(theme);
            Application.SetColorMode(currentTheme.ThemeColorMode);
            LoadAndBuildToolbar();
            Invalidate();
            Opacity = 0;
            Application.DoEvents();
            Opacity = 1;

            if (MessageBox.Show("To properly apply the theme you need to restart the application!\r\n\r\nDo you want to restart the application now?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                Application.Restart();
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

            void ProcessMenu(XElement menuElement, ToolStripItemCollection menuStripItems)
            {
                var name = menuElement.Name.ToString();
                switch (name)
                {
                    case "Menu":
                        {
                            var parent = new ToolStripMenuItem(menuElement.Attribute("Label")!.Value);
                            foreach (var el in menuElement.Elements())
                                ProcessMenu(el, parent.DropDownItems);
                            menuStripItems.Add(parent);
                            break;
                        }
                    case "Command":
                        {
                            var commandName = menuElement.Attribute("Command")?.Value;
                            if (commandName is null || !availableWindowCommands.ContainsKey(commandName))
                                throw new Exception($"Malformed menu file: Incorrect attribute \"Command\"");

                            var command = availableWindowCommands[commandName];
                            var image = currentTheme.GetImage($"Command.{commandName}");
                            var shortcut = currentKeymap.GetShortcutForCommand(commandName);
                            var button = new ToolStripMenuItem()
                            {
                                Name = $"dynamicToolStripMenuItem__{commandId++}__{commandName}",
                                Text = command.Info.DisplayNameTranslationKey,
                                Image = image,
                                ShortcutKeyDisplayString = shortcut?.ToString(),
                            };
                            button.Click += (sender, e) => command.Method.Invoke(this, []);
                            menuStripItems.Add(button);
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
                            var image = currentTheme.GetImage($"Command.{commandName}");
                            var button = new ToolStripButton()
                            {
                                Name = $"dynamicToolStripButton__{commandId++}__{commandName}",
                                Text = command.Info.DisplayNameTranslationKey,
                                Image = image,
                                DisplayStyle = image is null ? ToolStripItemDisplayStyle.Text : ToolStripItemDisplayStyle.Image,
                            };
                            button.Click += (sender, e) => command.Method.Invoke(this, []);
                            toolStrip1.Items.Add(button);
                        }
                        break;
                    case "Separator":
                        toolStrip1.Items.Add(new ToolStripSeparator());
                        break;
                    case "ZoomInput":
                        {
                            var item = new ToolStripNumericUpDown()
                            {
                                Minimum = 12.5m,
                                Maximum = 800m,
                                DecimalPlaces = 2,
                                Value = (decimal)imageViewControl1.ZoomFactor * 100m,
                            };
                            item.NumericUpDown.ValueChanged += (sender, e) => SetZoomFactor((float)(item.Value / 100m));
                            imageViewControl1.ZoomFactorChanged += (sender, e) => item.Value = (decimal)imageViewControl1.ZoomFactor * 100m;
                            toolStrip1.Items.Add(item);
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
        }
    }
}
