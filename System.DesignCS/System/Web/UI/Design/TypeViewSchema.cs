namespace System.Web.UI.Design
{
    using System;
    using System.Runtime;
    using System.Security.Permissions;

    [SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.UnmanagedCode)]
    internal sealed class TypeViewSchema : BaseTypeViewSchema
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public TypeViewSchema(string viewName, Type type) : base(viewName, type)
        {
        }

        protected override Type GetRowType(Type objectType)
        {
            return objectType;
        }
    }
}

