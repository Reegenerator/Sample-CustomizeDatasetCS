namespace System.ComponentModel.Design
{
    using System;
    using System.ComponentModel;
    using System.Design;
    using System.Reflection;
    using System.Runtime;

    public class DesignerActionMethodItem : DesignerActionItem
    {
        private DesignerActionList actionList;
        private bool includeAsDesignerVerb;
        private string memberName;
        private MethodInfo methodInfo;
        private IComponent relatedComponent;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal DesignerActionMethodItem()
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName) : this(actionList, memberName, displayName, null, null, false)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, bool includeAsDesignerVerb) : this(actionList, memberName, displayName, null, null, includeAsDesignerVerb)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, string category) : this(actionList, memberName, displayName, category, null, false)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, string category, bool includeAsDesignerVerb) : this(actionList, memberName, displayName, category, null, includeAsDesignerVerb)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, string category, string description) : this(actionList, memberName, displayName, category, description, false)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, string category, string description, bool includeAsDesignerVerb) : base(displayName, category, description)
        {
            this.actionList = actionList;
            this.memberName = memberName;
            this.includeAsDesignerVerb = includeAsDesignerVerb;
        }

        public virtual void Invoke()
        {
            if (this.methodInfo == null)
            {
                this.methodInfo = this.actionList.GetType().GetMethod(this.memberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            }
            if (this.methodInfo == null)
            {
                throw new InvalidOperationException(System.Design.SR.GetString("DesignerActionPanel_CouldNotFindMethod", new object[] { this.MemberName }));
            }
            this.methodInfo.Invoke(this.actionList, null);
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal void Invoke(object sender, EventArgs args)
        {
            this.Invoke();
        }

        public virtual bool IncludeAsDesignerVerb
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.includeAsDesignerVerb;
            }
        }

        public virtual string MemberName
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

