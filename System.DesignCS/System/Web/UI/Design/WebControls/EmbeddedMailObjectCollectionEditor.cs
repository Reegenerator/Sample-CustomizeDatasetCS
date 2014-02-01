namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Runtime;
    using System.Security.Permissions;

    [SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.UnmanagedCode)]
    public class EmbeddedMailObjectCollectionEditor : CollectionEditor
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public EmbeddedMailObjectCollectionEditor(Type type) : base(type)
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            object obj2;
            try
            {
                context.OnComponentChanging();
                obj2 = base.EditValue(context, provider, value);
            }
            finally
            {
                context.OnComponentChanged();
            }
            return obj2;
        }
    }
}

