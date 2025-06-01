using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace JK.ImageViewer.Controls
{
    public class IconListBox : ListBox
    {
        // TODO Make property
        const int ICON_SIZE = 16;
        const int ICON_TEXT_SPACING = 4;
        const int ICON_LEFT_SPACING = 4;
        const int VERTICAL_SPACING = 2;

        const int BASE_ITEM_HEIGHT = ICON_SIZE + 2 * VERTICAL_SPACING;

        VisualStyleRenderer renderer;

        const int LISS_NORMAL = 1;
        const int LISS_HOT = 2;
        const int LISS_SELECTED = 3;
        const int LISS_HOTSELECTED = 6;

        float currentScalingFactor = 1f;

        public IconListBox() : base()
        {
            DoubleBuffered = true;
            DrawMode = DrawMode.OwnerDrawFixed;
            RecalculateItemHeight();

            renderer = new((Application.IsDarkModeEnabled ? "DarkMode_Explorer" : "Explorer") + "::ListView", 1, LISS_NORMAL);
        }

        private void RecalculateItemHeight()
        {
            currentScalingFactor = (LogicalToDeviceUnits(100) / 100f);
            ItemHeight = (int)(BASE_ITEM_HEIGHT * currentScalingFactor);
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            RecalculateItemHeight();
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            //base.OnDrawItem(e);
            e.Graphics.SetClip(e.Bounds, CombineMode.Replace);
            e.Graphics.Clear(BackColor);

            if (e.Index < 0 || e.Index >= Items.Count)
                return;

            Image? icon = null;
            string label = "";
            var item = Items[e.Index];

            if (item is IconListBoxItem iconItem)
            {
                icon = iconItem.Image;
                label = iconItem.Label;
            }
            else
            {
                label = item.ToString() ?? "";
            }

            var currentIconHeight = (int)(ICON_SIZE * currentScalingFactor);
            var iconLeftSpacing = (int)(ICON_LEFT_SPACING * currentScalingFactor);
            var iconTextSpacing = (int)(ICON_TEXT_SPACING * currentScalingFactor);

            var iconRect = new Rectangle(
                e.Bounds.X + iconLeftSpacing,
                (int)(e.Bounds.Y + (e.Bounds.Height - currentIconHeight) / 2f),
                currentIconHeight,
                currentIconHeight
            );
            var textRect = new Rectangle(
                iconRect.Right + iconTextSpacing + iconLeftSpacing,
                e.Bounds.Y,
                e.Bounds.Width - iconRect.Right - iconTextSpacing,
                e.Bounds.Height
            );

            var selected = e.State.HasFlag(DrawItemState.Selected);
            var focused = e.State.HasFlag(DrawItemState.Focus);

            if (selected)
                renderer.SetParameters(renderer.Class, renderer.Part, LISS_SELECTED);
            else
                renderer.SetParameters(renderer.Class, renderer.Part, LISS_NORMAL);

            if (focused)
                e.DrawFocusRectangle();

            if (selected)
                renderer.DrawBackground(e.Graphics, e.Bounds);

            if (icon is not null)
                e.Graphics.DrawImage(icon, iconRect);
            else
                e.Graphics.FillRectangle(Brushes.Red, iconRect);

            TextRenderer.DrawText(e.Graphics, label, e.Font, textRect, ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            e.Graphics.ResetClip();
        }
    }
}
