namespace System.Windows.Forms.Design.Behavior
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Design;
    using System.Drawing;
    using System.Runtime;
    using System.Windows.Forms;

    internal sealed class ToolStripPanelSelectionGlyph : ControlBodyGlyph
    {
        private Control baseParent;
        private BehaviorService behaviorService;
        private Rectangle glyphBounds;
        private Bitmap image;
        private int imageHeight;
        private const int imageHeightOriginal = 6;
        private int imageWidth;
        private const int imageWidthOriginal = 50;
        private bool isExpanded;
        private IServiceProvider provider;
        private ToolStripPanelSelectionBehavior relatedBehavior;
        private ToolStripPanel relatedPanel;

        internal ToolStripPanelSelectionGlyph(Rectangle bounds, Cursor cursor, IComponent relatedComponent, IServiceProvider provider, ToolStripPanelSelectionBehavior behavior) : base(bounds, cursor, relatedComponent, behavior)
        {
            this.imageWidth = 50;
            this.imageHeight = 6;
            this.relatedBehavior = behavior;
            this.provider = provider;
            this.relatedPanel = relatedComponent as ToolStripPanel;
            this.behaviorService = (BehaviorService) provider.GetService(typeof(BehaviorService));
            if ((this.behaviorService != null) && (((IDesignerHost) provider.GetService(typeof(IDesignerHost))) != null))
            {
                this.UpdateGlyph();
            }
        }

        private void CollapseGlyph(Rectangle bounds)
        {
            DockStyle dock = this.relatedPanel.Dock;
            int num = 0;
            int num2 = 0;
            switch (dock)
            {
                case DockStyle.Top:
                    this.SetBitmap("topopen.bmp");
                    num = (bounds.Width - this.imageWidth) / 2;
                    if (num <= 0)
                    {
                        break;
                    }
                    this.glyphBounds = new Rectangle(bounds.X + num, bounds.Y + bounds.Height, this.imageWidth, this.imageHeight);
                    return;

                case DockStyle.Bottom:
                    this.SetBitmap("bottomopen.bmp");
                    num = (bounds.Width - this.imageWidth) / 2;
                    if (num <= 0)
                    {
                        break;
                    }
                    this.glyphBounds = new Rectangle(bounds.X + num, bounds.Y - this.imageHeight, this.imageWidth, this.imageHeight);
                    return;

                case DockStyle.Left:
                    this.SetBitmap("leftopen.bmp");
                    num2 = (bounds.Height - this.imageWidth) / 2;
                    if (num2 <= 0)
                    {
                        break;
                    }
                    this.glyphBounds = new Rectangle(bounds.X + bounds.Width, bounds.Y + num2, this.imageHeight, this.imageWidth);
                    return;

                case DockStyle.Right:
                    this.SetBitmap("rightopen.bmp");
                    num2 = (bounds.Height - this.imageWidth) / 2;
                    if (num2 <= 0)
                    {
                        break;
                    }
                    this.glyphBounds = new Rectangle(bounds.X - this.imageHeight, bounds.Y + num2, this.imageHeight, this.imageWidth);
                    return;

                default:
                    throw new Exception(System.Design.SR.GetString("ToolStripPanelGlyphUnsupportedDock"));
            }
        }

        private void ExpandGlyph(Rectangle bounds)
        {
            DockStyle dock = this.relatedPanel.Dock;
            int num = 0;
            int num2 = 0;
            switch (dock)
            {
                case DockStyle.Top:
                    this.SetBitmap("topclose.bmp");
                    num = (bounds.Width - this.imageWidth) / 2;
                    if (num <= 0)
                    {
                        break;
                    }
                    this.glyphBounds = new Rectangle(bounds.X + num, bounds.Y + bounds.Height, this.imageWidth, this.imageHeight);
                    return;

                case DockStyle.Bottom:
                    this.SetBitmap("bottomclose.bmp");
                    num = (bounds.Width - this.imageWidth) / 2;
                    if (num <= 0)
                    {
                        break;
                    }
                    this.glyphBounds = new Rectangle(bounds.X + num, bounds.Y - this.imageHeight, this.imageWidth, this.imageHeight);
                    return;

                case DockStyle.Left:
                    this.SetBitmap("leftclose.bmp");
                    num2 = (bounds.Height - this.imageWidth) / 2;
                    if (num2 <= 0)
                    {
                        break;
                    }
                    this.glyphBounds = new Rectangle(bounds.X + bounds.Width, bounds.Y + num2, this.imageHeight, this.imageWidth);
                    return;

                case DockStyle.Right:
                    this.SetBitmap("rightclose.bmp");
                    num2 = (bounds.Height - this.imageWidth) / 2;
                    if (num2 <= 0)
                    {
                        break;
                    }
                    this.glyphBounds = new Rectangle(bounds.X - this.imageHeight, bounds.Y + num2, this.imageHeight, this.imageWidth);
                    return;

                default:
                    throw new Exception(System.Design.SR.GetString("ToolStripPanelGlyphUnsupportedDock"));
            }
        }

        public override Cursor GetHitTest(Point p)
        {
            if ((this.behaviorService != null) && (this.baseParent != null))
            {
                Rectangle rectangle = this.behaviorService.ControlRectInAdornerWindow(this.baseParent);
                if (((this.glyphBounds != Rectangle.Empty) && rectangle.Contains(this.glyphBounds)) && this.glyphBounds.Contains(p))
                {
                    return Cursors.Hand;
                }
            }
            return null;
        }

        public override void Paint(PaintEventArgs pe)
        {
            if ((this.behaviorService != null) && (this.baseParent != null))
            {
                Rectangle rectangle = this.behaviorService.ControlRectInAdornerWindow(this.baseParent);
                if ((this.relatedPanel.Visible && (this.image != null)) && ((this.glyphBounds != Rectangle.Empty) && rectangle.Contains(this.glyphBounds)))
                {
                    pe.Graphics.DrawImage(this.image, this.glyphBounds.Left, this.glyphBounds.Top);
                }
            }
        }

        private void SetBitmap(string fileName)
        {
            this.image = new Bitmap(typeof(ToolStripPanelSelectionGlyph), fileName);
            this.image.MakeTransparent(Color.Magenta);
            if (System.Windows.Forms.DpiHelper.IsScalingRequired)
            {
                Bitmap bitmap = null;
                if (this.image.Width > this.image.Height)
                {
                    this.imageWidth = System.Windows.Forms.DpiHelper.LogicalToDeviceUnitsX(50);
                    this.imageHeight = System.Windows.Forms.DpiHelper.LogicalToDeviceUnitsY(6);
                    bitmap = System.Windows.Forms.DpiHelper.CreateResizedBitmap(this.image, new Size(this.imageWidth, this.imageHeight));
                }
                else
                {
                    this.imageHeight = System.Windows.Forms.DpiHelper.LogicalToDeviceUnitsX(6);
                    this.imageWidth = System.Windows.Forms.DpiHelper.LogicalToDeviceUnitsY(50);
                    bitmap = System.Windows.Forms.DpiHelper.CreateResizedBitmap(this.image, new Size(this.imageHeight, this.imageWidth));
                }
                if (bitmap != null)
                {
                    this.image.Dispose();
                    this.image = bitmap;
                }
            }
        }

        public void UpdateGlyph()
        {
            if (this.behaviorService != null)
            {
                Rectangle bounds = this.behaviorService.ControlRectInAdornerWindow(this.relatedPanel);
                this.glyphBounds = Rectangle.Empty;
                ToolStripContainer parent = this.relatedPanel.Parent as ToolStripContainer;
                if (parent != null)
                {
                    this.baseParent = parent.Parent;
                }
                if (this.image != null)
                {
                    this.image.Dispose();
                    this.image = null;
                }
                if (!this.isExpanded)
                {
                    this.CollapseGlyph(bounds);
                }
                else
                {
                    this.ExpandGlyph(bounds);
                }
            }
        }

        public override Rectangle Bounds
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.glyphBounds;
            }
        }

        public bool IsExpanded
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.isExpanded;
            }
            set
            {
                if (value != this.isExpanded)
                {
                    this.isExpanded = value;
                    this.UpdateGlyph();
                }
            }
        }
    }
}

