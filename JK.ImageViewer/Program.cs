using JK.ImageViewer.Theming;

namespace JK.ImageViewer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            SettingsManager.EnsureInstance();
            Theme theme = ThemeManager.LoadTheme(SettingsManager.Instance.GetPreference("Theme", "Default"));
            Application.SetColorMode(theme.ThemeColorMode);
            ToolStripManager.RenderMode = ToolStripManagerRenderMode.Professional;
            var form = new MainForm(theme);
            if (args.Length > 0)
                form.LoadImage(args[0]);
            Application.Run(form);
            SettingsManager.Instance.Save();
        }
    }
}