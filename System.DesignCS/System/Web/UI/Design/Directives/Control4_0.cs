﻿namespace System.Web.UI.Design.Directives
{
    using System;
    using System.ComponentModel;
    using System.Runtime;
    using System.Runtime.CompilerServices;
    using System.Web.UI;

    [SchemaElementName("Control")]
    internal class Control4_0 : System.Web.UI.Design.Directives.Control
    {
        [DefaultValue("Inherit")]
        public System.Web.UI.ClientIDMode ClientIDMode
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<ClientIDMode>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<ClientIDMode>k__BackingField = value;
            }
        }

        [Browsable(false), Filterable(false)]
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

        public System.Web.UI.ViewStateMode ViewStateMode
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<ViewStateMode>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<ViewStateMode>k__BackingField = value;
            }
        }
    }
}

