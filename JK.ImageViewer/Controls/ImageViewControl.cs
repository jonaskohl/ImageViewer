using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using ImageMagick;
using System.Windows.Forms.VisualStyles;

namespace JK.ImageViewer.Controls
{
    internal class ImageViewControl : ScrollableControl
    {
        public event EventHandler? ZoomFactorChanged;

        private float _zoomFactor = 1.0f;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float ZoomFactor
        {
            get => _zoomFactor;
            set
            {
                _zoomFactor = value;
                ZoomFactorChanged?.Invoke(this, EventArgs.Empty);
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

        private Image? _contentImage = null;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image? ContentImage
        {
            get => _contentImage;
            set
            {
                _contentImage = value;
                Repaint();
            }
        }

        private Exception? _imageLoadException = null;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Exception? ImageLoadException
        {
            get => _imageLoadException;
            set
            {
                _imageLoadException = value;
                Repaint();
            }
        }

        private InterpolationMode _downsampleMode = InterpolationMode.HighQualityBicubic;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public InterpolationMode DownsampleMode
        {
            get => _downsampleMode;
            set
            {
                _downsampleMode = value;
                Repaint();
            }
        }

        private InterpolationMode _upsampleMode = InterpolationMode.NearestNeighbor;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public InterpolationMode UpsampleMode
        {
            get => _upsampleMode;
            set
            {
                _upsampleMode = value;
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

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                var add = (e.Delta / 1440f) * ZoomFactor;
                ZoomFactor = MathUtil.Clamp(Constants.ZOOM_FACTOR_MIN, ZoomFactor + add, Constants.ZOOM_FACTOR_MAX);
            }
            else
                base.OnMouseWheel(e);
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

        bool isDragging = false;
        Point dragStart;
        Point dragEnd;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Right && ModifierKeys == Keys.Shift && ContentImage is not null)
            {
                isDragging = true;
                dragStart = e.Location;
                dragEnd = e.Location;
                Repaint();
            }
        }

        protected static Rectangle RectFromPoints(Point a, Point b)
        {
            var top = Math.Min(a.Y, b.Y);
            var bottom = Math.Max(a.Y, b.Y);
            var left = Math.Min(a.X, b.X);
            var right = Math.Max(a.X, b.X);

            var w = right - left;
            var h = bottom - top;
            return new Rectangle(left, top, w, h);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (isDragging)
            {
                dragEnd = e.Location;
                Repaint();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Right && isDragging)
            {
                isDragging = false;
                SetZoomRegion(dragStart, dragEnd);
            }
        }

        public void SetZoomRegion(Point topLeft, Point bottomRight)
        {
            SetZoomRegion(RectFromPoints(topLeft, bottomRight));
        }

        public void SetZoomRegion(Rectangle region)
        {
            if (ContentImage is null)
                return;

            var factor = Math.Min(
                (float)ContentImage.Width / Math.Max(1, region.Width),
                (float)ContentImage.Height / Math.Max(1, region.Height)
            );

            var startOffsetInImage = new Point(
                (int)((region.X - AutoScrollPosition.X) / ZoomFactor),
                (int)((region.Y - AutoScrollPosition.Y) / ZoomFactor)
            );

            var newOffset = new Point(
                (int)(startOffsetInImage.X * ZoomFactor),
                (int)(startOffsetInImage.Y * ZoomFactor)
            );

            ZoomFactor = factor;
            AutoScrollPosition = newOffset;
            Repaint();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            isDragging = false;
            Repaint();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_imageLoadException is not null)
            {
                AutoScrollMinSize = Size.Empty;

                TextRenderer.DrawText(
                    e.Graphics,
                    _imageLoadException.ToString(),
                    Font,
                    new Rectangle(0, 0, Math.Min(300, ClientSize.Width), ClientSize.Height),
                    Application.IsDarkModeEnabled ? Color.Pink : Color.Red,
                    TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);

                return;
            }

            if (_contentImage is null)
            {
                AutoScrollMinSize = Size.Empty;
                return;
            }

            var prevOffsetMode = e.Graphics.PixelOffsetMode;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            RectangleF displayRect = RectangleF.Empty;
            for (int i = 0; i < 2; ++i)
            {
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

                displayRect = new RectangleF(posX, posY, displayImageWidth, displayImageHeight);
            }

            if (_showCheckerboard)
            {
                var lightColor = Application.IsDarkModeEnabled ? Color.FromArgb(0x44, 0x44, 0x44) : Color.FromArgb(0xC0, 0xC0, 0xC0);
                var darkColor = Application.IsDarkModeEnabled ? Color.FromArgb(0x11, 0x11, 0x11) : Color.FromArgb(0x80, 0x80, 0x80);
                using var checkerBrush = new HatchBrush(HatchStyle.LargeCheckerBoard, lightColor, darkColor);
                e.Graphics.FillRectangle(checkerBrush, displayRect);
            }

            var prevInterpolationMode = e.Graphics.InterpolationMode;
            e.Graphics.InterpolationMode = _zoomFactor < 1 ? _downsampleMode : _upsampleMode;
            e.Graphics.DrawImage(_contentImage, displayRect);
            e.Graphics.InterpolationMode = prevInterpolationMode;

            if (isDragging)
                e.Graphics.DrawRectangle(Pens.Red, RectFromPoints(dragStart, dragEnd));
                //ControlPaint.DrawReversibleFrame(RectangleToScreen(RectFromPoints(dragStart, dragEnd)), Color.Black, FrameStyle.Thick);

            e.Graphics.PixelOffsetMode = prevOffsetMode;
        }
    }
}
