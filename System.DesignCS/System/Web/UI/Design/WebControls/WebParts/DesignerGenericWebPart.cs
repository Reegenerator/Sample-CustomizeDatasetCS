namespace System.Web.UI.Design.WebControls.WebParts
{
    using System;
    using System.Runtime;
    using System.Web.UI;
    using System.Web.UI.WebControls.WebParts;

    internal sealed class DesignerGenericWebPart : GenericWebPart
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerGenericWebPart(Control control) : base(control)
        {
        }

        protected internal override void CreateChildControls()
        {
        }
    }
}

