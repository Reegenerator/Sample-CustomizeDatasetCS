namespace System.Windows.Forms.Design.Behavior
{
    using System;
    using System.Drawing;
    using System.Runtime;

    public sealed class Adorner
    {
        private System.Windows.Forms.Design.Behavior.BehaviorService behaviorService;
        private bool enabled = true;
        private GlyphCollection glyphs = new GlyphCollection();

        public void Invalidate()
        {
            if (this.behaviorService != null)
            {
                this.behaviorService.Invalidate();
            }
        }

        public void Invalidate(Rectangle rectangle)
        {
            if (this.behaviorService != null)
            {
                this.behaviorService.Invalidate(rectangle);
            }
        }

        public void Invalidate(Region region)
        {
            if (this.behaviorService != null)
            {
                this.behaviorService.Invalidate(region);
            }
        }

        public System.Windows.Forms.Design.Behavior.BehaviorService BehaviorService
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.behaviorService;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.behaviorService = value;
            }
        }

        public bool Enabled
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.EnabledInternal;
            }
            set
            {
                if (value != this.EnabledInternal)
                {
                    this.EnabledInternal = value;
                    this.Invalidate();
                }
            }
        }

        internal bool EnabledInternal
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.enabled;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.enabled = value;
            }
        }

        public GlyphCollection Glyphs
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.glyphs;
            }
        }
    }
}

