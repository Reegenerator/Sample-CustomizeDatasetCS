namespace System.ComponentModel.Design.Data
{
    using System;
    using System.Collections;
    using System.Runtime;

    public sealed class DesignerDataRelationship
    {
        private ICollection _childColumns;
        private DesignerDataTable _childTable;
        private string _name;
        private ICollection _parentColumns;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerDataRelationship(string name, ICollection parentColumns, DesignerDataTable childTable, ICollection childColumns)
        {
            this._childColumns = childColumns;
            this._childTable = childTable;
            this._name = name;
            this._parentColumns = parentColumns;
        }

        public ICollection ChildColumns
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._childColumns;
            }
        }

        public DesignerDataTable ChildTable
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._childTable;
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

        public ICollection ParentColumns
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._parentColumns;
            }
        }
    }
}

