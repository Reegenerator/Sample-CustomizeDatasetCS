namespace System.Windows.Forms.Design
{
    using System;
    using System.ComponentModel;
    using System.Design;
    using System.Runtime;
    using System.Windows.Forms;

    internal class ListControlStringCollectionEditor : StringCollectionEditor
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ListControlStringCollectionEditor(System.Type type) : base(type)
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            ListControl instance = context.Instance as ListControl;
            if ((instance != null) && (instance.DataSource != null))
            {
                throw new ArgumentException(System.Design.SR.GetString("DataSourceLocksItems"));
            }
            return base.EditValue(context, provider, value);
        }
    }
}

