namespace System.Web.UI.Design
{
    using System;
    using System.Reflection;
    using System.Runtime;
    using System.Security.Permissions;

    [SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.UnmanagedCode)]
    internal sealed class TypeEnumerableViewSchema : BaseTypeViewSchema
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public TypeEnumerableViewSchema(string viewName, Type type) : base(viewName, type)
        {
        }

        protected override Type GetRowType(Type objectType)
        {
            if (objectType.IsArray)
            {
                return objectType.GetElementType();
            }
            foreach (PropertyInfo info in objectType.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance))
            {
                if (info.GetIndexParameters().Length > 0)
                {
                    return info.PropertyType;
                }
            }
            return null;
        }
    }
}

