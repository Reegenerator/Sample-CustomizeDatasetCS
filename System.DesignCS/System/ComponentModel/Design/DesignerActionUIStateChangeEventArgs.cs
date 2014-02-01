namespace System.ComponentModel.Design
{
    using System;
    using System.Runtime;

    public class DesignerActionUIStateChangeEventArgs : EventArgs
    {
        private DesignerActionUIStateChangeType changeType;
        private object relatedObject;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerActionUIStateChangeEventArgs(object relatedObject, DesignerActionUIStateChangeType changeType)
        {
            this.relatedObject = relatedObject;
            this.changeType = changeType;
        }

        public DesignerActionUIStateChangeType ChangeType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.changeType;
            }
        }

        public object RelatedObject
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.relatedObject;
            }
        }
    }
}

