using ImageMagick;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace JK.ImageViewer.Controls
{
    internal class ImageViewControl : ScrollableControl
    {
        public enum ToolMode
        {
            None,
            Marquee,
            Line,
        }

        const int LAYOUT_ITERATIONS = 2;

        private Bitmap? frameCache = null;
        private Bitmap? imageCache = null;

        public event EventHandler? ZoomFactorChanged;
        public event EventHandler<Rectangle>? MarqueeSelectionCreated;
        public event EventHandler<(Point From, Point To)>? LineCreated;

        private ToolMode _currentToolMode = ToolMode.None;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ToolMode CurrentToolMode
        {
            get => _currentToolMode;
            set
            {
                _currentToolMode = value;
                UpdateCursor();
            }
        }

        private bool _reverseWheel = false;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ReverseWheel
        {
            get => _reverseWheel;
            set => _reverseWheel = value;
        }

        private float _zoomFactor = 1.0f;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float ZoomFactor
        {
            get => _zoomFactor;
            set
            {
                _zoomFactor = MathUtil.Clamp(Constants.ZOOM_FACTOR_MIN, value, Constants.ZOOM_FACTOR_MAX);
                ZoomFactorChanged?.Invoke(this, EventArgs.Empty);
                ForceRecreateImageCache();
                InvalidateFrameCache();
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
                InvalidateFrameCache();
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
                UpdateCursor();
                ForceRecreateImageCache();
                InvalidateFrameCache();
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
                UpdateCursor();
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
                ForceRecreateImageCache();
                InvalidateFrameCache();
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
                ForceRecreateImageCache();
                InvalidateFrameCache();
            }
        }

        public ImageViewControl()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor
                | ControlStyles.ResizeRedraw
            , true);
        }

        private void UpdateCursor()
        {
            if (ContentImage is null || ImageLoadException is not null)
            {
                Cursor = Cursors.Arrow;
                return;
            }

            switch (_currentToolMode)
            {
                case ToolMode.Marquee:
                case ToolMode.Line:
                    Cursor = Cursors.Cross;
                    break;
                case ToolMode.None:
                default:
                    Cursor = Cursors.Arrow;
                    break;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (_reverseWheel)
                e = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, -e.Delta);

            var oldZoomFactor = ZoomFactor;
            var oldScrollPosX = AutoScrollPosition.X;
            var oldScrollPosY = AutoScrollPosition.Y;
            if (ModifierKeys == Keys.Control)
            {
                var add = (e.Delta / 1440f) * ZoomFactor;
                ZoomFactor = MathUtil.Clamp(Constants.ZOOM_FACTOR_MIN, ZoomFactor + add, Constants.ZOOM_FACTOR_MAX);
            }
            else if (ModifierKeys == Keys.Shift)
            {
                var vscroll = VScroll;
                VScroll = false;
                base.OnMouseWheel(e);
                VScroll = vscroll;
            }
            else
                base.OnMouseWheel(e);

            if (oldZoomFactor != ZoomFactor || oldScrollPosX != AutoScrollPosition.X || oldScrollPosY != AutoScrollPosition.Y)
            {
                InvalidateFrameCache();
            }
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            if (se.OldValue != se.NewValue)
            {
                InvalidateFrameCache();
            }
        }

        private void Repaint()
        {
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            InvalidateFrameCache();
        }

        bool isDragging = false;
        Point dragStart;
        Point dragEnd;

        protected bool CanDrag => CurrentToolMode != ToolMode.None && ModifierKeys == Keys.None && ContentImage is not null;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left && CanDrag)
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

            var isRightAnchored = b.X < a.X;
            var isBottomAnchored = b.Y < a.Y;

            var w = right - left;
            var h = bottom - top;

            if (ModifierKeys == Keys.Shift)
            {
                var s = Math.Min(w, h);

                var xOffs = isRightAnchored
                    ? (w - s)
                    : 0;
                var yOffs = isBottomAnchored
                    ? (h - s)
                    : 0;

                w = h = s;
                left += xOffs;
                top += yOffs;
            }

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

            if (e.Button == MouseButtons.Left && isDragging)
            {
                isDragging = false;
                if (!dragStart.Equals(dragEnd))
                {
                    switch (CurrentToolMode)
                    {
                        case ToolMode.Marquee:
                            MarqueeSelectionCreated?.Invoke(this, RectangleToImage(RectFromPoints(dragStart, dragEnd)));
                            break;
                        case ToolMode.Line:
                            LineCreated?.Invoke(this, (PointToImage(dragStart), PointToImage(dragEnd)));
                            break;
                        case ToolMode.None:
                        default:
                            break;
                    }
                }
                Repaint();
            }
        }

        public void SetZoomRegion(Point topLeft, Point bottomRight)
        {
            SetZoomRegion(RectFromPoints(topLeft, bottomRight));
        }

        public void SetZoomRegion(Rectangle regionInImageSpace)
        {
            if (ContentImage is null)
                return;

            var factor = Math.Min(
                (float)ContentImage.Width / Math.Max(1, regionInImageSpace.Width),
                (float)ContentImage.Height / Math.Max(1, regionInImageSpace.Height)
            );

            var newOffset = new Point(
                (int)(regionInImageSpace.X / ZoomFactor),
                (int)(regionInImageSpace.Y / ZoomFactor)
            );

            ZoomFactor = factor;
            InvalidateFrameCache();
            Application.DoEvents();
            PerformLayoutCalculation();
            Application.DoEvents();
            PerformLayout(this, nameof(AutoScrollMinSize));
            Application.DoEvents();
            AutoScrollPosition = newOffset;
            InvalidateFrameCache();
        }

        private Point PointToImage(Point pt)
        {
            if (ContentImage is null)
                return Point.Empty;

            var innerWidth = ClientSize.Width;
            var innerHeight = ClientSize.Height;

            var displayImageWidth = ContentImage.Width * ZoomFactor;
            var displayImageHeight = ContentImage.Height * ZoomFactor;
            var hOverflow = displayImageWidth - innerWidth;
            var vOverflow = displayImageHeight - innerHeight;

            var posX = Math.Max(0, (innerWidth - displayImageWidth) / 2f) + AutoScrollPosition.X;
            var posY = Math.Max(0, (innerHeight - displayImageHeight) / 2f) + AutoScrollPosition.Y;

            return new Point(
                (int)((pt.X - posX) / ZoomFactor),
                (int)((pt.Y - posY) / ZoomFactor)
            );
        }

        private Rectangle RectangleToImage(Rectangle region)
        {
            if (ContentImage is null)
                return Rectangle.Empty;

            var innerWidth = ClientSize.Width;
            var innerHeight = ClientSize.Height;

            var displayImageWidth = ContentImage.Width * ZoomFactor;
            var displayImageHeight = ContentImage.Height * ZoomFactor;

            var posX = Math.Max(0, (innerWidth - displayImageWidth) / 2f) + AutoScrollPosition.X;
            var posY = Math.Max(0, (innerHeight - displayImageHeight) / 2f) + AutoScrollPosition.Y;

            return new Rectangle(
                (int)((region.X - posX) / ZoomFactor),
                (int)((region.Y - posY) / ZoomFactor),
                (int)(region.Width / ZoomFactor),
                (int)(region.Height / ZoomFactor)
            );
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
                    ClientRectangle,
                    Application.IsDarkModeEnabled ? Color.Pink : Color.Red,
                    TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);

                return;
            }

            if (ContentImage is null)
            {
                AutoScrollMinSize = Size.Empty;
                return;
            }

            int maxTries = 0xFF;

            while (frameCache is null && (maxTries--) >= 0)
            {
                Debug.WriteLine($"No frame cache: Recreating {maxTries + 1}");
                RecreateFrameCache();
            }

            if (frameCache is null)
            {
                Debug.WriteLine("Still no frame cache; aborting rest of paint operation");
                return;
            }

            e.Graphics.DrawImageUnscaled(frameCache, Point.Empty);

            if (isDragging)
            {
                var prevOffsetMode = e.Graphics.PixelOffsetMode;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var pen = new Pen(Color.Black, 1f)
                {
                    DashStyle = DashStyle.Dot,
                };

                if (CurrentToolMode == ToolMode.Marquee)
                {
                    var rect = RectFromPoints(dragStart, dragEnd);
                    e.Graphics.DrawRectangle(Pens.White, rect);
                    e.Graphics.DrawRectangle(pen, rect);
                }
                else if (CurrentToolMode == ToolMode.Line)
                {
                    e.Graphics.DrawLine(Pens.White, dragStart, dragEnd);
                    e.Graphics.DrawLine(pen, dragStart, dragEnd);
                }

                e.Graphics.PixelOffsetMode = prevOffsetMode;
            }
        }

        private void InvalidateFrameCache()
        {
            frameCache?.Dispose();
            frameCache = null;
            Repaint();
        }

        private void ForceRecreateImageCache()
        {
            imageCache?.Dispose();
            
            if (_contentImage is null)
            {
                Debug.WriteLine("ForceRecreateImageCache -> Content image is null!");
                imageCache = null;
                return;
            }

            var newWidth = (int)(ContentImage.Width * ZoomFactor);
            var newHeight = (int)(ContentImage.Height * ZoomFactor);

            imageCache = new Bitmap(newWidth, newHeight);
            using var g = Graphics.FromImage(imageCache);

            var prevInterpolationMode = g.InterpolationMode;
            g.InterpolationMode = _zoomFactor < 1 ? _downsampleMode : _upsampleMode;
            g.DrawImage(ContentImage, new Rectangle(
                0,
                0,
                newWidth,
                newHeight
            ));
            g.InterpolationMode = prevInterpolationMode;
        }

        private void RecreateFrameCache()
        {
            if (ContentImage is null)
            {
                Debug.WriteLine("Not recreating frame cache: No content image");
                return;
            }

            if (_imageLoadException is not null)
            {
                Debug.WriteLine("Not recreating frame cache: In exception state");
                return;
            }

            if (frameCache is not null)
            {
                Debug.WriteLine("Not recreating frame cache: Frame cache still valid");
                return;
            }

            frameCache?.Dispose();
            frameCache = new Bitmap(ClientSize.Width, ClientSize.Height);

            if (frameCache is null)
            {
                Debug.WriteLine("Frame cache is null directly after creation!!!");
            }
            else
            {
                Debug.WriteLine("Frame cache object created");
            }

            if (imageCache is null)
                ForceRecreateImageCache();

            using var g = Graphics.FromImage(frameCache!);

            var prevOffsetMode = g.PixelOffsetMode;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var displayRect = PerformLayoutCalculation();

            if (_showCheckerboard)
            {
                var lightColor = Application.IsDarkModeEnabled ? Color.FromArgb(0x44, 0x44, 0x44) : Color.FromArgb(0xC0, 0xC0, 0xC0);
                var darkColor = Application.IsDarkModeEnabled ? Color.FromArgb(0x11, 0x11, 0x11) : Color.FromArgb(0x80, 0x80, 0x80);
                using var checkerBrush = new HatchBrush(HatchStyle.LargeCheckerBoard, lightColor, darkColor);
                g.FillRectangle(checkerBrush, displayRect);
            }

            var prevInterpolationMode = g.InterpolationMode;
            g.InterpolationMode = InterpolationMode.Low;
            g.DrawImageUnscaled(imageCache!, new Point(
                (int)displayRect.X,
                (int)displayRect.Y
            ));
            g.InterpolationMode = prevInterpolationMode;
            g.PixelOffsetMode = prevOffsetMode;
        }

        private RectangleF PerformLayoutCalculation()
        {
            if (ContentImage is null)
                return RectangleF.Empty;

            RectangleF displayRect = RectangleF.Empty;
            for (int i = 0; i < LAYOUT_ITERATIONS; ++i)
            {
                var innerWidth = ClientSize.Width;
                var innerHeight = ClientSize.Height;

                var displayImageWidth = ContentImage.Width * ZoomFactor;
                var displayImageHeight = ContentImage.Height * ZoomFactor;
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

            return displayRect;
        }

        public float GetBestFitZoomFactor()
        {
            if (ContentImage is null)
                return 1f;

            return Math.Min(
                Width / (float)ContentImage.Width,
                Height / (float)ContentImage.Height
            );
        }
    }
}
