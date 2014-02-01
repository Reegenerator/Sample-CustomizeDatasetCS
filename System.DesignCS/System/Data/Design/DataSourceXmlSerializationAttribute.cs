namespace System.Data.Design
{
    using System;
    using System.Runtime;

    internal abstract class DataSourceXmlSerializationAttribute : Attribute
    {
        private Type itemType;
        private string name;
        private bool specialWay = false;

        internal DataSourceXmlSerializationAttribute()
        {
        }

        public Type ItemType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.itemType;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.itemType = value;
            }
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

        public bool SpecialWay
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.specialWay;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.specialWay = value;
            }
        }
    }
}

