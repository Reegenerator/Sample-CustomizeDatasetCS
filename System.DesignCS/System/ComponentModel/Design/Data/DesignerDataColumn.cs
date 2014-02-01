namespace System.ComponentModel.Design.Data
{
    using System;
    using System.Data;
    using System.Runtime;

    public sealed class DesignerDataColumn
    {
        private DbType _dataType;
        private object _defaultValue;
        private bool _identity;
        private int _length;
        private string _name;
        private bool _nullable;
        private int _precision;
        private bool _primaryKey;
        private int _scale;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerDataColumn(string name, DbType dataType) : this(name, dataType, null, false, false, false, -1, -1, -1)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerDataColumn(string name, DbType dataType, object defaultValue) : this(name, dataType, defaultValue, false, false, false, -1, -1, -1)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerDataColumn(string name, DbType dataType, object defaultValue, bool identity, bool nullable, bool primaryKey, int precision, int scale, int length)
        {
            this._dataType = dataType;
            this._defaultValue = defaultValue;
            this._identity = identity;
            this._length = length;
            this._name = name;
            this._nullable = nullable;
            this._precision = precision;
            this._primaryKey = primaryKey;
            this._scale = scale;
        }

        public DbType DataType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._dataType;
            }
        }

        public object DefaultValue
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._defaultValue;
            }
        }

        public bool Identity
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._identity;
            }
        }

        public int Length
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._length;
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

        public bool Nullable
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._nullable;
            }
        }

        public int Precision
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._precision;
            }
        }

        public bool PrimaryKey
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._primaryKey;
            }
        }

        public int Scale
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._scale;
            }
        }
    }
}

