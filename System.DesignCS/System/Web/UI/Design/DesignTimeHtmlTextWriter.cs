namespace System.Web.UI.Design
{
    using System;
    using System.IO;
    using System.Runtime;
    using System.Security;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.UI;

    [SecurityCritical, AspNetHostingPermission(SecurityAction.InheritanceDemand, Level=AspNetHostingPermissionLevel.Minimal)]
    internal class DesignTimeHtmlTextWriter : HtmlTextWriter
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignTimeHtmlTextWriter(TextWriter writer) : base(writer)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignTimeHtmlTextWriter(TextWriter writer, string tabString) : base(writer, tabString)
        {
        }

        public override void AddAttribute(HtmlTextWriterAttribute key, string value)
        {
            if (((key == HtmlTextWriterAttribute.Src) || (key == HtmlTextWriterAttribute.Href)) || (key == HtmlTextWriterAttribute.Background))
            {
                base.AddAttribute(key.ToString(), value, key);
            }
            else
            {
                base.AddAttribute(key, value);
            }
        }
    }
}

