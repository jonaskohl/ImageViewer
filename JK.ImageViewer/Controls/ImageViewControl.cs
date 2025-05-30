using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using ImageMagick;

namespace JK.ImageViewer.Controls
{
    internal class ImageViewControl : ScrollableControl
    {
        private float _zoomFactor = 1.0f;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float ZoomFactor
        {
            get => _zoomFactor;
            set
            {
                _zoomFactor = value;
                Repaint();
            }
        }

        private bool _showCheckerboard = false;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowCheckerboard
        {
            get => _showCheckerboard;
            set
            {
                _showCheckerboard = value;
                Repaint();
            }
        }

        private Image _contentImage = null;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image ContentImage
        {
            get => _contentImage;
            set
            {
                _contentImage = value;
                Repaint();
            }
        }

        public ImageViewControl()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor
                | ControlStyles.ResizeRedraw
            , true);
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            if (se.OldValue != se.NewValue)
                Repaint();
        }

        private void Repaint()
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_contentImage is null)
                return;

            var prevOffsetMode = e.Graphics.PixelOffsetMode;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var innerWidth = ClientSize.Width;
            var innerHeight = ClientSize.Height;

            var displayImageWidth = _contentImage.Width * _zoomFactor;
            var displayImageHeight = _contentImage.Height * _zoomFactor;
            var hOverflow = displayImageWidth - innerWidth;
            var vOverflow = displayImageHeight - innerHeight;

            var posX = Math.Max(0, (innerWidth - displayImageWidth) / 2f) + AutoScrollPosition.X;
            var posY = Math.Max(0, (innerHeight - displayImageHeight) / 2f) + AutoScrollPosition.Y;

            AutoScrollMinSize = new Size(
                (int)Math.Ceiling(displayImageWidth),
                (int)Math.Ceiling(displayImageHeight)
            );

            var displayRect = new RectangleF(posX, posY, displayImageWidth, displayImageHeight);

            if (_showCheckerboard)
            {
                var lightColor = Application.IsDarkModeEnabled ? Color.FromArgb(0x44, 0x44, 0x44) : Color.FromArgb(0xC0, 0xC0, 0xC0);
                var darkColor = Application.IsDarkModeEnabled ? Color.FromArgb(0x11, 0x11, 0x11) : Color.FromArgb(0x80, 0x80, 0x80);
                using var checkerBrush = new HatchBrush(HatchStyle.LargeCheckerBoard, lightColor, darkColor);
                e.Graphics.FillRectangle(checkerBrush, displayRect);
            }

            var prevInterpolationMode = e.Graphics.InterpolationMode;
            e.Graphics.InterpolationMode = _zoomFactor < 1 ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(_contentImage, displayRect);
            e.Graphics.InterpolationMode = prevInterpolationMode;

            e.Graphics.PixelOffsetMode = prevOffsetMode;
        }
    }
}
