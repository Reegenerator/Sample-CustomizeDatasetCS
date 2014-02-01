namespace System.Web.UI.Design.Directives
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Design;
    using System.Globalization;

    public static class DirectiveRegistry
    {
        private static ReadOnlyCollection<Type> _emptyList = new ReadOnlyCollection<Type>(new List<Type>());
        private static Dictionary<Version, Dictionary<string, IList<Type>>> _versionMap = new Dictionary<Version, Dictionary<string, IList<Type>>>();

        static DirectiveRegistry()
        {
            BuildFrameworkPre40Directives();
            BuildFramework40Directives();
            BuildFramework45Directives();
        }

        private static void AddCommonDirectives(Version ver)
        {
            AddDirective(typeof(Assembly), ver, new string[] { "asax", "ashx", "asix", "asmx", "ascx", "svc", "aspx", "master" });
            AddDirective(typeof(Image), ver, new string[] { "asix" });
            AddDirective(typeof(Implements), ver, new string[] { "ascx", "aspx", "master" });
            AddDirective(typeof(Import), ver, new string[] { "asax", "ascx", "aspx", "master" });
            AddDirective(typeof(MasterType), ver, new string[] { "aspx", "master" });
            AddDirective(typeof(Msgx), ver, new string[] { "msgx" });
            AddDirective(typeof(OutputCache), ver, new string[] { "aspx" });
            AddDirective(typeof(OutputCacheAscx), ver, new string[] { "ascx" });
            AddDirective(typeof(OutputCacheAsix), ver, new string[] { "asix" });
            AddDirective(typeof(PreviousPageType), ver, new string[] { "aspx" });
            AddDirective(typeof(Reference), ver, new string[] { "ascx", "aspx", "master" });
            AddDirective(typeof(Register), ver, new string[] { "ascx", "aspx", "master" });
            AddDirective(typeof(ServiceHost), ver, new string[] { "svc" });
            AddDirective(typeof(WebService), ver, new string[] { "asmx" });
        }

        private static void AddDirective(Type directiveType, Version frameworkVersion, string[] extensions)
        {
            Dictionary<string, IList<Type>> dictionary;
            if (!_versionMap.ContainsKey(frameworkVersion))
            {
                dictionary = new Dictionary<string, IList<Type>>();
                _versionMap[frameworkVersion] = dictionary;
            }
            else
            {
                dictionary = _versionMap[frameworkVersion];
            }
            foreach (string str in extensions)
            {
                IList<Type> list;
                if (!dictionary.ContainsKey(str))
                {
                    list = new List<Type>();
                    dictionary[str] = list;
                }
                else
                {
                    list = dictionary[str];
                }
                list.Add(directiveType);
            }
        }

        private static void BuildFramework40Directives()
        {
            Version ver = new Version(4, 0);
            AddCommonDirectives(ver);
            AddDirective(typeof(Application4_0), ver, new string[] { "asax" });
            AddDirective(typeof(Control4_0), ver, new string[] { "ascx" });
            AddDirective(typeof(Master4_0), ver, new string[] { "master" });
            AddDirective(typeof(Page4_0), ver, new string[] { "aspx" });
            AddDirective(typeof(WebHandler4_0), ver, new string[] { "ashx" });
        }

        private static void BuildFramework45Directives()
        {
            Version ver = new Version(4, 5);
            AddCommonDirectives(ver);
            AddDirective(typeof(Application4_0), ver, new string[] { "asax" });
            AddDirective(typeof(Control4_5), ver, new string[] { "ascx" });
            AddDirective(typeof(Master4_0), ver, new string[] { "master" });
            AddDirective(typeof(Page4_0), ver, new string[] { "aspx" });
            AddDirective(typeof(WebHandler4_0), ver, new string[] { "ashx" });
        }

        private static void BuildFrameworkPre40Directives()
        {
            Version[] versionArray = new Version[] { new Version(2, 0), new Version(3, 0), new Version(3, 5) };
            foreach (Version version in versionArray)
            {
                AddCommonDirectives(version);
                AddDirective(typeof(Application2_0), version, new string[] { "asax" });
                AddDirective(typeof(Control2_0), version, new string[] { "ascx" });
                AddDirective(typeof(Master2_0), version, new string[] { "master" });
                AddDirective(typeof(Page2_0), version, new string[] { "aspx" });
                AddDirective(typeof(WebHandler2_0), version, new string[] { "ashx" });
            }
        }

        public static ReadOnlyCollection<Type> GetDirectives(Version frameworkVersion, string extension)
        {
            if (!_versionMap.ContainsKey(frameworkVersion))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, System.Design.SR.GetString("DirectiveRegistry_UnknownFramework"), new object[] { frameworkVersion }));
            }
            if (!_versionMap[frameworkVersion].ContainsKey(extension))
            {
                return _emptyList;
            }
            return new ReadOnlyCollection<Type>(_versionMap[frameworkVersion][extension]);
        }
    }
}

