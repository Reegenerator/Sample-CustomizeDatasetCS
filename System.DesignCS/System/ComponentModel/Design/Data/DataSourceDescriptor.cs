namespace System.ComponentModel.Design.Data
{
    using System;
    using System.Drawing;
    using System.Runtime;

    public abstract class DataSourceDescriptor
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected DataSourceDescriptor()
        {
        }

        public abstract Bitmap Image { get; }

        public abstract bool IsDesignable { get; }

        public abstract string Name { get; }

        public abstract string TypeName { get; }
    }
}

