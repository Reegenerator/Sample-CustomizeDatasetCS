namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Design;
    using System.Globalization;
    using System.Runtime;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.Design.Util;
    using System.Web.UI.WebControls;
    using System.Windows.Forms;

    public class TreeViewDesigner : HierarchicalDataBoundControlDesigner
    {
        private static DesignerAutoFormatCollection _autoFormats;
        private bool _emptyDataBinding;
        private System.Web.UI.WebControls.TreeView _treeView;
        private bool _usingSampleData;
        private const string emptyDesignTimeHtml = "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface\">\r\n                  <tr><td><span style=\"font-weight:bold\">TreeView</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ";
        private const string errorDesignTimeHtml = "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow\">\r\n                  <tr><td><span style=\"font-weight:bold\">TreeView</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ";

        protected void CreateLineImages()
        {
            ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.CreateLineImagesCallBack), null, System.Design.SR.GetString("TreeViewDesigner_CreateLineImagesTransactionDescription"));
        }

        private bool CreateLineImagesCallBack(object context)
        {
            TreeViewImageGenerator form = new TreeViewImageGenerator(this._treeView);
            return (UIServiceHelper.ShowDialog(base.Component.Site, form) == DialogResult.OK);
        }

        protected override void DataBind(BaseDataBoundControl dataBoundControl)
        {
            System.Web.UI.WebControls.TreeView view = (System.Web.UI.WebControls.TreeView) dataBoundControl;
            this._usingSampleData = false;
            this._emptyDataBinding = false;
            if (((view.DataSourceID != null) && (view.DataSourceID.Length > 0)) || ((view.DataSource != null) || (view.Nodes.Count == 0)))
            {
                view.Nodes.Clear();
                base.DataBind(view);
            }
            if (this._usingSampleData)
            {
                view.ExpandAll();
            }
            else
            {
                this.ExpandToDepth(view.Nodes, view.ExpandDepth);
                if (view.Nodes.Count == 0)
                {
                    this._emptyDataBinding = true;
                }
            }
        }

        protected void EditBindings()
        {
            IServiceProvider site = this._treeView.Site;
            TreeViewBindingsEditorForm form = new TreeViewBindingsEditorForm(site, this._treeView, this);
            UIServiceHelper.ShowDialog(site, form);
        }

        protected void EditNodes()
        {
            PropertyDescriptor member = TypeDescriptor.GetProperties(base.Component)["Nodes"];
            ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EditNodesChangeCallback), null, System.Design.SR.GetString("TreeViewDesigner_EditNodesTransactionDescription"), member);
        }

        private bool EditNodesChangeCallback(object context)
        {
            IServiceProvider site = this._treeView.Site;
            TreeNodeCollectionEditorDialog form = new TreeNodeCollectionEditorDialog(this._treeView, this);
            return (UIServiceHelper.ShowDialog(site, form) == DialogResult.OK);
        }

        private void ExpandToDepth(System.Web.UI.WebControls.TreeNodeCollection nodes, int depth)
        {
            foreach (System.Web.UI.WebControls.TreeNode node in nodes)
            {
                if ((node.Expanded != false) && ((depth == -1) || (node.Depth < depth)))
                {
                    node.Expanded = true;
                    this.ExpandToDepth(node.ChildNodes, depth);
                }
            }
        }

        public override string GetDesignTimeHtml()
        {
            string designTimeHtml = base.GetDesignTimeHtml();
            if (this._emptyDataBinding)
            {
                designTimeHtml = this.GetEmptyDataBindingDesignTimeHtml();
            }
            return designTimeHtml;
        }

        private string GetEmptyDataBindingDesignTimeHtml()
        {
            string name = this._treeView.Site.Name;
            return string.Format(CultureInfo.CurrentUICulture, "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface\">\r\n                  <tr><td><span style=\"font-weight:bold\">TreeView</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ", new object[] { name, System.Design.SR.GetString("TreeViewDesigner_EmptyDataBinding") });
        }

        protected override string GetEmptyDesignTimeHtml()
        {
            string name = this._treeView.Site.Name;
            return string.Format(CultureInfo.CurrentUICulture, "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface\">\r\n                  <tr><td><span style=\"font-weight:bold\">TreeView</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ", new object[] { name, System.Design.SR.GetString("TreeViewDesigner_Empty") });
        }

        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            string name = this._treeView.Site.Name;
            return string.Format(CultureInfo.CurrentUICulture, "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow\">\r\n                  <tr><td><span style=\"font-weight:bold\">TreeView</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ", new object[] { name, System.Design.SR.GetString("TreeViewDesigner_Error", new object[] { e.Message }) });
        }

        protected override IHierarchicalEnumerable GetSampleDataSource()
        {
            this._usingSampleData = true;
            ((System.Web.UI.WebControls.TreeView) base.ViewControl).AutoGenerateDataBindings = true;
            return base.GetSampleDataSource();
        }

        public override void Initialize(IComponent component)
        {
            ControlDesigner.VerifyInitializeArgument(component, typeof(System.Web.UI.WebControls.TreeView));
            base.Initialize(component);
            this._treeView = (System.Web.UI.WebControls.TreeView) component;
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal void InvokeTreeNodeCollectionEditor()
        {
            this.EditNodes();
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal void InvokeTreeViewBindingsEditor()
        {
            this.EditBindings();
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                DesignerActionListCollection lists = new DesignerActionListCollection();
                lists.AddRange(base.ActionLists);
                lists.Add(new TreeViewDesignerActionList(this));
                return lists;
            }
        }

        public override DesignerAutoFormatCollection AutoFormats
        {
            get
            {
                if (_autoFormats == null)
                {
                    if (CS9__CachedAnonymousMethodDelegate1 == null)
                    {
                        CS9__CachedAnonymousMethodDelegate1 = new Func<string, DesignerAutoFormat>(null, (IntPtr) <get_AutoFormats>b__0);
                    }
                    _autoFormats = ControlDesigner.CreateAutoFormats(AutoFormatSchemes.TREEVIEW_SCHEME_NAMES, CS9__CachedAnonymousMethodDelegate1);
                }
                return _autoFormats;
            }
        }

        protected override bool UsePreviewControl
        {
            get
            {
                return true;
            }
        }

        private class TreeViewDesignerActionList : DesignerActionList
        {
            private TreeViewDesigner _parent;

            public TreeViewDesignerActionList(TreeViewDesigner parent) : base(parent.Component)
            {
                this._parent = parent;
            }

            public void CreateLineImages()
            {
                this._parent.CreateLineImages();
            }

            public void EditBindings()
            {
                this._parent.EditBindings();
            }

            public void EditNodes()
            {
                this._parent.EditNodes();
            }

            public override DesignerActionItemCollection GetSortedActionItems()
            {
                DesignerActionItemCollection items = new DesignerActionItemCollection();
                string category = System.Design.SR.GetString("TreeViewDesigner_DataActionGroup");
                if (string.IsNullOrEmpty(this._parent.DataSourceID))
                {
                    items.Add(new DesignerActionMethodItem(this, "EditNodes", System.Design.SR.GetString("TreeViewDesigner_EditNodes"), category, System.Design.SR.GetString("TreeViewDesigner_EditNodesDescription"), true));
                }
                else
                {
                    items.Add(new DesignerActionMethodItem(this, "EditBindings", System.Design.SR.GetString("TreeViewDesigner_EditBindings"), category, System.Design.SR.GetString("TreeViewDesigner_EditBindingsDescription"), true));
                }
                if (this.ShowLines)
                {
                    items.Add(new DesignerActionMethodItem(this, "CreateLineImages", System.Design.SR.GetString("TreeViewDesigner_CreateLineImages"), category, System.Design.SR.GetString("TreeViewDesigner_CreateLineImagesDescription"), true));
                }
                items.Add(new DesignerActionPropertyItem("ShowLines", System.Design.SR.GetString("TreeViewDesigner_ShowLines"), "Actions", System.Design.SR.GetString("TreeViewDesigner_ShowLinesDescription")));
                return items;
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

            public bool ShowLines
            {
                get
                {
                    return ((System.Web.UI.WebControls.TreeView) base.Component).ShowLines;
                }
                set
                {
                    TypeDescriptor.GetProperties(typeof(System.Web.UI.WebControls.TreeView))["ShowLines"].SetValue(base.Component, value);
                    TypeDescriptor.Refresh(base.Component);
                }
            }
        }
    }
}

