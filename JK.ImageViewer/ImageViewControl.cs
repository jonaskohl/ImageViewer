using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace JK.ImageViewer
{
    internal class ImageViewControl : Control
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

        private void Repaint()
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_contentImage is null)
                return;

            var displayImageWidth = _contentImage.Width * _zoomFactor;
            var displayImageHeight = _contentImage.Height * _zoomFactor;
            var posX = Math.Max(0, (Width - displayImageWidth) / 2f);
            var posY = Math.Max(0, (Height - displayImageHeight) / 2f);

            var displayRect = new RectangleF(posX, posY, displayImageWidth, displayImageHeight);

            if (_showCheckerboard)
            {
                var lightColor = Application.IsDarkModeEnabled ? Color.FromArgb(0x44, 0x44, 0x44) : Color.FromArgb(0xC0, 0xC0, 0xC0);
                var darkColor = Application.IsDarkModeEnabled ? Color.FromArgb(0x11, 0x11, 0x11) : Color.FromArgb(0x80, 0x80, 0x80);
                using var checkerBrush = new HatchBrush(HatchStyle.LargeCheckerBoard, lightColor, darkColor);
                e.Graphics.FillRectangle(checkerBrush, displayRect);
            }

            var prev = e.Graphics.InterpolationMode;
            e.Graphics.InterpolationMode = _zoomFactor < 1 ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(_contentImage, displayRect);
            e.Graphics.InterpolationMode = prev;
        }
    }
}
