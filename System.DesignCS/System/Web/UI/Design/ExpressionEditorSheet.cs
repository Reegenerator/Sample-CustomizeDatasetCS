namespace System.Web.UI.Design
{
    using System;
    using System.ComponentModel;
    using System.Runtime;

    public abstract class ExpressionEditorSheet
    {
        private IServiceProvider _serviceProvider;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        protected ExpressionEditorSheet(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public abstract string GetExpression();

        [Browsable(false)]
        public virtual bool IsValid
        {
            get
            {
                return true;
            }
        }

        [Browsable(false)]
        public IServiceProvider ServiceProvider
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._serviceProvider;
            }
        }
    }
}

