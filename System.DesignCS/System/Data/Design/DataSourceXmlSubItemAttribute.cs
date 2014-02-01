namespace System.Data.Design
{
    using System;
    using System.Runtime;

    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class DataSourceXmlSubItemAttribute : DataSourceXmlSerializationAttribute
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal DataSourceXmlSubItemAttribute()
        {
        }

        internal DataSourceXmlSubItemAttribute(Type itemType)
        {
            base.ItemType = itemType;
        }
    }
}

