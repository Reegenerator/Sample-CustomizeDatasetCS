namespace System.Data.Design
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Runtime;

    internal sealed class DataComponentNameHandler
    {
        private static readonly string adapterPropertyName = "Adapter";
        private static readonly string adapterVariableName = "_adapter";
        private static readonly string clearBeforeFillPropertyName = "ClearBeforeFill";
        private static readonly string clearBeforeFillVariableName = "_clearBeforeFill";
        private static readonly string defaultConnectionPropertyName = "Connection";
        private static readonly string defaultConnectionVariableName = "_connection";
        private static readonly string deleteMethodName = "Delete";
        private bool globalSources;
        private static readonly string initAdapter = "InitAdapter";
        private static readonly string initCmdCollection = "InitCommandCollection";
        private static readonly string initConnection = "InitConnection";
        private static readonly string initMethodName = "InitClass";
        private static readonly string insertMethodName = "Insert";
        private bool languageCaseInsensitive;
        private static readonly string pagingMethodSuffix = "Page";
        private static readonly string selectCmdCollectionPropertyName = "CommandCollection";
        private static readonly string selectCmdCollectionVariableName = "_commandCollection";
        private static readonly string transactionPropertyName = "Transaction";
        private static readonly string transactionVariableName = "_transaction";
        private static readonly string updateMethodName = "Update";
        private MemberNameValidator validator;

        private void AddReservedNames()
        {
            this.validator.GetNewMemberName(initMethodName);
            this.validator.GetNewMemberName(deleteMethodName);
            this.validator.GetNewMemberName(insertMethodName);
            this.validator.GetNewMemberName(updateMethodName);
            this.validator.GetNewMemberName(adapterVariableName);
            this.validator.GetNewMemberName(adapterPropertyName);
            this.validator.GetNewMemberName(initAdapter);
            this.validator.GetNewMemberName(selectCmdCollectionVariableName);
            this.validator.GetNewMemberName(selectCmdCollectionPropertyName);
            this.validator.GetNewMemberName(initCmdCollection);
            this.validator.GetNewMemberName(defaultConnectionVariableName);
            this.validator.GetNewMemberName(defaultConnectionPropertyName);
            this.validator.GetNewMemberName(transactionVariableName);
            this.validator.GetNewMemberName(transactionPropertyName);
            this.validator.GetNewMemberName(initConnection);
            this.validator.GetNewMemberName(clearBeforeFillVariableName);
            this.validator.GetNewMemberName(clearBeforeFillPropertyName);
            this.validator.GetNewMemberName("TableAdapterManager");
            this.validator.GetNewMemberName("UpdateAll");
        }

        internal void GenerateMemberNames(DesignTable designTable, CodeDomProvider codeProvider, bool languageCaseInsensitive, ArrayList problemList)
        {
            this.languageCaseInsensitive = languageCaseInsensitive;
            this.validator = new MemberNameValidator(null, codeProvider, this.languageCaseInsensitive);
            this.validator.UseSuffix = true;
            this.AddReservedNames();
            this.ProcessMemberNames(designTable);
        }

        internal void ProcessClassName(DesignTable table)
        {
            if (!StringUtil.EqualValue(table.DataAccessorName, table.UserDataComponentName, this.languageCaseInsensitive) || StringUtil.Empty(table.GeneratorDataComponentClassName))
            {
                table.GeneratorDataComponentClassName = this.validator.GenerateIdName(table.DataAccessorName);
            }
            else
            {
                table.GeneratorDataComponentClassName = this.validator.GenerateIdName(table.GeneratorDataComponentClassName);
            }
        }

        private void ProcessMemberNames(DesignTable designTable)
        {
            this.ProcessClassName(designTable);
            if (!this.GlobalSources && (designTable.MainSource != null))
            {
                this.ProcessSourceName((DbSource) designTable.MainSource);
            }
            if (designTable.Sources != null)
            {
                foreach (Source source in designTable.Sources)
                {
                    this.ProcessSourceName((DbSource) source);
                }
            }
        }

        internal void ProcessSourceName(DbSource source)
        {
            bool flag = !StringUtil.EqualValue(source.Name, source.UserSourceName, this.languageCaseInsensitive);
            bool flag2 = !StringUtil.EqualValue(source.GetMethodName, source.UserGetMethodName, this.languageCaseInsensitive);
            if ((source.GenerateMethods == GenerateMethodTypes.Fill) || (source.GenerateMethods == GenerateMethodTypes.Both))
            {
                if (flag || StringUtil.Empty(source.GeneratorSourceName))
                {
                    source.GeneratorSourceName = this.validator.GenerateIdName(source.Name);
                }
                else
                {
                    source.GeneratorSourceName = this.validator.GenerateIdName(source.GeneratorSourceName);
                }
            }
            if ((source.QueryType == QueryType.Rowset) && ((source.GenerateMethods == GenerateMethodTypes.Get) || (source.GenerateMethods == GenerateMethodTypes.Both)))
            {
                if (flag2 || StringUtil.Empty(source.GeneratorGetMethodName))
                {
                    source.GeneratorGetMethodName = this.validator.GenerateIdName(source.GetMethodName);
                }
                else
                {
                    source.GeneratorGetMethodName = this.validator.GenerateIdName(source.GeneratorGetMethodName);
                }
            }
            if ((source.QueryType == QueryType.Rowset) && source.GeneratePagingMethods)
            {
                if ((source.GenerateMethods == GenerateMethodTypes.Fill) || (source.GenerateMethods == GenerateMethodTypes.Both))
                {
                    if (flag || StringUtil.Empty(source.GeneratorSourceNameForPaging))
                    {
                        source.GeneratorSourceNameForPaging = this.validator.GenerateIdName(source.Name + pagingMethodSuffix);
                    }
                    else
                    {
                        source.GeneratorSourceNameForPaging = this.validator.GenerateIdName(source.GeneratorSourceNameForPaging);
                    }
                }
                if ((source.GenerateMethods == GenerateMethodTypes.Get) || (source.GenerateMethods == GenerateMethodTypes.Both))
                {
                    if (flag2 || StringUtil.Empty(source.GeneratorGetMethodNameForPaging))
                    {
                        source.GeneratorGetMethodNameForPaging = this.validator.GenerateIdName(source.GetMethodName + pagingMethodSuffix);
                    }
                    else
                    {
                        source.GeneratorGetMethodNameForPaging = this.validator.GenerateIdName(source.GeneratorGetMethodNameForPaging);
                    }
                }
            }
        }

        internal static string AdapterPropertyName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return adapterPropertyName;
            }
        }

        internal static string AdapterVariableName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return adapterVariableName;
            }
        }

        internal static string ClearBeforeFillPropertyName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return clearBeforeFillPropertyName;
            }
        }

        internal static string ClearBeforeFillVariableName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return clearBeforeFillVariableName;
            }
        }

        internal static string DefaultConnectionPropertyName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return defaultConnectionPropertyName;
            }
        }

        internal static string DefaultConnectionVariableName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return defaultConnectionVariableName;
            }
        }

        internal static string DeleteMethodName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return deleteMethodName;
            }
        }

        internal bool GlobalSources
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.globalSources;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.globalSources = value;
            }
        }

        internal static string InitAdapter
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return initAdapter;
            }
        }

        internal static string InitCmdCollection
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return initCmdCollection;
            }
        }

        internal static string InitConnection
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return initConnection;
            }
        }

        internal static string InsertMethodName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return insertMethodName;
            }
        }

        internal static string PagingMethodSuffix
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return pagingMethodSuffix;
            }
        }

        internal static string SelectCmdCollectionPropertyName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return selectCmdCollectionPropertyName;
            }
        }

        internal static string SelectCmdCollectionVariableName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return selectCmdCollectionVariableName;
            }
        }

        internal static string TransactionPropertyName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return transactionPropertyName;
            }
        }

        internal static string TransactionVariableName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return transactionVariableName;
            }
        }

        internal static string UpdateMethodName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return updateMethodName;
            }
        }
    }
}

