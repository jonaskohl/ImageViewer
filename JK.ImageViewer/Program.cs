using ImageMagick;
using JK.ImageViewer.Theming;
using System.Diagnostics;

namespace JK.ImageViewer
{
    internal static class Program
    {
        static IReadOnlyList<IOpenCLDevice>? _OpenCLDevices;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            using var splashScreen = new SplashScreen();
            splashScreen.Show();

            InitializeMagickOpenCL();
            SettingsManager.EnsureInstance();
            LocalizationManager.LoadLocale(CurrentSettings.Language);
            ThemeManager.LoadTheme(CurrentSettings.Theme);
            Application.SetColorMode(SettingsManager.Instance.GetPreference("ForceColorTheme", ThemeManager.CurrentTheme.ThemeColorMode));
            ToolStripManager.RenderMode = ToolStripManagerRenderMode.Professional;
            var form = new MainForm();
            form.SplashScreen = splashScreen;
            if (CurrentSettings.StartMaximized)
                form.WindowState = FormWindowState.Maximized;
            if (args.Length > 0)
                form.LoadImage(args[0], args.Length == 1 ? null : args.Where(a => !a.StartsWith("/") && !a.StartsWith("-")).ToArray());
            Application.Run(form);
            SettingsManager.Instance.Save();
        }

        private static void InitializeMagickOpenCL()
        {
            OpenCL.IsEnabled = true;
            var cacheDir = Path.Combine(Constants.LocalStoragePath, "OpenCLCache");
            if (!Directory.Exists(cacheDir))
                Directory.CreateDirectory(cacheDir);
            OpenCL.SetCacheDirectory(cacheDir);
            _OpenCLDevices = OpenCL.Devices;

            Debug.WriteLine("OpenCL devices:");
            foreach (var device in _OpenCLDevices)
                Debug.WriteLine($"- \"{device.Name}\": Enabled={device.IsEnabled} Score={device.BenchmarkScore} Type={device.DeviceType}");
        }
    }
}