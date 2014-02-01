namespace System.Web.UI.Design.Directives
{
    using System;
    using System.Runtime;
    using System.Runtime.CompilerServices;
    using System.Web.UI;

    [SchemaElementName("OutputCache")]
    internal class OutputCacheAsix
    {
        [Filterable(false)]
        public bool DiskCacheable
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<DiskCacheable>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<DiskCacheable>k__BackingField = value;
            }
        }

        [Filterable(false)]
        public int Duration
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<Duration>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<Duration>k__BackingField = value;
            }
        }

        [Filterable(false)]
        public OutputCacheLocation Location
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<Location>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<Location>k__BackingField = value;
            }
        }

        [Filterable(false)]
        public string SqlDependency
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<SqlDependency>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<SqlDependency>k__BackingField = value;
            }
        }

        [Filterable(false)]
        public string VaryByContentEncoding
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<VaryByContentEncoding>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<VaryByContentEncoding>k__BackingField = value;
            }
        }

        [Filterable(false)]
        public string VaryByCustom
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<VaryByCustom>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<VaryByCustom>k__BackingField = value;
            }
        }

        [Filterable(false)]
        public string VaryByHeader
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<VaryByHeader>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<VaryByHeader>k__BackingField = value;
            }
        }

        [Filterable(false)]
        public string VaryByParam
        {
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.<VaryByParam>k__BackingField;
            }
            [CompilerGenerated, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.<VaryByParam>k__BackingField = value;
            }
        }
    }
}

