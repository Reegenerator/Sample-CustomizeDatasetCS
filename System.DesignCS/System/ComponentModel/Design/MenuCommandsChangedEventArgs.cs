namespace System.ComponentModel.Design
{
    using System;
    using System.Runtime;
    using System.Runtime.InteropServices;

    [ComVisible(true)]
    public class MenuCommandsChangedEventArgs : EventArgs
    {
        private MenuCommandsChangedType changeType;
        private MenuCommand command;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public MenuCommandsChangedEventArgs(MenuCommandsChangedType changeType, MenuCommand command)
        {
            this.changeType = changeType;
            this.command = command;
        }

        public MenuCommandsChangedType ChangeType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.changeType;
            }
        }

        public MenuCommand Command
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.command;
            }
        }
    }
}

