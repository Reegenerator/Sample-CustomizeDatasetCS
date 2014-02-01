namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.ComponentModel.Design.Data;
    using System.Runtime;

    internal sealed class SqlDataSourceOrderClause
    {
        private System.ComponentModel.Design.Data.DesignerDataColumn _designerDataColumn;
        private DesignerDataConnection _designerDataConnection;
        private DesignerDataTableBase _designerDataTable;
        private bool _isDescending;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public SqlDataSourceOrderClause(DesignerDataConnection designerDataConnection, DesignerDataTableBase designerDataTable, System.ComponentModel.Design.Data.DesignerDataColumn designerDataColumn, bool isDescending)
        {
            this._designerDataConnection = designerDataConnection;
            this._designerDataTable = designerDataTable;
            this._designerDataColumn = designerDataColumn;
            this._isDescending = isDescending;
        }

        public override string ToString()
        {
            SqlDataSourceColumnData data = new SqlDataSourceColumnData(this._designerDataConnection, this._designerDataColumn);
            if (this._isDescending)
            {
                return (data.EscapedName + " DESC");
            }
            return data.EscapedName;
        }

        public System.ComponentModel.Design.Data.DesignerDataColumn DesignerDataColumn
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._designerDataColumn;
            }
        }

        public bool IsDescending
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._isDescending;
            }
        }
    }
}

