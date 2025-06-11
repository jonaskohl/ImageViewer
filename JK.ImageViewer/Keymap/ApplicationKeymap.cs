using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JK.ImageViewer.Keymap
{
    public sealed class ApplicationKeymap
    {
        private Dictionary<string, Shortcut> map;

        public IEnumerable<KeyValuePair<string, Shortcut>> AllShortcuts => map.ToArray();

        private ApplicationKeymap() { }

        public Shortcut? GetShortcutForCommand(string command)
        {
            return GetShortcut("Command." + command);
        }

        public Shortcut? GetShortcutForTool(string tool)
        {
            return GetShortcut("Tool." + tool);
        }

        public Shortcut? GetShortcut(string id)
        {
            return map.ContainsKey(id) ? map[id] : null;
        }

        public static ApplicationKeymap LoadFromFile(string filePath)
        {
            var map = new Dictionary<string, Shortcut>();

            var doc = XDocument.Load(filePath);

            // TODO Assert root tag name

            var converter = new KeysConverter();

            foreach (var xShortcut in doc.Root!.Elements("Shortcut"))
            {
                var id = "";
                var command = xShortcut.Attribute("Command")?.Value;
                var tool = xShortcut.Attribute("Tool")?.Value;
                if (command is not null)
                    id = $"Command.{command}";
                else if (tool is not null)
                    id = $"Tool.{tool}";
                else
                {
                    Debug.WriteLine("Invalid shortcut definition " + xShortcut.ToString());
                    continue;
                }

                var hasCtrlKey = xShortcut.Element("Ctrl") is not null;
                var hasShiftKey = xShortcut.Element("Shift") is not null;
                var hasAltKey = xShortcut.Element("Alt") is not null;
                var key = xShortcut.Element("Key")!.Attribute("Which")!.Value;

                map[id] = new()
                {
                    CtrlKey = hasCtrlKey,
                    ShiftKey = hasShiftKey,
                    AltKey = hasAltKey,
                    Key = Enum.Parse<Keys>(key),
                };
            }

            return new ApplicationKeymap()
            {
                map = map,
            };
        }
    }
}
