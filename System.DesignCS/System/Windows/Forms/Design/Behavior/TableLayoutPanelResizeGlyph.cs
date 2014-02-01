namespace System.Windows.Forms.Design.Behavior
{
    using System;
    using System.Drawing;
    using System.Runtime;
    using System.Windows.Forms;

    internal class TableLayoutPanelResizeGlyph : Glyph
    {
        private Rectangle bounds;
        private Cursor hitTestCursor;
        private TableLayoutStyle style;
        private TableLayoutResizeType type;

        internal TableLayoutPanelResizeGlyph(Rectangle controlBounds, TableLayoutStyle style, Cursor hitTestCursor, System.Windows.Forms.Design.Behavior.Behavior behavior) : base(behavior)
        {
            this.bounds = controlBounds;
            this.hitTestCursor = hitTestCursor;
            this.style = style;
            if (style is ColumnStyle)
            {
                this.type = TableLayoutResizeType.Column;
            }
            else
            {
                this.type = TableLayoutResizeType.Row;
            }
        }

        public override Cursor GetHitTest(Point p)
        {
            if (this.bounds.Contains(p))
            {
                return this.hitTestCursor;
            }
            return null;
        }

        public override void Paint(PaintEventArgs pe)
        {
        }

        public override Rectangle Bounds
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.bounds;
            }
        }

        public TableLayoutStyle Style
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.style;
            }
        }

        public TableLayoutResizeType Type
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.type;
            }
        }

        public enum TableLayoutResizeType
        {
            Column,
            Row
        }
    }
}

