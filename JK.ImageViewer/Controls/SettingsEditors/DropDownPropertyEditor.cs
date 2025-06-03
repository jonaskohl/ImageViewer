using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace JK.ImageViewer.Controls.SettingsEditors
{
    [SettingsEditor(typeof(string))]
    public class DropDownPropertyEditor : Panel, IPropertyEditor<string>, IValueListPropertyEditor
    {
        private string _displayMember = "";
        private string _valueMember = "";
        private string? _value;
        private string _propertyName = "";
        private object?[]? _valueList;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? Value
        {
            get => _value;
            set
            {
                Debug.WriteLine($"DropDownPropertyEditor.Value.set {value ?? "<NULL>"}");

                _value = value;
                if (value is null)
                    comboBox.SelectedIndex = -1;
                else
                {
                    var index = FindValueIndex(value);
                    if (index >= 0)
                    {
                        var item = _valueList![index];
                        comboBox.SelectedItem = item;
                    }
                    else
                        comboBox.SelectedIndex = -1;
                }
            }
        }

        private int FindValueIndex(string? value)
        {
            if (comboBox.DataSource is null)
            {
                Debug.WriteLine($"DropDownPropertyEditor.FindValueIndex {value ?? "<NULL>"} -> DataSource is null");
                return -1;
            }

            
            var propInfo = GetValuePropInfo(_valueList[0], comboBox.ValueMember);

            for (var i = 0; i < _valueList.Length; ++i)
            {
                var itemValue = GetValue(_valueList[i]!, propInfo);
                if (itemValue == value)
                {
                    Debug.WriteLine($"DropDownPropertyEditor.FindValueIndex {value ?? "<NULL>"} -> Found at {i}");
                    return i;
                }
            }

            Debug.WriteLine($"DropDownPropertyEditor.FindValueIndex {value ?? "<NULL>"} -> Not found");

            return -1;
        }

        private PropertyInfo? GetValuePropInfo(object item, string valueMember)
        {
            var itemType = item.GetType()!;
            return valueMember == string.Empty
                ? null
                : itemType.GetProperty(valueMember, BindingFlags.Instance | BindingFlags.Public);
        }

        private string? GetValue(object item, string valueMember)
        {
            return GetValue(item, GetValuePropInfo(item, valueMember));
        }

        private string? GetValue(object item, PropertyInfo? propInfo)
        {
            return propInfo is null
                ? item?.ToString()
                : propInfo.GetValue(item)?.ToString();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PropertyName
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                label.Text = this._($"Setting.{PropertyName}") + ":";
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SerializedValue
        {
            get => Value ?? "";
            set
            {
                Debug.WriteLine($"DropDownPropertyEditor.SerializedValue.set {value ?? "<NULL>"}");
                Value = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object?[] ValueList
        {
            get => _valueList ?? [];
            set
            {
                Debug.WriteLine($"DropDownPropertyEditor.ValueList.set");

                _valueList = value;
                comboBox.DataSource = null;
                comboBox.DisplayMember = _displayMember;
                comboBox.ValueMember = _valueMember;
                comboBox.DataSource = new BindingSource()
                {
                    DataSource = _valueList,
                };
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ValueListDisplayMember
        {
            get => _displayMember;
            set => _displayMember = comboBox.DisplayMember = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ValueListValueMember
        {
            get => _valueMember;
            set => _valueMember = comboBox.ValueMember = value;
        }

        public event EventHandler? ValueChanged;

        private TableLayoutPanel tableLayoutPanel;
        private Label label;
        private ComboBox comboBox;

        protected void OnValueChanged()
        {
            Debug.WriteLine($"DropDownPropertyEditor.OnValueChanged");
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            proposedSize.Width = Parent?.ClientSize.Width ?? proposedSize.Width;
            return base.GetPreferredSize(proposedSize);
        }

        public DropDownPropertyEditor()
        {
            SuspendLayout();
            AutoSize = true;
            tableLayoutPanel = new()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Fill,
            };
            tableLayoutPanel.SuspendLayout();

            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            label = new Label()
            {
                Text = this._($"Setting.{PropertyName}") + ":",
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Left,
                Padding = Padding.Empty,
                Margin = Padding.Empty,
            };
            tableLayoutPanel.Controls.Add(label, 0, 0);

            comboBox = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.System,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
            };
            tableLayoutPanel.Controls.Add(comboBox, 1, 0);
            comboBox.SelectionChangeCommitted += ComboBox_SelectionChangeCommitted;

            tableLayoutPanel.ResumeLayout(true);
            Controls.Add(tableLayoutPanel);
            ResumeLayout(true);
        }

        private void ComboBox_SelectionChangeCommitted(object? sender, EventArgs e)
        {
            Debug.WriteLine($"DropDownPropertyEditor.ComboBox_SelectionChangeCommitted");

            Value = comboBox.SelectedValue?.ToString();
            OnValueChanged();
        }
    }
}
