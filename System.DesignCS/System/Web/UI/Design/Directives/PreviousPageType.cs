namespace System.Web.UI.Design.Directives
{
    using System;
    using System.Runtime;
    using System.Runtime.CompilerServices;
    using System.Web.UI;

    internal class PreviousPageType
    {
        [Filterable(false)]
        public string TypeName
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<TypeName>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<TypeName>k__BackingField = value;
            }
        }

        [UrlProperty("*.aspx"), Filterable(false)]
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

