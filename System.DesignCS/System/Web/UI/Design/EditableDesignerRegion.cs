namespace System.Web.UI.Design
{
    using System;
    using System.Runtime;
    using System.Web.UI;

    public class EditableDesignerRegion : DesignerRegion
    {
        private bool _serverControlsOnly;
        private bool _supportsDataBinding;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public EditableDesignerRegion(ControlDesigner owner, string name) : this(owner, name, false)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public EditableDesignerRegion(ControlDesigner owner, string name, bool serverControlsOnly) : base(owner, name)
        {
            this._serverControlsOnly = serverControlsOnly;
        }

        public virtual ViewRendering GetChildViewRendering(Control control)
        {
            return ControlDesigner.GetViewRendering(control);
        }

        public virtual string Content
        {
            get
            {
                return base.Designer.GetEditableDesignerRegionContent(this);
            }
            set
            {
                base.Designer.SetEditableDesignerRegionContent(this, value);
            }
        }

        public bool ServerControlsOnly
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._serverControlsOnly;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._serverControlsOnly = value;
            }
        }

        public virtual bool SupportsDataBinding
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._supportsDataBinding;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._supportsDataBinding = value;
            }
        }
    }
}

