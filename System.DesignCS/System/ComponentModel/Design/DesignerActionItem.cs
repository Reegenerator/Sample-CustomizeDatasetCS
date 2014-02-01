namespace System.ComponentModel.Design
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Runtime;
    using System.Text.RegularExpressions;

    public abstract class DesignerActionItem
    {
        private bool allowAssociate;
        private string category;
        private string description;
        private string displayName;
        private IDictionary properties;
        private bool showInSourceView;

        internal DesignerActionItem()
        {
            this.showInSourceView = true;
        }

        public DesignerActionItem(string displayName, string category, string description)
        {
            this.showInSourceView = true;
            this.category = category;
            this.description = description;
            this.displayName = (displayName == null) ? null : Regex.Replace(displayName, @"\(\&.\)", "");
        }

        public bool AllowAssociate
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.allowAssociate;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.allowAssociate = value;
            }
        }

        public virtual string Category
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.category;
            }
        }

        public virtual string Description
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.description;
            }
        }

        public virtual string DisplayName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.displayName;
            }
        }

        public IDictionary Properties
        {
            get
            {
                if (this.properties == null)
                {
                    this.properties = new HybridDictionary();
                }
                return this.properties;
            }
        }

        public bool ShowInSourceView
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.showInSourceView;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.showInSourceView = value;
            }
        }
    }
}

