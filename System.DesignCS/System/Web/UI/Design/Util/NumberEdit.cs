namespace System.Web.UI.Design.Util
{
    using System;
    using System.Globalization;
    using System.Runtime;
    using System.Security.Permissions;
    using System.Windows.Forms;

    [SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.UnmanagedCode)]
    internal sealed class NumberEdit : TextBox
    {
        private bool allowDecimal = true;
        private bool allowNegative = true;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x102)
            {
                char wParam = (char) ((int) m.WParam);
                if ((((wParam < '0') || (wParam > '9')) && (!NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.Contains(wParam.ToString(CultureInfo.CurrentCulture)) || !this.allowDecimal)) && ((!NumberFormatInfo.CurrentInfo.NegativeSign.Contains(wParam.ToString(CultureInfo.CurrentCulture)) || !this.allowNegative) && (wParam != '\b')))
                {
                    Console.Beep();
                    return;
                }
            }
            base.WndProc(ref m);
        }

        public bool AllowDecimal
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.allowDecimal;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.allowDecimal = value;
            }
        }

        public bool AllowNegative
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.allowNegative;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.allowNegative = value;
            }
        }
    }
}

