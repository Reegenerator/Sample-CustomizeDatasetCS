namespace System.Data.Design
{
    using System;
    using System.Runtime;

    [Serializable]
    internal sealed class DataSourceSerializationException : ApplicationException
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DataSourceSerializationException(string message) : base(message)
        {
        }
    }
}

