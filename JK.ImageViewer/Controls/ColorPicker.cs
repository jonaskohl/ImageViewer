using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace JK.ImageViewer.Controls
{
    public class ColorPicker : UserControl
    {
        public event EventHandler<Color>? ColorSelected;

        protected static readonly Color[] ColorPresets = [
            Color.Black, Color.Silver, Color.DarkRed, Color.Olive, Color.Green, Color.Teal, Color.Navy, Color.Purple,
            Color.Gray, Color.White, Color.Red, Color.Yellow, Color.Lime, Color.Aqua, Color.Blue, Color.Fuchsia,
        ];

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color SelectedColor { get; set; } = Color.Black;

        protected const int COLUMN_COUNT = 8;

        protected virtual void OnColorSelected(Color color)
        {
            SelectedColor = color;
            ColorSelected?.Invoke(this, color);
        }

        public ColorPicker()
        {
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Padding = new Padding(2);
            var table = new TableLayoutPanel()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
            };
            Controls.Add(table);
            table.ColumnStyles.Clear();
            table.SuspendLayout();

            var columnWidthPercent = (int)Math.Round(100f / COLUMN_COUNT, MidpointRounding.AwayFromZero);

            var rowCount = (int)Math.Ceiling(ColorPresets.Length / (float)COLUMN_COUNT);

            for (var i = 0; i < COLUMN_COUNT; ++i)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            for (var i = 0; i < rowCount + 1; ++i)
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            for (var c = 0; c < COLUMN_COUNT; ++c)
                for (var r = 0; r < rowCount; ++r)
                {
                    var i = COLUMN_COUNT * r + c;
                    var col = ColorPresets[i];
                    var button = new Button()
                    {
                        BackColor = col,
                        FlatStyle = FlatStyle.Flat,
                        Margin = new Padding(2),
                        MinimumSize = new Size(16, 16),
                        Size = new Size(16, 16),
                        Text = "",
                    };
                    button.Click += (s, e) => OnColorSelected(col);
                    table.Controls.Add(button, c, r);
                }

            var halfCols = COLUMN_COUNT / 2;
            var leftSpace = (COLUMN_COUNT - halfCols) / 2;
            var moreBtn = new Button()
            {
                FlatStyle = FlatStyle.System,
                Text = this._("Misc.ColorPicker.OtherLabel"),
            };
            moreBtn.Click += MoreBtn_Click;
            table.Controls.Add(moreBtn, leftSpace, rowCount);
            table.SetColumnSpan(moreBtn, halfCols);

            table.ResumeLayout(true);
        }

        private void MoreBtn_Click(object? sender, EventArgs e)
        {
            using var picker = new ColorDialog()
            {
                Color = SelectedColor,
            };
            if (picker.ShowDialog(FindForm()) == DialogResult.OK)
            {
                SelectedColor = picker.Color;
                OnColorSelected(picker.Color);
            }
        }
    }
}
