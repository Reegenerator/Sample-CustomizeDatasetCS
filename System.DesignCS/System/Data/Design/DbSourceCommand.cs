namespace System.Data.Design
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Design;
    using System.Runtime;

    [DataSourceXmlClass("DbCommand"), DefaultProperty("CommandText")]
    internal class DbSourceCommand : DataSourceComponent, ICloneable, INamedObject
    {
        private DbSource _parent;
        private System.Data.Design.CommandOperation commandOperation;
        private string commandText;
        private System.Data.CommandType commandType;
        private bool modifiedByUser;
        private string name;
        private DbSourceParameterCollection parameterCollection;

        public DbSourceCommand()
        {
            this.commandText = string.Empty;
            this.commandType = System.Data.CommandType.Text;
            this.parameterCollection = new DbSourceParameterCollection(this);
        }

        public DbSourceCommand(DbSource parent, System.Data.Design.CommandOperation operation) : this()
        {
            this.SetParent(parent);
            this.CommandOperation = operation;
        }

        public object Clone()
        {
            DbSourceCommand command = new DbSourceCommand {
                commandText = this.commandText,
                commandType = this.commandType,
                commandOperation = this.commandOperation,
                parameterCollection = (DbSourceParameterCollection) this.parameterCollection.Clone()
            };
            command.parameterCollection.CollectionHost = command;
            return command;
        }

        internal void SetParent(DbSource parent)
        {
            this._parent = parent;
        }

        private bool ShouldSerializeParameters()
        {
            return ((this.parameterCollection != null) && (0 < this.parameterCollection.Count));
        }

        public override string ToString()
        {
            if (StringUtil.NotEmptyAfterTrim(this.Name))
            {
                return this.Name;
            }
            return base.ToString();
        }

        internal System.Data.Design.CommandOperation CommandOperation
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.commandOperation;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.commandOperation = value;
            }
        }

        [DataSourceXmlElement, Browsable(false)]
        public string CommandText
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.commandText;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.commandText = value;
            }
        }

        [DataSourceXmlAttribute(ItemType=typeof(System.Data.CommandType)), DefaultValue(1)]
        public System.Data.CommandType CommandType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.commandType;
            }
            set
            {
                if (((value == System.Data.CommandType.TableDirect) && (this._parent != null)) && ((this._parent.Connection != null) && !StringUtil.EqualValue(this._parent.Connection.Provider, "System.Data.OleDb")))
                {
                    throw new Exception(System.Design.SR.GetString("DD_E_TableDirectValidForOleDbOnly"));
                }
                this.commandType = value;
            }
        }

        [DataSourceXmlAttribute(ItemType=typeof(bool)), Browsable(false)]
        public bool ModifiedByUser
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.modifiedByUser;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.modifiedByUser = value;
            }
        }

        [Browsable(false)]
        public string Name
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.name;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.name = value;
            }
        }

        [DataSourceXmlSubItem(ItemType=typeof(DesignParameter))]
        public DbSourceParameterCollection Parameters
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.parameterCollection;
            }
        }

        [Browsable(false)]
        public override object Parent
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._parent;
            }
        }
    }
}

