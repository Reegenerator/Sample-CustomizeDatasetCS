namespace System.Web.UI.Design
{
    using System;
    using System.Runtime;
    using System.Web.Compilation;

    public abstract class DesignTimeResourceProviderFactory
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected DesignTimeResourceProviderFactory()
        {
        }

        public abstract IResourceProvider CreateDesignTimeGlobalResourceProvider(IServiceProvider serviceProvider, string classKey);
        public abstract IResourceProvider CreateDesignTimeLocalResourceProvider(IServiceProvider serviceProvider);
        public abstract IDesignTimeResourceWriter CreateDesignTimeLocalResourceWriter(IServiceProvider serviceProvider);
    }
}

