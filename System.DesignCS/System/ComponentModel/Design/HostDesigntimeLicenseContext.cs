namespace System.ComponentModel.Design
{
    using System;
    using System.Runtime;

    internal class HostDesigntimeLicenseContext : DesigntimeLicenseContext
    {
        private IServiceProvider provider;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public HostDesigntimeLicenseContext(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public override object GetService(Type serviceClass)
        {
            return this.provider.GetService(serviceClass);
        }
    }
}

