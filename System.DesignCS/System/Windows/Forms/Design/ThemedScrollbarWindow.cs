namespace System.Windows.Forms.Design
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ThemedScrollbarWindow
    {
        public IntPtr Handle;
        public ThemedScrollbarMode Mode;
    }
}

