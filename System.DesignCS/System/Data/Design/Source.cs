namespace System.Data.Design
{
    using System;
    using System.CodeDom;
    using System.ComponentModel;
    using System.Runtime;

    internal abstract class Source : DataSourceComponent, IDataSourceNamedObject, INamedObject, ICloneable
    {
        private string generatorGetMethodName;
        private string generatorGetMethodNameForPaging;
        private string generatorSourceName;
        private string generatorSourceNameForPaging;
        private MemberAttributes modifier = MemberAttributes.Public;
        protected string name;
        protected DataSourceComponent owner;
        private string userSourceName;
        private bool webMethod;
        private string webMethodDescription;

        internal Source()
        {
        }

        public abstract object Clone();
        internal virtual bool NameExist(string nameToCheck)
        {
            return StringUtil.EqualValue(this.Name, nameToCheck, true);
        }

        public override void SetCollection(DataSourceCollectionBase collection)
        {
            base.SetCollection(collection);
            if (collection != null)
            {
                this.Owner = collection.CollectionHost;
            }
            else
            {
                this.Owner = null;
            }
        }

        public override string ToString()
        {
            return (this.PublicTypeName + " " + this.DisplayName);
        }

        internal virtual string DisplayName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.Name;
            }
            set
            {
            }
        }

        [DataSourceXmlAttribute, DefaultValue(false)]
        public bool EnableWebMethods
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.webMethod;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.webMethod = value;
            }
        }

        [DefaultValue((string) null), DataSourceXmlAttribute, Browsable(false)]
        public string GeneratorGetMethodName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.generatorGetMethodName;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.generatorGetMethodName = value;
            }
        }

        [DefaultValue((string) null), Browsable(false), DataSourceXmlAttribute]
        public string GeneratorGetMethodNameForPaging
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.generatorGetMethodNameForPaging;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.generatorGetMethodNameForPaging = value;
            }
        }

        [Browsable(false)]
        public override string GeneratorName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.GeneratorSourceName;
            }
        }

        [DefaultValue((string) null), DataSourceXmlAttribute, Browsable(false)]
        public string GeneratorSourceName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.generatorSourceName;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.generatorSourceName = value;
            }
        }

        [Browsable(false), DefaultValue((string) null), DataSourceXmlAttribute]
        public string GeneratorSourceNameForPaging
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.generatorSourceNameForPaging;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.generatorSourceNameForPaging = value;
            }
        }

        internal bool IsMainSource
        {
            get
            {
                DesignTable owner = this.Owner as DesignTable;
                return ((owner != null) && (owner.MainSource == this));
            }
        }

        [DefaultValue(0x6000), DataSourceXmlAttribute]
        public MemberAttributes Modifier
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.modifier;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.modifier = value;
            }
        }

        [DataSourceXmlAttribute, MergableProperty(false), DefaultValue("")]
        public virtual string Name
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    if (this.CollectionParent != null)
                    {
                        this.CollectionParent.ValidateUniqueName(this, value);
                    }
                    this.name = value;
                }
            }
        }

        [Browsable(false)]
        internal DataSourceComponent Owner
        {
            get
            {
                if ((this.owner == null) && (this.CollectionParent != null))
                {
                    SourceCollection collectionParent = this.CollectionParent as SourceCollection;
                    if (collectionParent != null)
                    {
                        this.owner = collectionParent.CollectionHost;
                    }
                }
                return this.owner;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.owner = value;
            }
        }

        [Browsable(false)]
        public virtual string PublicTypeName
        {
            get
            {
                return "Function";
            }
        }

        [DefaultValue((string) null), Browsable(false), DataSourceXmlAttribute]
        public string UserSourceName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.userSourceName;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.userSourceName = value;
            }
        }

        [DefaultValue(""), DataSourceXmlAttribute]
        public string WebMethodDescription
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.webMethodDescription;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.webMethodDescription = value;
            }
        }
    }
}

