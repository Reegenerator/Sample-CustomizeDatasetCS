﻿namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.Collections;
    using System.Runtime;
    using System.Web.UI.WebControls;

    internal sealed class SqlDataSourceQuery
    {
        private string _command;
        private SqlDataSourceCommandType _commandType;
        private ICollection _parameters;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public SqlDataSourceQuery(string command, SqlDataSourceCommandType commandType, ICollection parameters)
        {
            this._command = command;
            this._commandType = commandType;
            this._parameters = parameters;
        }

        public string Command
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._command;
            }
        }

        public SqlDataSourceCommandType CommandType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._commandType;
            }
        }

        public ICollection Parameters
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._parameters;
            }
        }
    }
}

