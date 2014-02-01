namespace System.Windows.Forms.Design
{
    using System;
    using System.Runtime;

    internal class ContainerSelectorActiveEventArgs : EventArgs
    {
        private readonly object component;
        private readonly ContainerSelectorActiveEventArgsType eventType;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ContainerSelectorActiveEventArgs(object component) : this(component, ContainerSelectorActiveEventArgsType.Mouse)
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ContainerSelectorActiveEventArgs(object component, ContainerSelectorActiveEventArgsType eventType)
        {
            this.component = component;
            this.eventType = eventType;
        }
    }
}

