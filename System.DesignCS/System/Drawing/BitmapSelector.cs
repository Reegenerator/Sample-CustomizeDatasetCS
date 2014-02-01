namespace System.Drawing
{
    using System;
    using System.Configuration;
   // using System.Drawing.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Runtime;

    internal static class BitmapSelector
    {
        private static string _suffix;

        internal static string AppendSuffix(string filePath)
        {
            try
            {
                return Path.ChangeExtension(filePath, Suffix + Path.GetExtension(filePath));
            }
            catch (ArgumentException)
            {
                return filePath;
            }
        }

        public static Bitmap CreateBitmap(Type type, string originalName)
        {
            return new Bitmap(GetResourceStream(type, originalName));
        }

        public static Icon CreateIcon(Type type, string originalName)
        {
            return new Icon(GetResourceStream(type, originalName));
        }

        private static bool DoesAssemblyHaveCustomAttribute(Assembly assembly, string typeName)
        {
            return DoesAssemblyHaveCustomAttribute(assembly, assembly.GetType(typeName));
        }

        private static bool DoesAssemblyHaveCustomAttribute(Assembly assembly, Type attrType)
        {
            return ((attrType != null) && (assembly.GetCustomAttributes(attrType, false).Length > 0));
        }

        public static string GetFileName(string originalPath)
        {
            if (Suffix == string.Empty)
            {
                return originalPath;
            }
            string path = AppendSuffix(originalPath);
            if (!File.Exists(path))
            {
                return originalPath;
            }
            return path;
        }

        public static Stream GetResourceStream(Type type, string originalName)
        {
            return GetResourceStream(type.Module.Assembly, type, originalName);
        }

        public static Stream GetResourceStream(Assembly assembly, Type type, string originalName)
        {
            if (Suffix != string.Empty)
            {
                try
                {
                    if (SameAssemblyOptIn(assembly))
                    {
                        string str = AppendSuffix(originalName);
                        Stream stream = GetResourceStreamHelper(assembly, type, str);
                        if (stream != null)
                        {
                            return stream;
                        }
                    }
                }
                catch
                {
                }
                try
                {
                    if (SatelliteAssemblyOptIn(assembly))
                    {
                        AssemblyName assemblyRef = assembly.GetName();
                        assemblyRef.Name = assemblyRef.Name + Suffix;
                        assemblyRef.ProcessorArchitecture = ProcessorArchitecture.None;
                        Assembly assembly2 = Assembly.Load(assemblyRef);
                        if (assembly2 != null)
                        {
                            Stream stream2 = GetResourceStreamHelper(assembly2, type, originalName);
                            if (stream2 != null)
                            {
                                return stream2;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            return assembly.GetManifestResourceStream(type, originalName);
        }

        private static Stream GetResourceStreamHelper(Assembly assembly, Type type, string name)
        {
            Stream manifestResourceStream = null;
            try
            {
                manifestResourceStream = assembly.GetManifestResourceStream(type, name);
            }
            catch (FileNotFoundException)
            {
            }
            return manifestResourceStream;
        }

        internal static bool SameAssemblyOptIn(Assembly assembly)
        {
            return (DoesAssemblyHaveCustomAttribute(assembly, typeof(BitmapSuffixInSameAssemblyAttribute)) || DoesAssemblyHaveCustomAttribute(assembly, "System.Drawing.BitmapSuffixInSameAssemblyAttribute"));
        }

        internal static bool SatelliteAssemblyOptIn(Assembly assembly)
        {
            return (DoesAssemblyHaveCustomAttribute(assembly, typeof(System.Drawing.BitmapSuffixInSatelliteAssemblyAttribute)) || DoesAssemblyHaveCustomAttribute(assembly, "System.Drawing.BitmapSuffixInSatelliteAssemblyAttribute"));
        }

        internal static string Suffix
        {
            get
            {
                if (_suffix == null)
                {
                    _suffix = string.Empty;
                    SystemDrawingSection section = ConfigurationManager.GetSection("system.drawing") as SystemDrawingSection;
                    if (section != null)
                    {
                        string bitmapSuffix = section.BitmapSuffix;
                        if ((bitmapSuffix != null) && (bitmapSuffix != null))
                        {
                            _suffix = bitmapSuffix;
                        }
                    }
                }
                return _suffix;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                _suffix = value;
            }
        }
    }
}

