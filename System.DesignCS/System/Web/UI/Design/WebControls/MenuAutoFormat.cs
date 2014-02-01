namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.Runtime;

    internal sealed class MenuAutoFormat : ReflectionBasedAutoFormat
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public MenuAutoFormat(string schemeName, string schemes) : base(schemeName, schemes)
        {
        }
    }
}

