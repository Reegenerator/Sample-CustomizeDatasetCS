namespace System.Data.Design
{
    using System;
    using System.Runtime;

    internal class ConnectionString
    {
        private string connectionString;
        private string providerName;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ConnectionString(string providerName, string connectionString)
        {
            this.connectionString = connectionString;
            this.providerName = providerName;
        }

        public string ToFullString()
        {
            return this.connectionString.ToString();
        }
    }
}

