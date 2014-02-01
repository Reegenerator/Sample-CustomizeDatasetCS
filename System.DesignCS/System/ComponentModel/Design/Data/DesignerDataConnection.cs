namespace System.ComponentModel.Design.Data
{
    using System;
    using System.Runtime;

    public sealed class DesignerDataConnection
    {
        private string _connectionString;
        private bool _isConfigured;
        private string _name;
        private string _providerName;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerDataConnection(string name, string providerName, string connectionString) : this(name, providerName, connectionString, false)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerDataConnection(string name, string providerName, string connectionString, bool isConfigured)
        {
            this._name = name;
            this._providerName = providerName;
            this._connectionString = connectionString;
            this._isConfigured = isConfigured;
        }

        public string ConnectionString
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._connectionString;
            }
        }

        public bool IsConfigured
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._isConfigured;
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

        public string ProviderName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._providerName;
            }
        }
    }
}

