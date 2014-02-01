namespace System.Diagnostics.Design
{
    using System;
    using System.Runtime;

    internal class EditableDictionaryEntry
    {
        public string _name;
        public string _value;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public EditableDictionaryEntry(string name, string value)
        {
            this._name = name;
            this._value = value;
        }

        public string Name
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._name;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._name = value;
            }
        }

        public string Value
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._value;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._value = value;
            }
        }
    }
}

