namespace System.ComponentModel.Design.Data
{
    using System;
    using System.Collections;
    using System.Runtime;

    public abstract class DesignerDataTable : DesignerDataTableBase
    {
        private ICollection _relationships;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected DesignerDataTable(string name) : base(name)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected DesignerDataTable(string name, string owner) : base(name, owner)
        {
        }

        protected abstract ICollection CreateRelationships();

        public ICollection Relationships
        {
            get
            {
                if (this._relationships == null)
                {
                    this._relationships = this.CreateRelationships();
                }
                return this._relationships;
            }
        }
    }
}

