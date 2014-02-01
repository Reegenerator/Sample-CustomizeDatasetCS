﻿namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.ComponentModel.Design;
    using System.Design;
    using System.Runtime;

    internal class FormViewActionList : DesignerActionList
    {
        private bool _allowDynamicData;
        private bool _allowPaging;
        private FormViewDesigner _formViewDesigner;

        public FormViewActionList(FormViewDesigner formViewDesigner) : base(formViewDesigner.Component)
        {
            this._formViewDesigner = formViewDesigner;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            if (this.AllowDynamicData)
            {
                items.Add(new DesignerActionPropertyItem("EnableDynamicData", System.Design.SR.GetString("FormView_EnableDynamicData"), "Behavior", System.Design.SR.GetString("FormView_EnableDynamicDataDesc")));
            }
            if (this.AllowPaging)
            {
                items.Add(new DesignerActionPropertyItem("EnablePaging", System.Design.SR.GetString("FormView_EnablePaging"), "Behavior", System.Design.SR.GetString("FormView_EnablePagingDesc")));
            }
            return items;
        }

        internal bool AllowDynamicData
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._allowDynamicData;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._allowDynamicData = value;
            }
        }

        internal bool AllowPaging
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._allowPaging;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._allowPaging = value;
            }
        }

        public override bool AutoShow
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public bool EnableDynamicData
        {
            get
            {
                return this._formViewDesigner.EnableDynamicData;
            }
            set
            {
                this._formViewDesigner.EnableDynamicData = value;
            }
        }

        public bool EnablePaging
        {
            get
            {
                return this._formViewDesigner.EnablePaging;
            }
            set
            {
                this._formViewDesigner.EnablePaging = value;
            }
        }
    }
}

