namespace System.ComponentModel.Design.Data
{
    using System;
    using System.Collections;
    using System.Runtime;

    public abstract class DesignerDataTableBase
    {
        private ICollection _columns;
        private string _name;
        private string _owner;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected DesignerDataTableBase(string name)
        {
            this._name = name;
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected DesignerDataTableBase(string name, string owner)
        {
            this._name = name;
            this._owner = owner;
        }

        protected abstract ICollection CreateColumns();

        public ICollection Columns
        {
            get
            {
                if (this._columns == null)
                {
                    this._columns = this.CreateColumns();
                }
                return this._columns;
            }
        }

        public string Name
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._name;
            }
        }

        public string Owner
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._owner;
            }
        }
    }
}

