using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Xml;

namespace DatasetRenderer
{
    static class ReflectionExtension
    {
        const BindingFlags InternalBinding = BindingFlags.Instance | BindingFlags.NonPublic;
        const BindingFlags PublicInstanceBinding = BindingFlags.Instance | BindingFlags.Public;

        static public object Invoke(this Type type, object o, BindingFlags bf, string methodName, params object[] pars)
        {
          
            var parSignatures = (from p in pars select p.GetType()).ToArray();
            var methodInfo = type.GetMethod(methodName, bf,null, parSignatures,null);
            return methodInfo.Invoke(o, pars);
        }

        static public object InvokeInternal(this object o, string methodName, params object[] pars)
        {
            return o.GetType().Invoke(o, InternalBinding, methodName, pars);
        }
        static public object Invoke(this object o, string methodName, params object[] pars)
        {
            return o.GetType().Invoke(o, PublicInstanceBinding, methodName, pars);
        }
        static public object GetInternalPropertyValue(this object o, string propName)
        {
            return o.GetPropertyValue(propName, InternalBinding);
        }
        static public object GetPropertyValue(this object o, string propName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            return o.GetType().GetProperty(propName, bindingFlags).GetValue(o,null);
        }


        static public void SetInternalPropertyValue(this object o, string propName, object value)
        {
            o.SetPropertyValue( propName, value, InternalBinding);
        }
        static public void SetPropertyValue(this object o, string propName, object value, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            o.GetType().GetProperty(propName, bindingFlags).SetValue(o, value, null);
        }
    }
    class DatasetGeneratorWrapper
    {
        static private Assembly assm;
        static DatasetGeneratorWrapper()
        {
                assm = Assembly.Load("System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
        }
        static public void EnsureCustomDbProviders()
        {
            //TypeDataSetGenerator uses a special translation for Microsoft.SqlServerCe.Client.4.0
            var factories=  System.Data.Common.DbProviderFactories.GetFactoryClasses();
            var sqlceFactoryRow = factories.Select(string.Format("InvariantName = '{0}'", "System.Data.SqlServerCe.4.0")).First();
            var sqlceFactory = System.Data.Common.DbProviderFactories.GetFactory(sqlceFactoryRow);
            var customDbProviders = new Hashtable();
            customDbProviders.Add("Microsoft.SqlServerCe.Client.4.0", sqlceFactory);
            var providerMgr = assm.GetType("System.Data.Design.ProviderManager");
            providerMgr.GetField("CustomDBProviders", BindingFlags.Static| BindingFlags.NonPublic).SetValue(null, customDbProviders);
            
        }
        static public string GenTableAdapters(System.IO.Stream xmlStream, TypedDataSetGenerator.GenerateOption genOption, string customToolNamespace)
        {
            try
            {


     

                EnsureCustomDbProviders();
                var designSource = Activator.CreateInstance(assm.GetType("System.Data.Design.DesignDataSource"));
                designSource.Invoke("ReadXmlSchema",  xmlStream, string.Empty );
                var dataSourceGeneratorType = assm.GetType("System.Data.Design.TypedDataSourceCodeGenerator");
                //get the specific constructor
                var constructor = dataSourceGeneratorType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                    null, CallingConventions.Any, new Type[0], new ParameterModifier[0]);
                var dataSourceGenerator = constructor.Invoke(null);
                
                var codeProvider = new Microsoft.CSharp.CSharpCodeProvider();
                dataSourceGenerator.SetInternalPropertyValue("CodeProvider", codeProvider);
        
                var codeCompileUnit = new CodeCompileUnit();
                var codeNamespace = new CodeNamespace(customToolNamespace);
                codeCompileUnit.Namespaces.Add(codeNamespace);

                dataSourceGenerator.InvokeInternal("GenerateDataSource",  designSource, codeCompileUnit, codeNamespace, customToolNamespace, genOption );
                var writer = new StringWriter();
                var adapterNameSpace = codeCompileUnit.Namespaces[1];
                codeProvider.GenerateCodeFromNamespace(adapterNameSpace, writer, new CodeGeneratorOptions());
                //codeProvider.GenerateCodeFromCompileUnit(codeCompileUnit, writer, options);
                var res = writer.ToString();
                return (string)res;
            }
            catch (Exception e)
            {


                return e.ToString();
            }

        }
    }
}
