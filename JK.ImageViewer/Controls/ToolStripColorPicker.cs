using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace JK.ImageViewer.Controls
{
    public class ToolStripColorPicker : ToolStripDropDownButton
    {
        private ToolStripDropDown _dropDownInstance;
        private ColorPicker _pickerInstance;

        public event EventHandler? ColorChanged;

        private Color _color = Color.Red;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color Color
        {
            get => _color;
            set
            {
                _pickerInstance.SelectedColor = value;
                _color = value;
                Invalidate();
            }
        }

        protected virtual void OnColorChanged()
        {
            ColorChanged?.Invoke(this, EventArgs.Empty);
        }

        public ToolStripColorPicker() : base()
        {
            _pickerInstance = new ColorPicker();
            _pickerInstance.ColorSelected += ColorPicker_ColorSelected;

            _dropDownInstance = new ToolStripDropDown();
            _dropDownInstance.Items.Add(new ToolStripControlHost(_pickerInstance));
            base.DropDown = _dropDownInstance;
        }

        private void ColorPicker_ColorSelected(object? sender, Color e)
        {
            Color = e;
            HideDropDown();
            OnColorChanged();
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ToolStripDropDown DropDown
        {
            get => base.DropDown;
        }

        protected float DpiScalingFactor => Owner is null ? 1f : Utils.GetInterfaceScalingFactor(Owner);

        public override Size GetPreferredSize(Size constrainingSize)
        {
            var bSize = base.GetPreferredSize(constrainingSize);
            return new Size(bSize.Width + (int)(24 * DpiScalingFactor), bSize.Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using var brush = new SolidBrush(Color);

            const int PREVIEW_SIZE = 16;
            int scaledPreviewSize = (int)(PREVIEW_SIZE * DpiScalingFactor);

            var contentRect = ContentRectangle;

            var rect = new Rectangle(
                contentRect.Left + (contentRect.Width - scaledPreviewSize) / 2,
                contentRect.Top + (contentRect.Height - scaledPreviewSize) / 2,
                scaledPreviewSize,
                scaledPreviewSize
            );

            if (Enabled)
            {
                e.Graphics.FillRectangle(brush, rect);
                e.Graphics.DrawRectangle(Pens.Black, rect);
            }
            else
            {
                ControlPaint.DrawBorder(e.Graphics, rect, SystemColors.GrayText, ButtonBorderStyle.Solid);
            }
        }
    }
}
