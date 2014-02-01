namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.Runtime;
    using System.Web.UI.Design.Util;
    using System.Windows.Forms;

    internal abstract class CollectionEditorDialog : DesignerForm
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected CollectionEditorDialog(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected ToolStripButton CreatePushButton(string toolTipText, int imageIndex)
        {
            return new ToolStripButton { Text = toolTipText, AutoToolTip = true, DisplayStyle = ToolStripItemDisplayStyle.Image, ImageIndex = imageIndex, ImageScaling = ToolStripItemImageScaling.SizeToFit };
        }
    }
}

