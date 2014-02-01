namespace System.Data.Design
{
    using System;
    using System.CodeDom;
    using System.Data;
    using System.Runtime;

    internal sealed class RelationHandler
    {
        private TypedDataSourceCodeGenerator codeGenerator;
        private DesignRelationCollection relations;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal RelationHandler(TypedDataSourceCodeGenerator codeGenerator, DesignRelationCollection relations)
        {
            this.codeGenerator = codeGenerator;
            this.relations = relations;
        }

        internal void AddPrivateVars(CodeTypeDeclaration dataSourceClass)
        {
            if (dataSourceClass == null)
            {
                throw new InternalException("DataSource CodeTypeDeclaration should not be null.");
            }
            if (this.relations != null)
            {
                foreach (DesignRelation relation in this.relations)
                {
                    if (relation.DataRelation != null)
                    {
                        string generatorRelationVarName = relation.GeneratorRelationVarName;
                        dataSourceClass.Members.Add(CodeGenHelper.FieldDecl(CodeGenHelper.GlobalType(typeof(DataRelation)), generatorRelationVarName));
                    }
                }
            }
        }

        internal DesignRelationCollection Relations
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.relations;
            }
        }
    }
}

