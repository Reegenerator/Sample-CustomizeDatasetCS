namespace System.ComponentModel.Design.Data
{
    using System;
    using System.Data;
    using System.Runtime;

    public sealed class DesignerDataParameter
    {
        private DbType _dataType;
        private ParameterDirection _direction;
        private string _name;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerDataParameter(string name, DbType dataType, ParameterDirection direction)
        {
            this._dataType = dataType;
            this._direction = direction;
            this._name = name;
        }

        public DbType DataType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._dataType;
            }
        }

        public ParameterDirection Direction
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._direction;
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
    }
}

