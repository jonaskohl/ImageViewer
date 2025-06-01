using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer
{
    public static class UIUtil
    {
        public static float GetInterfaceScalingFactor(Control control)
        {
            return control.LogicalToDeviceUnits(100) / 100f;
        }
    }
}
