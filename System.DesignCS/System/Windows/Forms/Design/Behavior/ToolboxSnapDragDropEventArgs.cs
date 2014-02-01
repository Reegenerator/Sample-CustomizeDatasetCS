﻿namespace System.Windows.Forms.Design.Behavior
{
    using System;
    using System.Drawing;
    using System.Runtime;
    using System.Windows.Forms;

    internal sealed class ToolboxSnapDragDropEventArgs : DragEventArgs
    {
        private Point offset;
        private SnapDirection snapDirections;

        public ToolboxSnapDragDropEventArgs(SnapDirection snapDirections, Point offset, DragEventArgs origArgs) : base(origArgs.Data, origArgs.KeyState, origArgs.X, origArgs.Y, origArgs.AllowedEffect, origArgs.Effect)
        {
            this.snapDirections = snapDirections;
            this.offset = offset;
        }

        public Point Offset
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.offset;
            }
        }

        public SnapDirection SnapDirections
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.snapDirections;
            }
        }

        [Flags]
        public enum SnapDirection
        {
            Bottom = 2,
            Left = 8,
            None = 0,
            Right = 4,
            Top = 1
        }
    }
}

