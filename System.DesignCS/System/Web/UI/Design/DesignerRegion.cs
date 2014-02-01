namespace System.Web.UI.Design
{
    using System;
    using System.Drawing;
    using System.Runtime;

    public class DesignerRegion : DesignerObject
    {
        private string _description;
        private string _displayName;
        private bool _ensureSize;
        private bool _highlight;
        private bool _selectable;
        private bool _selected;
        private object _userData;
        public static readonly string DesignerRegionAttributeName = "_designerRegion";

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerRegion(ControlDesigner designer, string name) : this(designer, name, false)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerRegion(ControlDesigner designer, string name, bool selectable) : base(designer, name)
        {
            this._selectable = selectable;
        }

        public Rectangle GetBounds()
        {
            return base.Designer.View.GetBounds(this);
        }

        public virtual string Description
        {
            get
            {
                if (this._description == null)
                {
                    return string.Empty;
                }
                return this._description;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._description = value;
            }
        }

        public virtual string DisplayName
        {
            get
            {
                if (this._displayName == null)
                {
                    return string.Empty;
                }
                return this._displayName;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._displayName = value;
            }
        }

        public bool EnsureSize
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._ensureSize;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._ensureSize = value;
            }
        }

        public virtual bool Highlight
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._highlight;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._highlight = value;
            }
        }

        public virtual bool Selectable
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._selectable;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._selectable = value;
            }
        }

        public virtual bool Selected
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._selected;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._selected = value;
            }
        }

        public object UserData
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._userData;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._userData = value;
            }
        }
    }
}

