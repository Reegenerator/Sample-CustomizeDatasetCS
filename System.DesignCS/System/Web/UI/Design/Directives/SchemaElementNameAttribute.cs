namespace System.Web.UI.Design.Directives
{
    using System;
    using System.Runtime;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SchemaElementNameAttribute : Attribute
    {
        public SchemaElementNameAttribute(string value)
        {
            this.Value = value;
        }

        public string Value
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<Value>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Value>k__BackingField = value;
            }
        }
    }
}

