namespace System.Windows.Forms.Design
{
    using System;
    using System.Collections.Generic;
    using System.Runtime;

    internal class ContextMenuStripGroup
    {
        private List<ToolStripItem> items;
        private string name;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ContextMenuStripGroup(string name)
        {
            this.name = name;
        }

        public List<ToolStripItem> Items
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new List<ToolStripItem>();
                }
                return this.items;
            }
        }
    }
}

