using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer
{
    public static class CurrentSettings
    {
        public static string Language => SettingsManager.Instance.GetPreference("Language", "en-US");
        public static string Theme => SettingsManager.Instance.GetPreference("Theme", "Default");
        public static bool EnableTransparencyGrid => SettingsManager.Instance.GetPreference("EnableTransparencyGrid", true);
        public static InterpolationMode ImageUpsampleMode => SettingsManager.Instance.GetPreference("ImageUpsampleMode", InterpolationMode.NearestNeighbor);
        public static InterpolationMode ImageDownsampleMode => SettingsManager.Instance.GetPreference("ImageDownsampleMode", InterpolationMode.HighQualityBicubic);
        public static bool StartMaximized => SettingsManager.Instance.GetPreference("StartMaximized", false);
    }
}
