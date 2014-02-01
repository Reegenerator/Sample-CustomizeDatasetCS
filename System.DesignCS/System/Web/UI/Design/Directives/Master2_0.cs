namespace System.Web.UI.Design.Directives
{
    using System;
    using System.Runtime;
    using System.Runtime.CompilerServices;
    using System.Web.UI;

    [SchemaElementName("Master")]
    internal class Master2_0 : Master
    {
        [Filterable(false)]
        public string Description
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<Description>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<Description>k__BackingField = value;
            }
        }
    }
}

