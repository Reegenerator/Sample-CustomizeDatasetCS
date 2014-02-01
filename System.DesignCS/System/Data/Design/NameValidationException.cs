namespace System.Data.Design
{
    using System;
    using System.Runtime;

    [Serializable]
    internal sealed class NameValidationException : ApplicationException
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public NameValidationException(string message) : base(message)
        {
        }
    }
}

