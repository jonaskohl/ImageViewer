using System.ComponentModel;

namespace JK.ImageViewer.Controls.SettingsEditors
{
    [SettingsEditor(typeof(bool))]
    public class BoolPropertyEditor : CheckBox, IPropertyEditor<bool>
    {
        private static TypeConverter _converter = TypeDescriptor.GetConverter(typeof(bool));
        private bool _value;
        private string _propertyName = "";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Value
        {
            get => _value;
            set
            {
                _value = value;
                Checked = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PropertyName
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                Text = this._($"Setting.{PropertyName}");
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SerializedValue
        {
            get => Value ? "True" : "False";
            set
            {
                Value = (bool)(_converter.ConvertFromString(value) ?? false);
            }
        }

        public event EventHandler? ValueChanged;

        protected void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            Value = Checked;
            OnValueChanged();
        }

        public BoolPropertyEditor()
        {
            SuspendLayout();
            AutoSize = true;
            Text = this._($"Setting.{PropertyName}");
            Checked = _value;
            //FlatStyle = FlatStyle.System;
            ResumeLayout(true);
        }
    }
}
