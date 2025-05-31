using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer
{
    public static class MathUtil
    {
        public static T Clamp<T>(T min, T value, T max) where T : INumber<T>
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }
    }
}
