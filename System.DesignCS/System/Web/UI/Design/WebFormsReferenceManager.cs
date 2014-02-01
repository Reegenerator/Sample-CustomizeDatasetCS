namespace System.Web.UI.Design
{
    using System;
    using System.Collections;
    using System.Runtime;

    public abstract class WebFormsReferenceManager
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected WebFormsReferenceManager()
        {
        }

        public abstract ICollection GetRegisterDirectives();
        public abstract string GetTagPrefix(Type objectType);
        public abstract Type GetType(string tagPrefix, string tagName);
        public abstract string GetUserControlPath(string tagPrefix, string tagName);
        public abstract string RegisterTagPrefix(Type objectType);
    }
}

