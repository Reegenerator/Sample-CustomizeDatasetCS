namespace System.Web.UI.Design
{
    using System;
    using System.Runtime;

    public class ViewEventArgs : System.EventArgs
    {
        private System.EventArgs _eventArgs;
        private ViewEvent _eventType;
        private DesignerRegion _region;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ViewEventArgs(ViewEvent eventType, DesignerRegion region, System.EventArgs eventArgs)
        {
            this._eventType = eventType;
            this._region = region;
            this._eventArgs = eventArgs;
        }

        public System.EventArgs EventArgs
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._eventArgs;
            }
        }

        public ViewEvent EventType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._eventType;
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

