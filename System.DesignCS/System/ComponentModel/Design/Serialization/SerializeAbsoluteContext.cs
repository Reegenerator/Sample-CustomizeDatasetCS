namespace System.ComponentModel.Design.Serialization
{
    using System;
    using System.ComponentModel;
    using System.Runtime;

    public sealed class SerializeAbsoluteContext
    {
        private MemberDescriptor _member;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public SerializeAbsoluteContext()
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public SerializeAbsoluteContext(MemberDescriptor member)
        {
            this._member = member;
        }

        public bool ShouldSerialize(MemberDescriptor member)
        {
            if (this._member != null)
            {
                return (this._member == member);
            }
            return true;
        }

        public MemberDescriptor Member
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._member;
            }
        }
    }
}

