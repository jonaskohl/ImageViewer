using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer
{
    public class Constants
    {
        public const float ZOOM_FACTOR_MIN = 0.125f;
        public const float ZOOM_FACTOR_MAX = 80f;

        public static string LocalStoragePath => Environment.ExpandEnvironmentVariables(@"%localappdata%\Jonas Kohl\ImageViewer");
        public static string StoragePath => Environment.ExpandEnvironmentVariables(@"%appdata%\Jonas Kohl\ImageViewer");
    }
}
