namespace System.Windows.Forms.Design
{
    using System;
    using System.Drawing;
    using System.Runtime;
    using System.Windows.Forms;
    using System.Windows.Forms.Design.Behavior;
    using System.Windows.Forms.VisualStyles;

    internal class ToolStripItemGlyph : ControlBodyGlyph
    {
        private Rectangle _bounds;
        private ToolStripItem _item;
        private ToolStripItemDesigner _itemDesigner;

        public ToolStripItemGlyph(ToolStripItem item, ToolStripItemDesigner itemDesigner, Rectangle bounds, System.Windows.Forms.Design.Behavior.Behavior b) : base(bounds, Cursors.Default, item, b)
        {
            this._item = item;
            this._bounds = bounds;
            this._itemDesigner = itemDesigner;
        }

        public override Cursor GetHitTest(Point p)
        {
            if (this._item.Visible && this._bounds.Contains(p))
            {
                return Cursors.Default;
            }
            return null;
        }

        public override void Paint(PaintEventArgs pe)
        {
            if (((this._item is ToolStripControlHost) && this._item.IsOnDropDown) && (!(this._item is ToolStripComboBox) || !VisualStyleRenderer.IsSupported))
            {
                this._item.Invalidate();
            }
        }

        public override Rectangle Bounds
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._bounds;
            }
        }

        public ToolStripItem Item
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._item;
            }
        }

        public ToolStripItemDesigner ItemDesigner
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._itemDesigner;
            }
        }
    }
}

