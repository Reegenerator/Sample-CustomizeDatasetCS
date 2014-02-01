namespace System.Web.UI.Design
{
    using System;
    using System.Runtime;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SupportsPreviewControlAttribute : Attribute
    {
        private bool _supportsPreviewControl;
        public static readonly SupportsPreviewControlAttribute Default = new SupportsPreviewControlAttribute(false);

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public SupportsPreviewControlAttribute(bool supportsPreviewControl)
        {
            this._supportsPreviewControl = supportsPreviewControl;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            SupportsPreviewControlAttribute attribute = obj as SupportsPreviewControlAttribute;
            return ((attribute != null) && (attribute.SupportsPreviewControl == this._supportsPreviewControl));
        }

        public override int GetHashCode()
        {
            return this._supportsPreviewControl.GetHashCode();
        }

        public override bool IsDefaultAttribute()
        {
            return this.Equals(Default);
        }

        public bool SupportsPreviewControl
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._supportsPreviewControl;
            }
        }
    }
}

