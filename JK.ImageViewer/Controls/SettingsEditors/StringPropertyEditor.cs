using System.ComponentModel;

namespace JK.ImageViewer.Controls.SettingsEditors
{
    [SettingsEditor(typeof(string))]
    public class StringPropertyEditor : Panel, IPropertyEditor<string>
    {
        private string? _value;
        private string _propertyName = "";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? Value
        {
            get => _value;
            set
            {
                _value = value;
                textBox.Text = value ?? "";
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PropertyName
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                label.Text = this._($"Setting.{PropertyName}");
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SerializedValue
        {
            get => Value ?? "";
            set => Value = value;
        }

        public event EventHandler? ValueChanged;

        private TableLayoutPanel tableLayoutPanel;
        private Label label;
        private TextBox textBox;

        protected void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            proposedSize.Width = Parent?.ClientSize.Width ?? proposedSize.Width;
            return base.GetPreferredSize(proposedSize);
        }

        public StringPropertyEditor()
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
                Text = this._($"Setting.{PropertyName}"),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Left,
                Padding = Padding.Empty,
                Margin = Padding.Empty,
            };
            tableLayoutPanel.Controls.Add(label, 0, 0);

            textBox = new TextBox()
            {
                Text = Value ?? "",
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
            };
            tableLayoutPanel.Controls.Add(textBox, 1, 0);
            textBox.TextChanged += TextBox_TextChanged;

            tableLayoutPanel.ResumeLayout(true);
            Controls.Add(tableLayoutPanel);
            ResumeLayout(true);
        }

        private void TextBox_TextChanged(object? sender, EventArgs e)
        {
            Value = textBox.Text;
            OnValueChanged();
        }
    }
}
