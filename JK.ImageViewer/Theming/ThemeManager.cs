using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer.Theming
{
    public static class ThemeManager
    {
        private static Theme? _currentTheme;

        public static Theme CurrentTheme => _currentTheme!;

        public static string ThemeDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes");

        public static void LoadTheme(string themeName)
        {
            _currentTheme = GetTheme(themeName);
        }

        public static Theme GetTheme(string themeName)
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
