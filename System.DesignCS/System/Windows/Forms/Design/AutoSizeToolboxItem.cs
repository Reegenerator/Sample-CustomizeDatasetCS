namespace System.Windows.Forms.Design
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;
    using System.Runtime;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    [Serializable]
    internal class AutoSizeToolboxItem : ToolboxItem
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public AutoSizeToolboxItem()
        {
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public AutoSizeToolboxItem(System.Type toolType) : base(toolType)
        {
        }

        private AutoSizeToolboxItem(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
        }

        protected override IComponent[] CreateComponentsCore(IDesignerHost host)
        {
            IComponent[] componentArray = base.CreateComponentsCore(host);
            if (((componentArray != null) && (componentArray.Length > 0)) && (componentArray[0] is Control))
            {
                Control control = componentArray[0] as Control;
                control.AutoSize = true;
            }
            return componentArray;
        }
    }
}

