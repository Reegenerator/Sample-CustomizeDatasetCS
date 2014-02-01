namespace System.Resources.Tools
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Design;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Runtime;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;

    public static class StronglyTypedResourceBuilder
    {
        private static readonly char[] CharsToReplace = new char[] { 
            ' ', '\x00a0', '.', ',', ';', '|', '~', '@', '#', '%', '^', '&', '*', '+', '-', '/', 
            '\\', '<', '>', '?', '[', ']', '(', ')', '{', '}', '"', '\'', ':', '!'
         };
        private const string CultureInfoFieldName = "resourceCulture";
        private const string CultureInfoPropertyName = "Culture";
        private const int DocCommentLengthThreshold = 0x200;
        private const string DocCommentSummaryEnd = "</summary>";
        private const string DocCommentSummaryStart = "<summary>";
        private const char ReplacementChar = '_';
        private const string ResMgrFieldName = "resourceMan";
        private const string ResMgrPropertyName = "ResourceManager";

        private static void AddGeneratedCodeAttributeforMember(CodeTypeMember typeMember)
        {
            CodeAttributeDeclaration declaration = new CodeAttributeDeclaration(new CodeTypeReference(typeof(GeneratedCodeAttribute))) {
                AttributeType = { Options = CodeTypeReferenceOptions.GlobalReference }
            };
            CodeAttributeArgument argument = new CodeAttributeArgument(new CodePrimitiveExpression(typeof(StronglyTypedResourceBuilder).FullName));
            CodeAttributeArgument argument2 = new CodeAttributeArgument(new CodePrimitiveExpression("4.0.0.0"));
            declaration.Arguments.Add(argument);
            declaration.Arguments.Add(argument2);
            typeMember.CustomAttributes.Add(declaration);
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static CodeCompileUnit Create(IDictionary resourceList, string baseName, string generatedCodeNamespace, CodeDomProvider codeProvider, bool internalClass, out string[] unmatchable)
        {
            return Create(resourceList, baseName, generatedCodeNamespace, null, codeProvider, internalClass, out unmatchable);
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static CodeCompileUnit Create(string resxFile, string baseName, string generatedCodeNamespace, CodeDomProvider codeProvider, bool internalClass, out string[] unmatchable)
        {
            return Create(resxFile, baseName, generatedCodeNamespace, null, codeProvider, internalClass, out unmatchable);
        }

        public static CodeCompileUnit Create(IDictionary resourceList, string baseName, string generatedCodeNamespace, string resourcesNamespace, CodeDomProvider codeProvider, bool internalClass, out string[] unmatchable)
        {
            if (resourceList == null)
            {
                throw new ArgumentNullException("resourceList");
            }
            Dictionary<string, ResourceData> dictionary = new Dictionary<string, ResourceData>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DictionaryEntry entry in resourceList)
            {
                ResourceData data;
                ResXDataNode node = entry.Value as ResXDataNode;
                if (node != null)
                {
                    string key = (string) entry.Key;
                    if (key != node.Name)
                    {
                        throw new ArgumentException(System.Design.SR.GetString("MismatchedResourceName", new object[] { key, node.Name }));
                    }
                    Type type = Type.GetType(node.GetValueTypeName((AssemblyName[]) null));
                    string valueAsString = node.GetValue((AssemblyName[]) null).ToString();
                    data = new ResourceData(type, valueAsString);
                }
                else
                {
                    Type type2 = (entry.Value == null) ? typeof(object) : entry.Value.GetType();
                    data = new ResourceData(type2, (entry.Value == null) ? null : entry.Value.ToString());
                }
                dictionary.Add((string) entry.Key, data);
            }
            return InternalCreate(dictionary, baseName, generatedCodeNamespace, resourcesNamespace, codeProvider, internalClass, out unmatchable);
        }

        public static CodeCompileUnit Create(string resxFile, string baseName, string generatedCodeNamespace, string resourcesNamespace, CodeDomProvider codeProvider, bool internalClass, out string[] unmatchable)
        {
            if (resxFile == null)
            {
                throw new ArgumentNullException("resxFile");
            }
            Dictionary<string, ResourceData> resourceList = new Dictionary<string, ResourceData>(StringComparer.InvariantCultureIgnoreCase);
            using (ResXResourceReader reader = new ResXResourceReader(resxFile))
            {
                reader.UseResXDataNodes = true;
                foreach (DictionaryEntry entry in reader)
                {
                    ResXDataNode node = (ResXDataNode) entry.Value;
                    Type type = Type.GetType(node.GetValueTypeName((AssemblyName[]) null));
                    string valueAsString = node.GetValue((AssemblyName[]) null).ToString();
                    ResourceData data = new ResourceData(type, valueAsString);
                    resourceList.Add((string) entry.Key, data);
                }
            }
            return InternalCreate(resourceList, baseName, generatedCodeNamespace, resourcesNamespace, codeProvider, internalClass, out unmatchable);
        }

        private static bool DefineResourceFetchingProperty(string propertyName, string resourceName, ResourceData data, CodeTypeDeclaration srClass, bool internalClass, bool useStatic)
        {
            CodeMethodReturnStatement statement;
            CodeMemberProperty property = new CodeMemberProperty {
                Name = propertyName,
                HasGet = true,
                HasSet = false
            };
            Type baseType = data.Type;
            if (baseType == null)
            {
                return false;
            }
            if (baseType == typeof(MemoryStream))
            {
                baseType = typeof(UnmanagedMemoryStream);
            }
            while (!baseType.IsPublic)
            {
                baseType = baseType.BaseType;
            }
            CodeTypeReference targetType = new CodeTypeReference(baseType);
            property.Type = targetType;
            if (internalClass)
            {
                property.Attributes = MemberAttributes.Assembly;
            }
            else
            {
                property.Attributes = MemberAttributes.Public;
            }
            if (useStatic)
            {
                property.Attributes |= MemberAttributes.Static;
            }
            CodePropertyReferenceExpression targetObject = new CodePropertyReferenceExpression(null, "ResourceManager");
            CodeFieldReferenceExpression expression2 = new CodeFieldReferenceExpression(useStatic ? null : new CodeThisReferenceExpression(), "resourceCulture");
            bool flag = baseType == typeof(string);
            bool flag2 = (baseType == typeof(UnmanagedMemoryStream)) || (baseType == typeof(MemoryStream));
            string methodName = string.Empty;
            string text = string.Empty;
            string b = TruncateAndFormatCommentStringForOutput(data.ValueAsString);
            string a = string.Empty;
            if (!flag)
            {
                a = TruncateAndFormatCommentStringForOutput(baseType.ToString());
            }
            if (flag)
            {
                methodName = "GetString";
            }
            else if (flag2)
            {
                methodName = "GetStream";
            }
            else
            {
                methodName = "GetObject";
            }
            if (flag)
            {
                text = System.Design.SR.GetString("StringPropertyComment", new object[] { b });
            }
            else if ((b == null) || string.Equals(a, b))
            {
                text = System.Design.SR.GetString("NonStringPropertyComment", new object[] { a });
            }
            else
            {
                text = System.Design.SR.GetString("NonStringPropertyDetailedComment", new object[] { a, b });
            }
            property.Comments.Add(new CodeCommentStatement("<summary>", true));
            property.Comments.Add(new CodeCommentStatement(text, true));
            property.Comments.Add(new CodeCommentStatement("</summary>", true));
            CodeExpression expression = new CodeMethodInvokeExpression(targetObject, methodName, new CodeExpression[] { new CodePrimitiveExpression(resourceName), expression2 });
            if (flag || flag2)
            {
                statement = new CodeMethodReturnStatement(expression);
            }
            else
            {
                CodeVariableDeclarationStatement statement2 = new CodeVariableDeclarationStatement(typeof(object), "obj", expression);
                property.GetStatements.Add(statement2);
                statement = new CodeMethodReturnStatement(new CodeCastExpression(targetType, new CodeVariableReferenceExpression("obj")));
            }
            property.GetStatements.Add(statement);
            srClass.Members.Add(property);
            return true;
        }

        private static void EmitBasicClassMembers(CodeTypeDeclaration srClass, string nameSpace, string baseName, string resourcesNamespace, bool internalClass, bool useStatic, bool supportsTryCatch, bool useTypeInfo)
        {
            string str;
            CodePropertyReferenceExpression expression4;
            if (resourcesNamespace != null)
            {
                if (resourcesNamespace.Length > 0)
                {
                    str = resourcesNamespace + '.' + baseName;
                }
                else
                {
                    str = baseName;
                }
            }
            else if ((nameSpace != null) && (nameSpace.Length > 0))
            {
                str = nameSpace + '.' + baseName;
            }
            else
            {
                str = baseName;
            }
            CodeCommentStatement statement = new CodeCommentStatement(System.Design.SR.GetString("ClassComments1"));
            srClass.Comments.Add(statement);
            statement = new CodeCommentStatement(System.Design.SR.GetString("ClassComments2"));
            srClass.Comments.Add(statement);
            statement = new CodeCommentStatement(System.Design.SR.GetString("ClassComments3"));
            srClass.Comments.Add(statement);
            statement = new CodeCommentStatement(System.Design.SR.GetString("ClassComments4"));
            srClass.Comments.Add(statement);
            CodeAttributeDeclaration declaration = new CodeAttributeDeclaration(new CodeTypeReference(typeof(SuppressMessageAttribute))) {
                AttributeType = { Options = CodeTypeReferenceOptions.GlobalReference }
            };
            declaration.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression("Microsoft.Performance")));
            declaration.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression("CA1811:AvoidUncalledPrivateCode")));
            CodeConstructor constructor = new CodeConstructor();
            constructor.CustomAttributes.Add(declaration);
            if (useStatic || internalClass)
            {
                constructor.Attributes = MemberAttributes.FamilyAndAssembly;
            }
            else
            {
                constructor.Attributes = MemberAttributes.Public;
            }
            srClass.Members.Add(constructor);
            CodeTypeReference type = new CodeTypeReference(typeof(ResourceManager), CodeTypeReferenceOptions.GlobalReference);
            CodeMemberField field = new CodeMemberField(type, "resourceMan") {
                Attributes = MemberAttributes.Private
            };
            if (useStatic)
            {
                field.Attributes |= MemberAttributes.Static;
            }
            srClass.Members.Add(field);
            CodeTypeReference reference2 = new CodeTypeReference(typeof(CultureInfo), CodeTypeReferenceOptions.GlobalReference);
            field = new CodeMemberField(reference2, "resourceCulture") {
                Attributes = MemberAttributes.Private
            };
            if (useStatic)
            {
                field.Attributes |= MemberAttributes.Static;
            }
            srClass.Members.Add(field);
            CodeMemberProperty property = new CodeMemberProperty();
            srClass.Members.Add(property);
            property.Name = "ResourceManager";
            property.HasGet = true;
            property.HasSet = false;
            property.Type = type;
            if (internalClass)
            {
                property.Attributes = MemberAttributes.Assembly;
            }
            else
            {
                property.Attributes = MemberAttributes.Public;
            }
            if (useStatic)
            {
                property.Attributes |= MemberAttributes.Static;
            }
            CodeTypeReference reference3 = new CodeTypeReference(typeof(EditorBrowsableState)) {
                Options = CodeTypeReferenceOptions.GlobalReference
            };
            CodeAttributeArgument argument = new CodeAttributeArgument(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(reference3), "Advanced"));
            CodeAttributeDeclaration declaration2 = new CodeAttributeDeclaration("System.ComponentModel.EditorBrowsableAttribute", new CodeAttributeArgument[] { argument }) {
                AttributeType = { Options = CodeTypeReferenceOptions.GlobalReference }
            };
            property.CustomAttributes.Add(declaration2);
            CodeMemberProperty property2 = new CodeMemberProperty();
            srClass.Members.Add(property2);
            property2.Name = "Culture";
            property2.HasGet = true;
            property2.HasSet = true;
            property2.Type = reference2;
            if (internalClass)
            {
                property2.Attributes = MemberAttributes.Assembly;
            }
            else
            {
                property2.Attributes = MemberAttributes.Public;
            }
            if (useStatic)
            {
                property2.Attributes |= MemberAttributes.Static;
            }
            property2.CustomAttributes.Add(declaration2);
            CodeFieldReferenceExpression left = new CodeFieldReferenceExpression(null, "resourceMan");
            CodeMethodReferenceExpression method = new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(object)), "ReferenceEquals");
            CodeMethodInvokeExpression condition = new CodeMethodInvokeExpression(method, new CodeExpression[] { left, new CodePrimitiveExpression(null) });
            if (useTypeInfo)
            {
                CodeMethodInvokeExpression targetObject = new CodeMethodInvokeExpression(new CodeTypeOfExpression(new CodeTypeReference(srClass.Name)), "GetTypeInfo", new CodeExpression[0]);
                expression4 = new CodePropertyReferenceExpression(targetObject, "Assembly");
            }
            else
            {
                expression4 = new CodePropertyReferenceExpression(new CodeTypeOfExpression(new CodeTypeReference(srClass.Name)), "Assembly");
            }
            CodeObjectCreateExpression initExpression = new CodeObjectCreateExpression(type, new CodeExpression[] { new CodePrimitiveExpression(str), expression4 });
            CodeStatement[] trueStatements = new CodeStatement[] { new CodeVariableDeclarationStatement(type, "temp", initExpression), new CodeAssignStatement(left, new CodeVariableReferenceExpression("temp")) };
            property.GetStatements.Add(new CodeConditionStatement(condition, trueStatements));
            property.GetStatements.Add(new CodeMethodReturnStatement(left));
            property.Comments.Add(new CodeCommentStatement("<summary>", true));
            property.Comments.Add(new CodeCommentStatement(System.Design.SR.GetString("ResMgrPropertyComment"), true));
            property.Comments.Add(new CodeCommentStatement("</summary>", true));
            CodeFieldReferenceExpression expression = new CodeFieldReferenceExpression(null, "resourceCulture");
            property2.GetStatements.Add(new CodeMethodReturnStatement(expression));
            CodePropertySetValueReferenceExpression right = new CodePropertySetValueReferenceExpression();
            property2.SetStatements.Add(new CodeAssignStatement(expression, right));
            property2.Comments.Add(new CodeCommentStatement("<summary>", true));
            property2.Comments.Add(new CodeCommentStatement(System.Design.SR.GetString("CulturePropertyComment1"), true));
            property2.Comments.Add(new CodeCommentStatement(System.Design.SR.GetString("CulturePropertyComment2"), true));
            property2.Comments.Add(new CodeCommentStatement("</summary>", true));
        }

        private static CodeCompileUnit InternalCreate(Dictionary<string, ResourceData> resourceList, string baseName, string generatedCodeNamespace, string resourcesNamespace, CodeDomProvider codeProvider, bool internalClass, out string[] unmatchable)
        {
            Hashtable hashtable;
            if (baseName == null)
            {
                throw new ArgumentNullException("baseName");
            }
            if (codeProvider == null)
            {
                throw new ArgumentNullException("codeProvider");
            }
            ArrayList errors = new ArrayList(0);
            SortedList list2 = VerifyResourceNames(resourceList, codeProvider, errors, out hashtable);
            string str = baseName;
            if (!codeProvider.IsValidIdentifier(str))
            {
                string str2 = VerifyResourceName(str, codeProvider);
                if (str2 != null)
                {
                    str = str2;
                }
            }
            if (!codeProvider.IsValidIdentifier(str))
            {
                throw new ArgumentException(System.Design.SR.GetString("InvalidIdentifier", new object[] { str }));
            }
            if (!string.IsNullOrEmpty(generatedCodeNamespace) && !codeProvider.IsValidIdentifier(generatedCodeNamespace))
            {
                string str3 = VerifyResourceName(generatedCodeNamespace, codeProvider, true);
                if (str3 != null)
                {
                    generatedCodeNamespace = str3;
                }
            }
            CodeCompileUnit e = new CodeCompileUnit();
            e.ReferencedAssemblies.Add("System.dll");
            e.UserData.Add("AllowLateBound", false);
            e.UserData.Add("RequireVariableDeclaration", true);
            CodeNamespace namespace2 = new CodeNamespace(generatedCodeNamespace);
            namespace2.Imports.Add(new CodeNamespaceImport("System"));
            e.Namespaces.Add(namespace2);
            CodeTypeDeclaration declaration = new CodeTypeDeclaration(str);
            namespace2.Types.Add(declaration);
            AddGeneratedCodeAttributeforMember(declaration);
            TypeAttributes attributes = internalClass ? TypeAttributes.AnsiClass : TypeAttributes.Public;
            declaration.TypeAttributes = attributes;
            declaration.Comments.Add(new CodeCommentStatement("<summary>", true));
            declaration.Comments.Add(new CodeCommentStatement(System.Design.SR.GetString("ClassDocComment"), true));
            declaration.Comments.Add(new CodeCommentStatement("</summary>", true));
            CodeTypeReference attributeType = new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute)) {
                Options = CodeTypeReferenceOptions.GlobalReference
            };
            declaration.CustomAttributes.Add(new CodeAttributeDeclaration(attributeType));
            CodeTypeReference reference2 = new CodeTypeReference(typeof(CompilerGeneratedAttribute)) {
                Options = CodeTypeReferenceOptions.GlobalReference
            };
            declaration.CustomAttributes.Add(new CodeAttributeDeclaration(reference2));
            bool useStatic = internalClass || codeProvider.Supports(GeneratorSupport.PublicStaticMembers);
            bool supportsTryCatch = codeProvider.Supports(GeneratorSupport.TryCatchStatements);
            ITargetAwareCodeDomProvider provider = codeProvider as ITargetAwareCodeDomProvider;
            bool useTypeInfo = (provider != null) && !provider.SupportsProperty(typeof(Type), "Assembly", false);
            if (useTypeInfo)
            {
                namespace2.Imports.Add(new CodeNamespaceImport("System.Reflection"));
            }
            EmitBasicClassMembers(declaration, generatedCodeNamespace, baseName, resourcesNamespace, internalClass, useStatic, supportsTryCatch, useTypeInfo);
            foreach (DictionaryEntry entry in list2)
            {
                string key = (string) entry.Key;
                string resourceName = (string) hashtable[key];
                if (resourceName == null)
                {
                    resourceName = key;
                }
                if (!DefineResourceFetchingProperty(key, resourceName, (ResourceData) entry.Value, declaration, internalClass, useStatic))
                {
                    errors.Add(entry.Key);
                }
            }
            unmatchable = (string[]) errors.ToArray(typeof(string));
            CodeGenerator.ValidateIdentifiers(e);
            return e;
        }

        private static string TruncateAndFormatCommentStringForOutput(string commentString)
        {
            if (commentString != null)
            {
                if (commentString.Length > 0x200)
                {
                    commentString = System.Design.SR.GetString("StringPropertyTruncatedComment", new object[] { commentString.Substring(0, 0x200) });
                }
                commentString = SecurityElement.Escape(commentString);
            }
            return commentString;
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static string VerifyResourceName(string key, CodeDomProvider provider)
        {
            return VerifyResourceName(key, provider, false);
        }

        private static string VerifyResourceName(string key, CodeDomProvider provider, bool isNameSpace)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            foreach (char ch in CharsToReplace)
            {
                if (!isNameSpace || ((ch != '.') && (ch != ':')))
                {
                    key = key.Replace(ch, '_');
                }
            }
            if (provider.IsValidIdentifier(key))
            {
                return key;
            }
            key = provider.CreateValidIdentifier(key);
            if (provider.IsValidIdentifier(key))
            {
                return key;
            }
            key = "_" + key;
            if (provider.IsValidIdentifier(key))
            {
                return key;
            }
            return null;
        }

        private static SortedList VerifyResourceNames(Dictionary<string, ResourceData> resourceList, CodeDomProvider codeProvider, ArrayList errors, out Hashtable reverseFixupTable)
        {
            reverseFixupTable = new Hashtable(0, StringComparer.InvariantCultureIgnoreCase);
            SortedList list = new SortedList(StringComparer.InvariantCultureIgnoreCase, resourceList.Count);
            foreach (KeyValuePair<string, ResourceData> pair in resourceList)
            {
                string key = pair.Key;
                if ((string.Equals(key, "ResourceManager") || string.Equals(key, "Culture")) || (typeof(void) == pair.Value.Type))
                {
                    errors.Add(key);
                }
                else
                {
                    if (((key.Length <= 0) || (key[0] != '$')) && (((key.Length <= 1) || (key[0] != '>')) || (key[1] != '>')))
                    {
                        if (!codeProvider.IsValidIdentifier(key))
                        {
                            string str2 = VerifyResourceName(key, codeProvider, false);
                            if (str2 == null)
                            {
                                errors.Add(key);
                                goto Label_0185;
                            }
                            string item = (string) reverseFixupTable[str2];
                            if (item != null)
                            {
                                if (!errors.Contains(item))
                                {
                                    errors.Add(item);
                                }
                                if (list.Contains(str2))
                                {
                                    list.Remove(str2);
                                }
                                errors.Add(key);
                                goto Label_0185;
                            }
                            reverseFixupTable[str2] = key;
                            key = str2;
                        }
                        ResourceData data = pair.Value;
                        if (!list.Contains(key))
                        {
                            list.Add(key, data);
                        }
                        else
                        {
                            string str4 = (string) reverseFixupTable[key];
                            if (str4 != null)
                            {
                                if (!errors.Contains(str4))
                                {
                                    errors.Add(str4);
                                }
                                reverseFixupTable.Remove(key);
                            }
                            errors.Add(pair.Key);
                            list.Remove(key);
                        }
                    }
                Label_0185:;
                }
            }
            return list;
        }

        internal sealed class ResourceData
        {
            private System.Type _type;
            private string _valueAsString;

            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            internal ResourceData(System.Type type, string valueAsString)
            {
                this._type = type;
                this._valueAsString = valueAsString;
            }

            internal System.Type Type
            {
                [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
                get
                {
                    return this._type;
                }
            }

            internal string ValueAsString
            {
                [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
                get
                {
                    return this._valueAsString;
                }
            }
        }
    }
}

