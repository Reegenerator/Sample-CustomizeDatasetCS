namespace System.Web.UI.Design.Directives
{
    using System;
    using System.Runtime;
    using System.Runtime.CompilerServices;
    using System.Web.UI;

    internal class Reference
    {
        [Filterable(false), UrlProperty("*.ascx")]
        public string Control
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<Control>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<Control>k__BackingField = value;
            }
        }

        [UrlProperty("*.aspx"), Filterable(false)]
        public string Page
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<Page>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<Page>k__BackingField = value;
            }
        }

        [Filterable(false), UrlProperty("*.aspx;*.ascx")]
        public string VirtualPath
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<VirtualPath>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<VirtualPath>k__BackingField = value;
            }
        }
    }
}

