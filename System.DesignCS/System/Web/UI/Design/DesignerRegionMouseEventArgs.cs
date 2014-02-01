namespace System.Web.UI.Design
{
    using System;
    using System.Drawing;
    using System.Runtime;

    public sealed class DesignerRegionMouseEventArgs : EventArgs
    {
        private Point _location;
        private DesignerRegion _region;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DesignerRegionMouseEventArgs(DesignerRegion region, Point location)
        {
            this._location = location;
            this._region = region;
        }

        public Point Location
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._location;
            }
        }

        public DesignerRegion Region
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._region;
            }
        }
    }
}

