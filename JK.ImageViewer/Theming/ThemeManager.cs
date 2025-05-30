using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer.Theming
{
    public static class ThemeManager
    {
        public static string ThemeDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes");

        public static Theme LoadTheme(string themeName)
        {
            return Theme.LoadFromFile(Path.Combine(
                ThemeDirectory,
                themeName,
                "Theme.xml"
            ));
        }

        public static string[] GetAvailableThemes()
        {
            return Directory.GetDirectories(ThemeDirectory)
                .Where(dir => File.Exists(Path.Combine(dir, "Theme.xml")))
                .Select(dir => new DirectoryInfo(dir).Name)
                .ToArray();
        }
    }
}
