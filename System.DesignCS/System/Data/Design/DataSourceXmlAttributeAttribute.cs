namespace System.Data.Design
{
    using System;
    using System.Runtime;

    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class DataSourceXmlAttributeAttribute : DataSourceXmlSerializationAttribute
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal DataSourceXmlAttributeAttribute() : this(null)
        {
        }

        internal DataSourceXmlAttributeAttribute(string attributeName)
        {
            base.Name = attributeName;
        }
    }
}

