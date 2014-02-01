namespace System.Web.UI.Design
{
    using System;
    using System.Runtime;

    public class ViewRendering
    {
        private string _content;
        private DesignerRegionCollection _regions;
        private bool _visible;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ViewRendering(string content, DesignerRegionCollection regions) : this(content, regions, true)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ViewRendering(string content, DesignerRegionCollection regions, bool visible)
        {
            this._content = content;
            this._regions = regions;
            this._visible = visible;
        }

        public string Content
        {
            get
            {
                if (this._content == null)
                {
                    return string.Empty;
                }
                return this._content;
            }
        }

        public DesignerRegionCollection Regions
        {
            get
            {
                if (this._regions == null)
                {
                    this._regions = new DesignerRegionCollection();
                }
                return this._regions;
            }
        }

        public bool Visible
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._visible;
            }
        }
    }
}

