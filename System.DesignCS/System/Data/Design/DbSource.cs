﻿namespace System.Data.Design
{
    using System;
    using System.CodeDom;
    using System.ComponentModel;
    using System.Runtime;
    using System.Xml;

    [DataSourceXmlClass("DbSource")]
    internal class DbSource : Source, IDataSourceXmlSpecialOwner
    {
        private IDesignConnection connection;
        private string connectionRef;
        private System.Data.Design.DbObjectType dbObjectType;
        private DbSourceCommand deleteCommand;
        private GenerateMethodTypes generateMethods = GenerateMethodTypes.Both;
        private bool generatePagingMethods;
        private bool generateShortCommands = true;
        private MemberAttributes getMethodModifier = MemberAttributes.Public;
        private string getMethodName;
        private DbSourceCommand insertCommand;
        internal const string INSTANCE_NAME_FOR_FILLMETHOD = "FillBy";
        internal const string INSTANCE_NAME_FOR_FILLMETHOD_MAIN = "Fill";
        internal const string INSTANCE_NAME_FOR_FUNCTION = "Query";
        internal const string INSTANCE_NAME_FOR_GETMETHOD = "GetDataBy";
        internal const string INSTANCE_NAME_FOR_GETMETHOD_MAIN = "GetData";
        private TypeEnum parameterType;
        private const string PROPERTY_COMMANDTEXT = "CommandText";
        private System.Data.Design.QueryType queryType;
        private Type scalarCallRetval = typeof(object);
        private DbSourceCommand selectCommand;
        internal const string TYPE_NAME_FOR_FUNCTION = "Query";
        internal const string TYPE_NAME_FOR_QUERY = "Query";
        private DbSourceCommand updateCommand;
        private bool useOptimisticConcurrency = true;
        private string userGetMethodName;

        public override object Clone()
        {
            DbSource parent = new DbSource();
            if (this.connection != null)
            {
                parent.connection = (DesignConnection) this.connection.Clone();
            }
            if (this.selectCommand != null)
            {
                parent.selectCommand = (DbSourceCommand) this.selectCommand.Clone();
                parent.selectCommand.SetParent(parent);
            }
            if (this.insertCommand != null)
            {
                parent.insertCommand = (DbSourceCommand) this.insertCommand.Clone();
                parent.insertCommand.SetParent(parent);
            }
            if (this.updateCommand != null)
            {
                parent.updateCommand = (DbSourceCommand) this.updateCommand.Clone();
                parent.updateCommand.SetParent(parent);
            }
            if (this.deleteCommand != null)
            {
                parent.deleteCommand = (DbSourceCommand) this.deleteCommand.Clone();
                parent.deleteCommand.SetParent(parent);
            }
            parent.Name = this.Name;
            parent.Modifier = base.Modifier;
            parent.scalarCallRetval = this.scalarCallRetval;
            parent.generateMethods = this.generateMethods;
            parent.queryType = this.queryType;
            parent.getMethodModifier = this.getMethodModifier;
            parent.getMethodName = this.getMethodName;
            parent.generatePagingMethods = this.generatePagingMethods;
            return parent;
        }

        internal DbSourceCommand GetActiveCommand()
        {
            switch (this.CommandOperation)
            {
                case System.Data.Design.CommandOperation.Select:
                    return this.SelectCommand;

                case System.Data.Design.CommandOperation.Insert:
                    return this.InsertCommand;

                case System.Data.Design.CommandOperation.Update:
                    return this.UpdateCommand;

                case System.Data.Design.CommandOperation.Delete:
                    return this.DeleteCommand;
            }
            return null;
        }

        internal override bool NameExist(string nameToCheck)
        {
            if (!StringUtil.EqualValue(this.FillMethodName, nameToCheck, true))
            {
                return StringUtil.EqualValue(this.GetMethodName, nameToCheck, true);
            }
            return true;
        }

        void IDataSourceXmlSpecialOwner.ReadSpecialItem(string propertyName, XmlNode xmlNode, DataSourceXmlSerializer serializer)
        {
            if (propertyName.Equals("ScalarCallRetval"))
            {
                this.scalarCallRetval = typeof(object);
                if (StringUtil.NotEmptyAfterTrim(xmlNode.InnerText))
                {
                    this.scalarCallRetval = Type.GetType(xmlNode.InnerText, false);
                }
            }
        }

        void IDataSourceXmlSpecialOwner.WriteSpecialItem(string propertyName, XmlWriter writer, DataSourceXmlSerializer serializer)
        {
            if (propertyName.Equals("ScalarCallRetval"))
            {
                writer.WriteString(this.scalarCallRetval.AssemblyQualifiedName);
            }
        }

        protected internal override DataSourceCollectionBase CollectionParent
        {
            get
            {
                if (base.CollectionParent != null)
                {
                    return base.CollectionParent;
                }
                if (((base.owner != null) && (base.owner is DesignTable)) && (((DesignTable) base.owner).MainSource == this))
                {
                    return ((DesignTable) base.owner).Sources;
                }
                return null;
            }
        }

        public System.Data.Design.CommandOperation CommandOperation
        {
            get
            {
                if (this.SelectCommand != null)
                {
                    return System.Data.Design.CommandOperation.Select;
                }
                if (this.InsertCommand != null)
                {
                    return System.Data.Design.CommandOperation.Insert;
                }
                if (this.UpdateCommand != null)
                {
                    return System.Data.Design.CommandOperation.Update;
                }
                if (this.DeleteCommand != null)
                {
                    return System.Data.Design.CommandOperation.Delete;
                }
                return System.Data.Design.CommandOperation.Unknown;
            }
        }

        [RefreshProperties(RefreshProperties.All), DefaultValue((string) null)]
        public IDesignConnection Connection
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.connection;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.connection = value;
            }
        }

        [DataSourceXmlAttribute, Browsable(false)]
        public string ConnectionRef
        {
            get
            {
                if (this.connection != null)
                {
                    return this.connection.Name;
                }
                return this.connectionRef;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.connectionRef = value;
            }
        }

        [DataSourceXmlAttribute]
        public System.Data.Design.DbObjectType DbObjectType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.dbObjectType;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.dbObjectType = value;
            }
        }

        [DataSourceXmlSubItem(Name="DeleteCommand", ItemType=typeof(DbSourceCommand)), Browsable(false)]
        public DbSourceCommand DeleteCommand
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.deleteCommand;
            }
            set
            {
                if (this.deleteCommand != null)
                {
                    this.deleteCommand.SetParent(null);
                }
                this.deleteCommand = value;
                if (this.deleteCommand != null)
                {
                    this.deleteCommand.SetParent(this);
                    this.deleteCommand.CommandOperation = System.Data.Design.CommandOperation.Delete;
                }
            }
        }

        [DataSourceXmlAttribute, DefaultValue(0x6000)]
        public MemberAttributes FillMethodModifier
        {
            get
            {
                return base.Modifier;
            }
            set
            {
                base.Modifier = value;
            }
        }

        [DefaultValue("Fill"), DataSourceXmlAttribute]
        public string FillMethodName
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        [DefaultValue(3), RefreshProperties(RefreshProperties.All), DataSourceXmlAttribute]
        public GenerateMethodTypes GenerateMethods
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.generateMethods;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.generateMethods = value;
            }
        }

        [DefaultValue(true), DataSourceXmlAttribute]
        public bool GeneratePagingMethods
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.generatePagingMethods;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.generatePagingMethods = value;
            }
        }

        [DataSourceXmlAttribute]
        public bool GenerateShortCommands
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.generateShortCommands;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.generateShortCommands = value;
            }
        }

        [DefaultValue(0x6000), DataSourceXmlAttribute]
        public MemberAttributes GetMethodModifier
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.getMethodModifier;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.getMethodModifier = value;
            }
        }

        [DefaultValue("GetData"), DataSourceXmlAttribute]
        public string GetMethodName
        {
            get
            {
                if (StringUtil.EmptyOrSpace(this.getMethodName) && (this.CollectionParent != null))
                {
                    if (base.IsMainSource)
                    {
                        this.GetMethodName = "GetData";
                    }
                    else
                    {
                        this.GetMethodName = "GetDataBy";
                    }
                }
                return this.getMethodName;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.getMethodName = value;
            }
        }

        [Browsable(false), DataSourceXmlSubItem(Name="InsertCommand", ItemType=typeof(DbSourceCommand))]
        public DbSourceCommand InsertCommand
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.insertCommand;
            }
            set
            {
                if (this.insertCommand != null)
                {
                    this.insertCommand.SetParent(null);
                }
                this.insertCommand = value;
                if (this.insertCommand != null)
                {
                    this.insertCommand.SetParent(this);
                    this.insertCommand.CommandOperation = System.Data.Design.CommandOperation.Insert;
                }
            }
        }

        [DataSourceXmlAttribute, DefaultValue(0)]
        public TypeEnum MethodsParameterType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.parameterType;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.parameterType = value;
            }
        }

        [Browsable(false)]
        public override string Name
        {
            get
            {
                if (StringUtil.Empty(base.Name) && (this.generateMethods == GenerateMethodTypes.Get))
                {
                    return this.GetMethodName;
                }
                return base.Name;
            }
            set
            {
                if (base.name != value)
                {
                    base.name = value;
                    SourceCollection collectionParent = this.CollectionParent as SourceCollection;
                    if (collectionParent != null)
                    {
                        collectionParent.ValidateUniqueDbSourceName(this, value, true);
                    }
                }
            }
        }

        [Browsable(false)]
        public override object Parent
        {
            get
            {
                if (base.Parent != null)
                {
                    return base.Parent;
                }
                return base.Owner;
            }
        }

        [Browsable(false)]
        public override string PublicTypeName
        {
            get
            {
                if (base.Owner is DesignTable)
                {
                    return "Query";
                }
                return "Query";
            }
        }

        [DataSourceXmlAttribute, Browsable(false)]
        public System.Data.Design.QueryType QueryType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.queryType;
            }
            set
            {
                this.queryType = value;
                if (this.queryType != System.Data.Design.QueryType.Rowset)
                {
                    this.GenerateMethods = GenerateMethodTypes.Fill;
                }
            }
        }

        [DataSourceXmlAttribute(SpecialWay=true), Browsable(false)]
        public Type ScalarCallRetval
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.scalarCallRetval;
            }
        }

        [Browsable(false), DataSourceXmlSubItem(Name="SelectCommand", ItemType=typeof(DbSourceCommand))]
        public DbSourceCommand SelectCommand
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.selectCommand;
            }
            set
            {
                if (this.selectCommand != null)
                {
                    this.selectCommand.SetParent(null);
                }
                this.selectCommand = value;
                if (this.selectCommand != null)
                {
                    this.selectCommand.SetParent(this);
                    this.selectCommand.CommandOperation = System.Data.Design.CommandOperation.Select;
                }
            }
        }

        [Browsable(false), DataSourceXmlSubItem(Name="UpdateCommand", ItemType=typeof(DbSourceCommand))]
        public DbSourceCommand UpdateCommand
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.updateCommand;
            }
            set
            {
                if (this.updateCommand != null)
                {
                    this.updateCommand.SetParent(null);
                }
                this.updateCommand = value;
                if (this.updateCommand != null)
                {
                    this.updateCommand.SetParent(this);
                    this.updateCommand.CommandOperation = System.Data.Design.CommandOperation.Update;
                }
            }
        }

        [DataSourceXmlAttribute]
        public bool UseOptimisticConcurrency
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.useOptimisticConcurrency;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.useOptimisticConcurrency = value;
            }
        }

        [DataSourceXmlAttribute]
        public string UserGetMethodName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.userGetMethodName;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.userGetMethodName = value;
            }
        }
    }
}

