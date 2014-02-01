namespace System.ComponentModel.Design.Serialization
{
    using System;
    using System.CodeDom;
    using System.Runtime;

    public sealed class RootContext
    {
        private CodeExpression expression;
        private object value;

        public RootContext(CodeExpression expression, object value)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.expression = expression;
            this.value = value;
        }

        public CodeExpression Expression
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.expression;
            }
        }

        public object Value
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.value;
            }
        }
    }
}

