using JK.ImageViewer.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer.ValueListProviders
{
    public class ThemeValueListProvider : IValueListProvider
    {
        public string DisplayMember => "";

        public string ValueMember => "";

        public object?[] GetValues()
        {
            return ThemeManager.GetAvailableThemes();
        }
    }
}
