namespace System.ComponentModel.Design.Data
{
    using System;
    using System.Runtime;

    public abstract class DesignerDataView : DesignerDataTableBase
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected DesignerDataView(string name) : base(name)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected DesignerDataView(string name, string owner) : base(name, owner)
        {
        }
    }
}

