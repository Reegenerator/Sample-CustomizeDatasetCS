namespace System.ComponentModel.Design
{
    using System;
    using System.Runtime;

    public class DesignerActionListsChangedEventArgs : EventArgs
    {
        private DesignerActionListCollection actionLists;
        private DesignerActionListsChangedType changeType;
        private object relatedObject;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerActionListsChangedEventArgs(object relatedObject, DesignerActionListsChangedType changeType, DesignerActionListCollection actionLists)
        {
            this.relatedObject = relatedObject;
            this.changeType = changeType;
            this.actionLists = actionLists;
        }

        public DesignerActionListCollection ActionLists
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.actionLists;
            }
        }

        public DesignerActionListsChangedType ChangeType
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

