using ImageMagick;
using ImageMagick.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer
{
    public static class Utils
    {
        public static float GetInterfaceScalingFactor(Control control)
        {
            return control.LogicalToDeviceUnits(100) / 100f;
        }

        public static MagickImage GdiToMagick(Bitmap bmp)
        {
            var magickFactory = new MagickFactory();
            return (MagickImage)magickFactory.Image.Create(bmp);
        }
    }
}
