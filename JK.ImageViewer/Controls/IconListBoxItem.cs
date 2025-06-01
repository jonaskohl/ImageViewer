using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer.Controls
{
    public class IconListBoxItem
    {
        public Image? Image { get; set; } = null;
        public required string Label { get; set; }
        public object? Tag { get; set; }
    }
}
