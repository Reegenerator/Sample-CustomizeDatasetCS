namespace System.Windows.Forms.Design.Behavior
{
    using System;
    using System.Collections;
    using System.Runtime;

    public class BehaviorDragDropEventArgs : EventArgs
    {
        private ICollection dragComponents;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public BehaviorDragDropEventArgs(ICollection dragComponents)
        {
            this.dragComponents = dragComponents;
        }

        public ICollection DragComponents
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.dragComponents;
            }
        }
    }
}

