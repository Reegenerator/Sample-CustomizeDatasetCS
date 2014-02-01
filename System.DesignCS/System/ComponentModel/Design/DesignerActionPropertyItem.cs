namespace System.ComponentModel.Design
{
    using System;
    using System.ComponentModel;
    using System.Runtime;

    public sealed class DesignerActionPropertyItem : DesignerActionItem
    {
        private string memberName;
        private IComponent relatedComponent;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerActionPropertyItem(string memberName, string displayName) : this(memberName, displayName, null, null)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerActionPropertyItem(string memberName, string displayName, string category) : this(memberName, displayName, category, null)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerActionPropertyItem(string memberName, string displayName, string category, string description) : base(displayName, category, description)
        {
            this.memberName = memberName;
        }

        public string MemberName
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.memberName;
            }
        }

        public IComponent RelatedComponent
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.relatedComponent;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.relatedComponent = value;
            }
        }
    }
}

