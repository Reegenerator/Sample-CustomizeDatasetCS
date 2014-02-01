namespace System.ComponentModel.Design.Data
{
    using System;
    using System.Collections;
    using System.Runtime;

    public abstract class DesignerDataStoredProcedure
    {
        private string _name;
        private string _owner;
        private ICollection _parameters;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected DesignerDataStoredProcedure(string name)
        {
            this._name = name;
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected DesignerDataStoredProcedure(string name, string owner)
        {
            this._name = name;
            this._owner = owner;
        }

        protected abstract ICollection CreateParameters();

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

        public ICollection Parameters
        {
            get
            {
                if (this._parameters == null)
                {
                    this._parameters = this.CreateParameters();
                }
                return this._parameters;
            }
        }
    }
}

