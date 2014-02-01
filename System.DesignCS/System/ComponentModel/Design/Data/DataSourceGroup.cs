namespace System.ComponentModel.Design.Data
{
    using System;
    using System.Drawing;
    using System.Runtime;

    public abstract class DataSourceGroup
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected DataSourceGroup()
        {
        }

        public abstract DataSourceDescriptorCollection DataSources { get; }

        public abstract Bitmap Image { get; }

        public abstract bool IsDefault { get; }

        public abstract string Name { get; }
    }
}

