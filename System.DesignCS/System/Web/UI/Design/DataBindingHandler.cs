namespace System.Web.UI.Design
{
    using System;
    using System.ComponentModel.Design;
    using System.Runtime;
    using System.Security.Permissions;
    using System.Web.UI;

    [SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.UnmanagedCode)]
    public abstract class DataBindingHandler
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected DataBindingHandler()
        {
        }

        public abstract void DataBindControl(IDesignerHost designerHost, Control control);
    }
}

