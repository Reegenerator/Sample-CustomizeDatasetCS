namespace System.ComponentModel.Design
{
    using System;
    using System.Runtime;

    public sealed class DesignerActionHeaderItem : DesignerActionTextItem
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerActionHeaderItem(string displayName) : base(displayName, displayName)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerActionHeaderItem(string displayName, string category) : base(displayName, category)
        {
        }
    }
}

