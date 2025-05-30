using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JK.ImageViewer.Keymap
{
    public sealed class ApplicationKeymap
    {
        private Dictionary<string, Shortcut> map;

        private ApplicationKeymap() { }

        public Shortcut? GetShortcutForCommand(string command)
        {
            return map.ContainsKey(command) ? map[command] : null;
        }

        public static ApplicationKeymap LoadFromFile(string filePath)
        {
            var map = new Dictionary<string, Shortcut>();

            var doc = XDocument.Load(filePath);

            // TODO Assert root tag name

            var converter = new KeysConverter();

            foreach (var xShortcut in doc.Root!.Elements("Shortcut"))
            {
                var command = xShortcut.Attribute("Command")!.Value;
                var hasCtrlKey = xShortcut.Element("Ctrl") is not null;
                var hasShiftKey = xShortcut.Element("Shift") is not null;
                var hasAltKey = xShortcut.Element("Alt") is not null;
                var key = xShortcut.Element("Key")!.Attribute("Which")!.Value;

                map[command] = new()
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
