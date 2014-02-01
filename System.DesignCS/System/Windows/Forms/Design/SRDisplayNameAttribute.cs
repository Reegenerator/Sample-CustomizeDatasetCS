namespace System.Windows.Forms.Design
{
    using System;
    using System.ComponentModel;
    using System.Design;
    using System.Runtime;

    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class)]
    internal sealed class SRDisplayNameAttribute : DisplayNameAttribute
    {
        private bool replaced;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public SRDisplayNameAttribute(string displayName) : base(displayName)
        {
        }

        public override string DisplayName
        {
            get
            {
                if (!this.replaced)
                {
                    this.replaced = true;
                    base.DisplayNameValue = System.Design.SR.GetString(base.DisplayName);
                }
                return base.DisplayName;
            }
        }
    }
}

