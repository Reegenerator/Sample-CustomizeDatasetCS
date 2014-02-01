namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.Runtime;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;

    public class WizardStepTemplatedEditableRegion : TemplatedEditableDesignerRegion, IWizardStepEditableRegion
    {
        private WizardStepBase _wizardStep;

        public WizardStepTemplatedEditableRegion(TemplateDefinition templateDefinition, WizardStepBase wizardStep) : base(templateDefinition)
        {
            this._wizardStep = wizardStep;
            base.EnsureSize = true;
        }

        public WizardStepBase Step
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._wizardStep;
            }
        }
    }
}

