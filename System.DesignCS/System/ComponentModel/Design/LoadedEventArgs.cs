namespace System.ComponentModel.Design
{
    using System;
    using System.Collections;
    using System.Runtime;

    public sealed class LoadedEventArgs : EventArgs
    {
        private ICollection _errors;
        private bool _succeeded;

        public LoadedEventArgs(bool succeeded, ICollection errors)
        {
            this._succeeded = succeeded;
            this._errors = errors;
            if (this._errors == null)
            {
                this._errors = new object[0];
            }
        }

        public ICollection Errors
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._errors;
            }
        }

        public bool HasSucceeded
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._succeeded;
            }
        }
    }
}

