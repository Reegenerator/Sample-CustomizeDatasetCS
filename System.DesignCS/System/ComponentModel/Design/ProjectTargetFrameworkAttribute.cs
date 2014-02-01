namespace System.ComponentModel.Design
{
    using System;
    using System.Runtime;

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class ProjectTargetFrameworkAttribute : Attribute
    {
        private string _targetFrameworkMoniker;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ProjectTargetFrameworkAttribute(string targetFrameworkMoniker)
        {
            this._targetFrameworkMoniker = targetFrameworkMoniker;
        }

        public string TargetFrameworkMoniker
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._targetFrameworkMoniker;
            }
        }
    }
}

