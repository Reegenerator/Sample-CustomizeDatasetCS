namespace System.Data.Design
{
    using System;
    using System.Runtime;

    internal sealed class DataSourceGeneratorException : Exception
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal DataSourceGeneratorException(string message) : base(message)
        {
        }
    }
}

