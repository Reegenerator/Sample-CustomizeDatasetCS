namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.Runtime;
    using System.Security.Permissions;
    using System.Web.UI.WebControls;

    [SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.UnmanagedCode)]
    public class TreeNodeStyleCollectionEditor : StyleCollectionEditor
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public TreeNodeStyleCollectionEditor(Type type) : base(type)
        {
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(TreeNodeStyle);
        }
    }
}

