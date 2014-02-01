namespace System.Windows.Forms.Design
{
    using System;
    using System.Runtime;

    internal class StringArrayEditor : StringCollectionEditor
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public StringArrayEditor(Type type) : base(type)
        {
        }

        protected override Type CreateCollectionItemType()
        {
            return base.CollectionType.GetElementType();
        }

        protected override object[] GetItems(object editValue)
        {
            Array sourceArray = editValue as Array;
            if (sourceArray == null)
            {
                return new object[0];
            }
            object[] destinationArray = new object[sourceArray.GetLength(0)];
            Array.Copy(sourceArray, destinationArray, destinationArray.Length);
            return destinationArray;
        }

        protected override object SetItems(object editValue, object[] value)
        {
            if (!(editValue is Array) && (editValue != null))
            {
                return editValue;
            }
            Array destinationArray = Array.CreateInstance(base.CollectionItemType, value.Length);
            Array.Copy(value, destinationArray, value.Length);
            return destinationArray;
        }
    }
}

