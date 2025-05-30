using ImageMagick;
using JK.ImageViewer.Attributes;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace JK.ImageViewer
{
    public partial class Form1 : Form
    {
        string baseTitle;
        string? currentPath;

        string[]? folderFiles = null;
        int folderIndex = -1;

        private record Command(string name, WindowCommandAttribute info, MethodInfo method);

        Dictionary<string, Command> availableWindowCommands;
        Theme currentTheme;

        public Form1()
        {
            InitializeComponent();
            baseTitle = Text;

            imageViewControl1.ShowCheckerboard = true;

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

            currentTheme = LoadTheme("Default");
            availableWindowCommands = GetAvailableCommands();
            LoadAndBuildToolbar();
        }

        private Theme LoadTheme(string themeName)
        {
            return Theme.LoadFromFile(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Themes",
                themeName,
                "Theme.xml"
            ));
        }

        private void LoadAndBuildToolbar()
        {
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
                            var button = new ToolStripButton()
                            {
                                Name = $"dynamicToolStripButton__{commandId++}__{commandName}",
                                Text = command.info.DisplayNameTranslationKey,
                                Image = currentTheme.GetImageForCommand(commandName),
                                DisplayStyle = ToolStripItemDisplayStyle.Image,
                            };
                            button.Click += (sender, e) => command.method.Invoke(this, []);
                            toolStrip1.Items.Add(button);
                        }
                        break;
                    case "Separator":
                        toolStrip1.Items.Add(new ToolStripSeparator());
                        break;
                    case "ZoomInput":
                        break;
                    default:
                        throw new Exception($"Malformed toolbar file: Incorrect leaf {name}");
                }
            }
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
                .Select(c => new KeyValuePair<string, Command>(c!.name, c!))
                .ToDictionary(c => c.Key, c => c.Value);
        }

        private void NumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            //SetZoomFactor((float)(toolStripNumericUpDown1.Value / 100m));
        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_OpenFile();
        }

        private void zoominToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_ZoomIn();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_ZoomOriginal();
        }

        private void zoomoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_ZoomOut();
        }

        private void previousImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_FolderImagePrevious();
        }

        private void nextImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_FolderImageNext();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_CloseImage();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_ExitApplication();
        }

        private void fitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_ZoomToFit();
        }
    }
}
