namespace System.ComponentModel.Design
{
    using System;
    using System.Collections;
    using System.Runtime;
    using System.Runtime.Serialization;
    using System.Security;

    [Serializable]
    public sealed class ExceptionCollection : Exception
    {
        private ArrayList exceptions;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ExceptionCollection(ArrayList exceptions)
        {
            this.exceptions = exceptions;
        }

        private ExceptionCollection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.exceptions = (ArrayList) info.GetValue("exceptions", typeof(ArrayList));
        }

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("exceptions", this.exceptions);
            base.GetObjectData(info, context);
        }

        public ArrayList Exceptions
        {
            get
            {
                if (this.exceptions != null)
                {
                    return (ArrayList) this.exceptions.Clone();
                }
                return null;
            }
        }
    }
}

