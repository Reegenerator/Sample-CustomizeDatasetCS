namespace System.Web.UI.Design
{
    using System;
    using System.Runtime;
    using System.Web.UI.WebControls;

    public class DesignerAutoFormatStyle : Style
    {
        private System.Web.UI.WebControls.VerticalAlign _verticalAlign;

        public System.Web.UI.WebControls.VerticalAlign VerticalAlign
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._verticalAlign;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._verticalAlign = value;
            }
        }
    }
}

