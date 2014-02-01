namespace System.ComponentModel.Design.Serialization
{
    using System;
    using System.CodeDom;
    using System.Runtime;

    public sealed class ExpressionContext
    {
        private CodeExpression _expression;
        private Type _expressionType;
        private object _owner;
        private object _presetValue;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ExpressionContext(CodeExpression expression, Type expressionType, object owner) : this(expression, expressionType, owner, null)
        {
        }

        public ExpressionContext(CodeExpression expression, Type expressionType, object owner, object presetValue)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            if (expressionType == null)
            {
                throw new ArgumentNullException("expressionType");
            }
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            this._expression = expression;
            this._expressionType = expressionType;
            this._owner = owner;
            this._presetValue = presetValue;
        }

        public CodeExpression Expression
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._expression;
            }
        }

        public Type ExpressionType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._expressionType;
            }
        }

        public object Owner
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._owner;
            }
        }

        public object PresetValue
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._presetValue;
            }
        }
    }
}

