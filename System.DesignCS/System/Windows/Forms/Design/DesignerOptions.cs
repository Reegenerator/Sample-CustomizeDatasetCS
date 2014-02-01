namespace System.Windows.Forms.Design
{
    using System;
    using System.ComponentModel;
    using System.Design;
    using System.Drawing;
    using System.Runtime;
    using System.Runtime.InteropServices;

    [ComVisible(true)]
    public class DesignerOptions
    {
        private bool enableComponentCache;
        private bool enableInSituEditing = true;
        private Size gridSize = new Size(8, 8);
        private const int maxGridSize = 200;
        private const int minGridSize = 2;
        private bool objectBoundSmartTagAutoShow = true;
        private bool showGrid = true;
        private bool snapToGrid = true;
        private bool useSmartTags;
        private bool useSnapLines;

        [System.Design.SRCategory("DesignerOptions_EnableInSituEditingCat"), Browsable(false), SRDisplayName("DesignerOptions_EnableInSituEditingDisplay"), System.Design.SRDescription("DesignerOptions_EnableInSituEditingDesc")]
        public virtual bool EnableInSituEditing
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.enableInSituEditing;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.enableInSituEditing = value;
            }
        }

        [SRDisplayName("DesignerOptions_GridSizeDisplayName"), System.Design.SRDescription("DesignerOptions_GridSizeDesc"), System.Design.SRCategory("DesignerOptions_LayoutSettings")]
        public virtual Size GridSize
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.gridSize;
            }
            set
            {
                if (value.Width < 2)
                {
                    value.Width = 2;
                }
                if (value.Height < 2)
                {
                    value.Height = 2;
                }
                if (value.Width > 200)
                {
                    value.Width = 200;
                }
                if (value.Height > 200)
                {
                    value.Height = 200;
                }
                this.gridSize = value;
            }
        }

        [System.Design.SRDescription("DesignerOptions_ObjectBoundSmartTagAutoShow"), System.Design.SRCategory("DesignerOptions_ObjectBoundSmartTagSettings"), SRDisplayName("DesignerOptions_ObjectBoundSmartTagAutoShowDisplayName")]
        public virtual bool ObjectBoundSmartTagAutoShow
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.objectBoundSmartTagAutoShow;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.objectBoundSmartTagAutoShow = value;
            }
        }

        [System.Design.SRCategory("DesignerOptions_LayoutSettings"), SRDisplayName("DesignerOptions_ShowGridDisplayName"), System.Design.SRDescription("DesignerOptions_ShowGridDesc")]
        public virtual bool ShowGrid
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.showGrid;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.showGrid = value;
            }
        }

        [System.Design.SRDescription("DesignerOptions_SnapToGridDesc"), System.Design.SRCategory("DesignerOptions_LayoutSettings"), SRDisplayName("DesignerOptions_SnapToGridDisplayName")]
        public virtual bool SnapToGrid
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.snapToGrid;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.snapToGrid = value;
            }
        }

        [System.Design.SRDescription("DesignerOptions_OptimizedCodeGen"), System.Design.SRCategory("DesignerOptions_CodeGenSettings"), SRDisplayName("DesignerOptions_CodeGenDisplay")]
        public virtual bool UseOptimizedCodeGeneration
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.enableComponentCache;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.enableComponentCache = value;
            }
        }

        [System.Design.SRCategory("DesignerOptions_LayoutSettings"), System.Design.SRDescription("DesignerOptions_UseSmartTags")]
        public virtual bool UseSmartTags
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.useSmartTags;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.useSmartTags = value;
            }
        }

        [System.Design.SRDescription("DesignerOptions_UseSnapLines"), System.Design.SRCategory("DesignerOptions_LayoutSettings")]
        public virtual bool UseSnapLines
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.useSnapLines;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.useSnapLines = value;
            }
        }
    }
}

