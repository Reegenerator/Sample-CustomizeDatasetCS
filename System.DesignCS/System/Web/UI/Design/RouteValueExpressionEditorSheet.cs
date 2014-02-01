namespace System.Web.UI.Design
{
    using System;
    using System.ComponentModel;
    using System.Design;
    using System.Runtime;

    public class RouteValueExpressionEditorSheet : ExpressionEditorSheet
    {
        private string _routeValue;

        public RouteValueExpressionEditorSheet(string expression, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                this.RouteValue = expression;
            }
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public override string GetExpression()
        {
            return this.RouteValue;
        }

        public override bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(this.RouteValue);
            }
        }

        [System.Design.SRDescription("RouteValueExpressionEditorSheet_RouteValue"), DefaultValue("")]
        public string RouteValue
        {
            get
            {
                if (this._routeValue == null)
                {
                    return string.Empty;
                }
                return this._routeValue;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._routeValue = value;
            }
        }
    }
}

