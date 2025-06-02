using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer.Controls.SettingsEditors
{
    public interface IPropertyEditor
    {
        public event EventHandler? ValueChanged;
        public string PropertyName { get; set; }
        public string SerializedValue { get; set; }
    }

    public interface IPropertyEditor<TProperty> : IPropertyEditor
    {
        public TProperty? Value { get; set; }
    }
}
