namespace System.ComponentModel.Design
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(false), Guid("665f0ba5-ce72-4e87-9ba0-3c461de74d0b")]
    public interface IDesignTimeAssemblyLoader
    {
        string GetTargetAssemblyPath(AssemblyName runtimeOrTargetAssemblyName, string suggestedAssemblyPath, FrameworkName targetFramework);
        Assembly LoadRuntimeAssembly(AssemblyName targetAssemblyName);
    }
}

