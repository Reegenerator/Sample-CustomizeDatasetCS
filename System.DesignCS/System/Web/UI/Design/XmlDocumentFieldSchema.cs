﻿namespace System.Web.UI.Design
{
    using System;
    using System.Runtime;

    internal sealed class XmlDocumentFieldSchema : IDataSourceFieldSchema
    {
        private string _name;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public XmlDocumentFieldSchema(string name)
        {
            this._name = name;
        }

        public Type DataType
        {
            get
            {
                return typeof(string);
            }
        }

        public bool Identity
        {
            get
            {
                return false;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool IsUnique
        {
            get
            {
                return false;
            }
        }

        public int Length
        {
            get
            {
                return -1;
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
            get
            {
                return true;
            }
        }

        public int Precision
        {
            get
            {
                return -1;
            }
        }

        public bool PrimaryKey
        {
            get
            {
                return false;
            }
        }

        public int Scale
        {
            get
            {
                return -1;
            }
        }
    }
}

