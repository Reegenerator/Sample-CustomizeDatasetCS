namespace Microsoft.Internal.Performance
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    internal sealed class CodeMarkers
    {
        private const string AtomName = "VSCodeMarkersEnabled";
        private const string DllName = "Microsoft.Internal.Performance.CodeMarkers.dll";
        public static readonly CodeMarkers Instance = new CodeMarkers();
        private State state = ((NativeMethods.FindAtom("VSCodeMarkersEnabled") != 0) ? State.Enabled : State.Disabled);

        private CodeMarkers()
        {
        }

        public bool CodeMarker(int nTimerID)
        {
            if (!this.IsEnabled)
            {
                return false;
            }
            try
            {
                NativeMethods.DllPerfCodeMarker(nTimerID, null, 0);
            }
            catch (DllNotFoundException)
            {
                this.state = State.DisabledDueToDllImportException;
                return false;
            }
            return true;
        }

        public bool CodeMarkerEx(int nTimerID, byte[] aBuff)
        {
            if (!this.IsEnabled)
            {
                return false;
            }
            if (aBuff == null)
            {
                throw new ArgumentNullException("aBuff");
            }
            try
            {
                NativeMethods.DllPerfCodeMarker(nTimerID, aBuff, aBuff.Length);
            }
            catch (DllNotFoundException)
            {
                this.state = State.DisabledDueToDllImportException;
                return false;
            }
            return true;
        }

        public bool CodeMarkerEx(int nTimerID, Guid guidData)
        {
            return this.CodeMarkerEx(nTimerID, guidData.ToByteArray());
        }

        public bool CodeMarkerEx(int nTimerID, string stringData)
        {
            return this.CodeMarkerEx(nTimerID, Encoding.Unicode.GetBytes(stringData));
        }

        public bool CodeMarkerEx(int nTimerID, uint uintData)
        {
            return this.CodeMarkerEx(nTimerID, BitConverter.GetBytes(uintData));
        }

        public bool CodeMarkerEx(int nTimerID, ulong ulongData)
        {
            return this.CodeMarkerEx(nTimerID, BitConverter.GetBytes(ulongData));
        }

        public bool IsEnabled
        {
            get
            {
                return (this.state == State.Enabled);
            }
        }

        private static class NativeMethods
        {
            [DllImport("Microsoft.Internal.Performance.CodeMarkers.dll", EntryPoint="PerfCodeMarker")]
            public static extern void DllPerfCodeMarker(int nTimerID, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] byte[] aUserParams, int cbParams);
            [DllImport("kernel32.dll", CharSet=CharSet.Unicode)]
            public static extern ushort FindAtom([MarshalAs(UnmanagedType.LPWStr)] string lpString);
        }

        private enum State
        {
            Enabled,
            Disabled,
            DisabledDueToDllImportException,
            DisabledViaRegistryCheck
        }
    }
}

