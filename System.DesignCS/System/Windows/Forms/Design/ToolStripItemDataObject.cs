namespace System.Windows.Forms.Design
{
    using System;
    using System.Collections;
    using System.Runtime;
    using System.Windows.Forms;

    internal class ToolStripItemDataObject : DataObject
    {
        private ArrayList dragComponents;
        private ToolStrip owner;
        private ToolStripItem primarySelection;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal ToolStripItemDataObject(ArrayList dragComponents, ToolStripItem primarySelection, ToolStrip owner)
        {
            this.dragComponents = dragComponents;
            this.owner = owner;
            this.primarySelection = primarySelection;
        }

        internal ArrayList DragComponents
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.dragComponents;
            }
        }

        internal ToolStrip Owner
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.owner;
            }
        }

        internal ToolStripItem PrimarySelection
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.primarySelection;
            }
        }
    }
}

