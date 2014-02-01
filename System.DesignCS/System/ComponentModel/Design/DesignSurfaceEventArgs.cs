namespace System.ComponentModel.Design
{
    using System;
    using System.Runtime;

    public class DesignSurfaceEventArgs : EventArgs
    {
        private DesignSurface _surface;

        public DesignSurfaceEventArgs(DesignSurface surface)
        {
            if (surface == null)
            {
                throw new ArgumentNullException("surface");
            }
            this._surface = surface;
        }

        public DesignSurface Surface
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._surface;
            }
        }
    }
}

