namespace System.Web.UI.Design
{
    using System;
    using System.Runtime;

    public class TemplateModeChangedEventArgs : EventArgs
    {
        private TemplateGroup _newTemplateGroup;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public TemplateModeChangedEventArgs(TemplateGroup newTemplateGroup)
        {
            this._newTemplateGroup = newTemplateGroup;
        }

        public TemplateGroup NewTemplateGroup
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._newTemplateGroup;
            }
        }
    }
}

