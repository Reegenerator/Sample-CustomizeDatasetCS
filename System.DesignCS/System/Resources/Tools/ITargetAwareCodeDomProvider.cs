namespace System.Resources.Tools
{
    using System;

    public interface ITargetAwareCodeDomProvider
    {
        bool SupportsProperty(Type type, string propertyName, bool isWritable);
    }
}

