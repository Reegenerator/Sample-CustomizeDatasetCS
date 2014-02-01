namespace System.Windows.Forms.Design
{
    using System;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Runtime;
    using System.Windows.Forms;

    internal class ItemTypeToolStripMenuItem : ToolStripMenuItem
    {
        private System.Drawing.Image _image;
        private System.Type _itemType;
        private bool convertTo;
        private static ToolboxItem invalidToolboxItem = new ToolboxItem();
        private static string systemWindowsFormsNamespace = typeof(ToolStripItem).Namespace;
        private ToolboxItem tbxItem = invalidToolboxItem;

        public ItemTypeToolStripMenuItem(System.Type t)
        {
            this._itemType = t;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.tbxItem = null;
            }
            base.Dispose(disposing);
        }

        public bool ConvertTo
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.convertTo;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.convertTo = value;
            }
        }

        public override System.Drawing.Image Image
        {
            get
            {
                if (this._image == null)
                {
                    this._image = ToolStripDesignerUtils.GetToolboxBitmap(this.ItemType);
                }
                return this._image;
            }
            set
            {
            }
        }

        public System.Type ItemType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._itemType;
            }
        }

        public override string Text
        {
            get
            {
                return ToolStripDesignerUtils.GetToolboxDescription(this.ItemType);
            }
            set
            {
            }
        }
    }
}

