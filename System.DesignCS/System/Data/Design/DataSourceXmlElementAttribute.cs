namespace System.Data.Design
{
    using System;
    using System.Runtime;

    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class DataSourceXmlElementAttribute : DataSourceXmlSerializationAttribute
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal DataSourceXmlElementAttribute() : this(null)
        {
        }

        internal DataSourceXmlElementAttribute(string elementName)
        {
            base.Name = elementName;
        }
    }
}

