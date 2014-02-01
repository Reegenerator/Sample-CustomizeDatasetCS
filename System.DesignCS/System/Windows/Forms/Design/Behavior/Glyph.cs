namespace System.Windows.Forms.Design.Behavior
{
    using System;
    using System.Drawing;
    using System.Runtime;
    using System.Windows.Forms;

    public abstract class Glyph
    {
        private System.Windows.Forms.Design.Behavior.Behavior behavior;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected Glyph(System.Windows.Forms.Design.Behavior.Behavior behavior)
        {
            this.behavior = behavior;
        }

        public abstract Cursor GetHitTest(Point p);
        public abstract void Paint(PaintEventArgs pe);
        protected void SetBehavior(System.Windows.Forms.Design.Behavior.Behavior behavior)
        {
            this.behavior = behavior;
        }

        public virtual System.Windows.Forms.Design.Behavior.Behavior Behavior
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.behavior;
            }
        }

        public virtual Rectangle Bounds
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return Rectangle.Empty;
            }
        }
    }
}

