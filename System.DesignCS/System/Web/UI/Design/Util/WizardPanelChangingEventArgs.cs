namespace System.Web.UI.Design.Util
{
    using System;
    using System.Runtime;

    internal class WizardPanelChangingEventArgs : EventArgs
    {
        private WizardPanel _currentPanel;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public WizardPanelChangingEventArgs(WizardPanel currentPanel)
        {
            this._currentPanel = currentPanel;
        }

        public WizardPanel CurrentPanel
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._currentPanel;
            }
        }
    }
}

