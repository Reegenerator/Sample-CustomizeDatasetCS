namespace System.ComponentModel.Design
{
    using System;
    using System.Runtime;

    public class ActiveDesignSurfaceChangedEventArgs : EventArgs
    {
        private DesignSurface _newSurface;
        private DesignSurface _oldSurface;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ActiveDesignSurfaceChangedEventArgs(DesignSurface oldSurface, DesignSurface newSurface)
        {
            this._oldSurface = oldSurface;
            this._newSurface = newSurface;
        }

        public DesignSurface NewSurface
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._newSurface;
            }
        }

        public DesignSurface OldSurface
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._oldSurface;
            }
        }
    }
}

