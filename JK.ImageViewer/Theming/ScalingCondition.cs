using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer.Theming
{
    public class ScalingCondition(string strFactorProperty) : ThemePropertyCondition
    {
        public float MaxFactor { get; private set; } = float.Parse(strFactorProperty);

        public override bool Evaluate(Control control)
        {
            var currentScalingFactor = UIUtil.GetInterfaceScalingFactor(control);
            return currentScalingFactor <= MaxFactor;
        }
    }
}
