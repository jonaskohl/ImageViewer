using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer.Controls.SettingsEditors
{
    public interface IValueListPropertyEditor
    {
        public object?[] ValueList { get; set; }
        public string ValueListDisplayMember { get; set; }
        public string ValueListValueMember { get; set; }
    }
}
