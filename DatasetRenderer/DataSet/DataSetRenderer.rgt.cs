using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;


namespace DatasetRenderer.Dataset
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using System.Linq;
    using System.Data;
    /// <summary>
    /// DataSetRenderer renderer class.
    /// </summary>
    public partial class DataSetRenderer
    {
        public const string GeneratedCodeAttribute = "[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"System.Data.Design.TypedDataSetGenerator\", \"4.0.0.0\")]";
        public const string NonUserAndGeneratedCodeAttribute =
            "[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]\r\n" +
            "        " + GeneratedCodeAttribute;

        /// <summary>
        /// The dataset object as loaded from the dataset file that triggered the code rendering process.
        /// </summary>
        private System.Data.DataSet _dataSet;

        private System.Data.Design.TypedDataSetGenerator.GenerateOption _generateOption;
        /// <summary>
        /// Method that gets called prior to calling <see cref="Render"/>.
        /// Use this method to initialize the properties to be used by the render process.
        /// You can access the project item attached to this generator by using the <see cref="ProjectItem"/> property.
        /// </summary> 
        public override void PreRender()
        {

            base.PreRender();
            var filePath = base.ProjectItem.FullPath;
            this._dataSet = new System.Data.DataSet();
            this._dataSet.ReadXmlSchema(filePath);

            var xdoc = XDocument.Load(filePath);
            //get HierarchicalUpdate property 
            var ns = xdoc.Root.Name.Namespace;
            var mspropNs = xdoc.Root.GetNamespaceOfPrefix("msprop");
            var enableManagerString = xdoc.Root.Element(ns + "element").Attribute(mspropNs + "EnableTableAdapterManager").Value;
            var enableManager = Convert.ToBoolean(enableManagerString);
            _generateOption = enableManager
                ? System.Data.Design.TypedDataSetGenerator.GenerateOption.HierarchicalUpdate
                : System.Data.Design.TypedDataSetGenerator.GenerateOption.None;

        }

        class CommandInfo
        {
            public string Name { get; set; }
            public string Text { get; set; }
        }
        private CommandInfo[] GenCommands(XElement tableAdpX)
        {

            var commands = from cmd in tableAdpX.XPathSelectElements("MainSource/DbSource/*")
                           select new CommandInfo() { Name = cmd.Name.LocalName, Text = cmd.XPathSelectElement("DbCommand/CommandText").Value };

            return null;

            //select new { Name = cmd.Name, 
            //             Command = cmd.XPathSelectElement("DbCommand/CommandText").Value,
            //             Parameters = GenParameters(cmd)
            //            };
        }

        private SqlParameter GenParameters(XElement cmdX)
        {
            //var pars = from par in cmdX.XPathSelectElements("DbCommand/CommandText")
            //            select new SqlParameter( par.Attr("ParameterName") );
            return null;
        }

        private SqlDbType GetSqlDbType(XElement parX)
        {
            SqlDbType res = SqlDbType.Int;
            switch (parX.Attr("DbType"))
            {
                case "Int32":
                    res = SqlDbType.Int;
                    break;
                case "Binary":
                    switch (parX.Attr("ProviderType"))
                    {
                        case "TimeStamp":
                            res = SqlDbType.Timestamp;
                            break;

                    };
                    break;
            }
            return res;
        }

        /// <summary>
        /// Gets the name of the row changed event for a given table.
        /// </summary>
        private string RowChangedName(System.Data.DataTable table)
        {

            return GetExtProp(table, "Generator_RowChangedName", "{0}RowChanged");
        }

        /// <summary>
        /// Gets the name of the row changed event for a given table.
        /// </summary>
        private string RowChangingName(System.Data.DataTable table)
        {
            return GetExtProp(table, "Generator_RowChangingName", "{0}RowChanging");
        }

        /// <summary>
        /// Gets the name of the class derived from a table row (containing all table fields as properties).
        /// </summary>
        private string RowClassName(System.Data.DataTable table)
        {
            return GetExtProp(table, "Generator_RowClassName", "{0}Row");
        }

        /// <summary>
        /// Gets the name of the row deleted event for a given table.
        /// </summary>
        private string RowDeletedName(System.Data.DataTable table)
        {
            return GetExtProp(table, "Generator_RowDeletedName", "{0}RowDeleted");
        }

        /// <summary>
        /// Gets the name of the row deleting event for a given table.
        /// </summary>
        private string RowDeletingName(System.Data.DataTable table)
        {
            return GetExtProp(table, "Generator_RowDeletingName", "{0}RowDeleting");
        }

        /// <summary>
        /// Gets the name of the row change event for a given table.
        /// </summary>
        private string RowChangeEventName(System.Data.DataTable table)
        {
            return GetExtProp(table, "Generator_RowEvArgName", "{0}RowChangeEvent");
        }

        /// <summary>
        /// Gets the name of the row change event handler for a given table.
        /// </summary>
        private string RowChangeEventHandlerName(System.Data.DataTable table)
        {
            return GetExtProp(table, "Generator_RowEvHandlerName", "{0}RowChangeEventHandler");
        }

        /// <summary>
        /// Gets the name of the class derived from a table.
        /// </summary>
        private string TableClassName(System.Data.DataTable table)
        {
            return GetExtProp(table, "Generator_TableClassName", "{0}DataTable");
        }

        /// <summary>
        /// Gets the name of the dataset property returning the instance of a class derived from a given table.
        /// </summary>
        private string TablePropName(System.Data.DataTable table)
        {
            return GetExtProp(table, "Generator_TablePropName", "{0}");
        }

        /// <summary>
        /// Gets the name of the dataset private property containing the instance of a class derived from a given table.
        /// </summary>
        private string TableVarName(System.Data.DataTable table)
        {
            return GetExtProp(table, "Generator_TableVarName", "table{0}");
        }

        /// <summary>
        /// Gets the key of a given table in the Tables collection belonging to the dataset.
        /// </summary>
        private string UserTableName(System.Data.DataTable table)
        {
            if (table.ExtendedProperties.ContainsKey("Generator_UserTableName"))
                return table.ExtendedProperties["Generator_UserTableName"].ToString();
            return table.TableName;
        }

        /// <summary>
        /// Gets the formatted value of a table property.
        /// </summary>
        private string GetExtProp(System.Data.DataTable table, string key, string format)
        {
            if (table.ExtendedProperties.ContainsKey(key))
                return table.ExtendedProperties[key].ToString();

            string propName = null;
            if (table.ExtendedProperties.ContainsKey("Generator_TablePropName"))
                propName = table.ExtendedProperties[key].ToString();
            else
                propName = table.TableName.ToIdentifier();

            return string.Format(format, propName);
        }

        /// <summary>
        /// Gets a dictionary containing all the properties associated with a given table.
        /// The dictionary is keyed on the property name and the values are the values of the properties.
        /// </summary>
        /// <remarks>
        /// The following properties are not added to the returned dictionary:
        ///     - Generator_RowChangedName
        ///     - Generator_RowChangingName
        ///     - Generator_RowDeletedName
        ///     - Generator_RowDeletingName
        /// </remarks>
        private SortedDictionary<string, string> GetExtendedProperties(System.Data.DataTable table)
        {
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>();
            if (table.TableName.ToIdentifier() != UserTableName(table))
            {
                foreach (string key in table.ExtendedProperties.Keys)
                {
                    if (key == "Generator_RowChangedName" ||
                        key == "Generator_RowChangingName" ||
                        key == "Generator_RowDeletedName" ||
                        key == "Generator_RowDeletingName")
                    {
                        continue;
                    }
                    dictionary.Add(key, table.ExtendedProperties[key].ToString());
                }
            }
            return dictionary;
        }

  

        /// <summary>
        /// Renders the full name of an enumeration item
        /// </summary>
        private string RenderEnumItem(object o)
        {
            return o.GetType().FullName + "." + o.ToString();
        }

        /// <summary>
        /// Gets a list containing all the columns belonging to a column collection.
        /// </summary>
        private List<System.Data.DataColumn> ColumnsAsList(System.Data.DataColumnCollection columns)
        {
            List<System.Data.DataColumn> list = new List<System.Data.DataColumn>();
            foreach (System.Data.DataColumn column in columns)
                list.Add(column);
            return list;
        }

        /// <summary>
        /// Renders the columns that belong to a constraint as an array.
        /// </summary>
        private string RenderConstraintColumns(System.Data.DataColumn[] columns)
        {
            return Join(columns, ", ", delegate(System.Data.DataColumn c)
                {
                    return string.Format("this.table{0}.{1}Column", c.Table.TableName.ToIdentifier(), c.ColumnName.ToIdentifier());
                });
        }

        /// <summary>
        /// Renders the columns that belong to an unique constraint as an array.
        /// </summary>
        private string RenderUniqueConstraintColumns(System.Data.DataColumn[] columns)
        {
            return Join(columns, ", " + Environment.NewLine, delegate(System.Data.DataColumn c)
                {
                    return string.Format("this.column{0}", c.ColumnName.ToIdentifier());
                });
        }

        /// <summary>
        /// Renders the parameters of the methods that adds a row to a table.
        /// </summary>
        private string RenderAddRowParameters(System.Data.DataColumnCollection columns)
        {
            string result = Join(ColumnsAsList(columns), string.Empty,
                delegate(System.Data.DataColumn c)
                {
                    if (c.AutoIncrement)
                        return string.Empty;

                    foreach (System.Data.DataRelation parentRelation in c.Table.ParentRelations)
                    {
                        foreach (System.Data.DataColumn column in parentRelation.ChildColumns)
                        {
                            if (column == c)
                            {
                                return string.Format("{0}Row parent{0}RowBy{1}, ",
                                    parentRelation.ParentTable.TableName.ToIdentifier(),
                                    parentRelation.RelationName.ToIdentifier());
                            }
                        }
                    }
                    return string.Format("{0} {1}, ", LangType(c.DataType), c.ColumnName.ToIdentifier());
                });

            if (result.Length >= 2)
                result = result.Substring(0, result.Length - 2);
            if (columns.Count > 16)
            {
                result = Environment.NewLine + result.Replace(", ", "," + Environment.NewLine);
                result = DecorateMultiLine("                        {0}", result);
            }
            return result;
        }

        /// <summary>
        /// Builds the array of fields that is used by the methods that adds a row to a table.
        /// </summary>
        private string RenderAddRowArray(System.Data.DataColumnCollection columns)
        {
            string columnArray =
@"object[] columnValuesArray = new object[] {{
        {0}}};";

            string columnArrayValues = Join(ColumnsAsList(columns), "," + Environment.NewLine,
                delegate(System.Data.DataColumn c)
                {
                    if (c.AutoIncrement)
                        return "null";
                    foreach (System.Data.DataRelation parentRelation in c.Table.ParentRelations)
                    {
                        foreach (System.Data.DataColumn column in parentRelation.ChildColumns)
                        {
                            if (column == c)
                                return "null";
                        }
                    }
                    return c.ColumnName.ToIdentifier();
                });

            string arrayInitialization = string.Empty;
            for (int i = 0; i < columns.Count; i++)
            {
                System.Data.DataColumn c = columns[i];
                if (c.AutoIncrement)
                    continue;
                foreach (System.Data.DataRelation parentRelation in c.Table.ParentRelations)
                {
                    int index = 0;
                    foreach (System.Data.DataColumn column in parentRelation.ChildColumns)
                    {
                        if (column == c)
                        {
                            arrayInitialization = arrayInitialization + string.Format(
@"if ((parent{0}RowBy{1} != null)) {{
    columnValuesArray[{3}] = parent{0}RowBy{1}[{2}];
}}
", parentRelation.ParentTable.TableName.ToIdentifier(),
                                parentRelation.RelationName.ToIdentifier(),
                                parentRelation.ParentColumns[index].Ordinal,
                                i);
                        }
                        index++;
                    }
                }
            }
            return string.Format(columnArray, columnArrayValues) + Environment.NewLine + arrayInitialization;
        }

        /// <summary>
        /// Concatenates the names of the given columns.
        /// </summary>
        private string GetColumnsAsNameList(System.Data.DataColumn[] columns)
        {
            return Join(columns, string.Empty,
                delegate(System.Data.DataColumn c)
                {
                    return c.ColumnName.ToIdentifier();
                });
        }

        /// <summary>
        /// Renders the given columns as parameters declaration.
        /// </summary>
        private string GetColumnsAsParameters(System.Data.DataColumn[] columns)
        {
            return Join(columns, ", ",
                delegate(System.Data.DataColumn c)
                {
                    return string.Format("{0} {1}", LangType(c.DataType), c.ColumnName.ToIdentifier());
                });
        }

        /// <summary>
        /// Renders the given columns as parameters for function call.
        /// </summary>
        private string GetColumnsAsArray(System.Data.DataColumn[] columns)
        {
            return Join(columns, "," + Environment.NewLine,
                delegate(System.Data.DataColumn c)
                {
                    return string.Format("{0}", c.ColumnName.ToIdentifier());
                });
        }


        /// <summary>
        /// Transforms the .NET name of a type into a C# type. E.g. System.String -> string
        /// </summary>
        private static string LangType(Type type)
        {
            return LangType(type.FullName);
        }

        private static CodeDomProvider _codeProvider = CodeDomProvider.CreateProvider("CSharp");

        /// <summary>
        /// Transforms the .NET name of a type into a C# type. E.g. System.String -> string
        /// </summary>
        private static string LangType(string type)
        {
            return _codeProvider.GetTypeOutput(new System.CodeDom.CodeTypeReference(type));
        }

        private string RenderTableAdapters()
        {
            var filePath = base.ProjectItem.FullPath;
            var reader = System.IO.File.OpenRead(filePath);
       
            var res = DatasetGeneratorWrapper.GenTableAdapters(reader, _generateOption, base.ProjectItem.CodeNamespace);
            return res;
        }
    }
}