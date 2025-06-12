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
            LocalizationManager.LoadLocale(CurrentSettings.Language);
            ThemeManager.LoadTheme(CurrentSettings.Theme);
            Application.SetColorMode(SettingsManager.Instance.GetPreference("ForceColorTheme", ThemeManager.CurrentTheme.ThemeColorMode));
            ToolStripManager.RenderMode = ToolStripManagerRenderMode.Professional;
            var form = new MainForm();
            if (args.Length > 0)
                form.LoadImage(args[0]);
            Application.Run(form);
            SettingsManager.Instance.Save();
        }
    }
}