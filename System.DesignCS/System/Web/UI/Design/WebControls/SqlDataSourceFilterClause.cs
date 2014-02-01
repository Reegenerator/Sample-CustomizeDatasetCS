﻿namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.ComponentModel.Design.Data;
    using System.Globalization;
    using System.Runtime;
    using System.Web.UI.WebControls;

    internal sealed class SqlDataSourceFilterClause
    {
        private System.ComponentModel.Design.Data.DesignerDataColumn _designerDataColumn;
        private DesignerDataConnection _designerDataConnection;
        private DesignerDataTableBase _designerDataTable;
        private bool _isBinary;
        private string _operatorFormat;
        private System.Web.UI.WebControls.Parameter _parameter;
        private string _value;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public SqlDataSourceFilterClause(DesignerDataConnection designerDataConnection, DesignerDataTableBase designerDataTable, System.ComponentModel.Design.Data.DesignerDataColumn designerDataColumn, string operatorFormat, bool isBinary, string value, System.Web.UI.WebControls.Parameter parameter)
        {
            this._designerDataConnection = designerDataConnection;
            this._designerDataTable = designerDataTable;
            this._designerDataColumn = designerDataColumn;
            this._isBinary = isBinary;
            this._operatorFormat = operatorFormat;
            this._value = value;
            this._parameter = parameter;
        }

        public override string ToString()
        {
            SqlDataSourceColumnData data = new SqlDataSourceColumnData(this._designerDataConnection, this._designerDataColumn);
            if (this._isBinary)
            {
                return string.Format(CultureInfo.InvariantCulture, this._operatorFormat, new object[] { data.EscapedName, this._value });
            }
            return string.Format(CultureInfo.InvariantCulture, this._operatorFormat, new object[] { data.EscapedName });
        }

        public System.ComponentModel.Design.Data.DesignerDataColumn DesignerDataColumn
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._designerDataColumn;
            }
        }

        public bool IsBinary
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._isBinary;
            }
        }

        public string OperatorFormat
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._operatorFormat;
            }
        }

        public System.Web.UI.WebControls.Parameter Parameter
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._parameter;
            }
        }

        public string Value
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._value;
            }
        }
    }
}

