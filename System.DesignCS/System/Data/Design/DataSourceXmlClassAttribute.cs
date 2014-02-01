namespace System.Data.Design
{
    using System;
    using System.Runtime;

    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class DataSourceXmlClassAttribute : Attribute
    {
        private string name;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal DataSourceXmlClassAttribute(string elementName)
        {
            this.name = elementName;
        }

        public string Name
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.name;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.name = value;
            }
        }
    }
}

