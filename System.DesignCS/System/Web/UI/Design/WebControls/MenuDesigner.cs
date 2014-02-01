namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
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

    public class MenuDesigner : HierarchicalDataBoundControlDesigner, IDataBindingSchemaProvider
    {
        private static DesignerAutoFormatCollection _autoFormats;
        private ViewType _currentView;
        private const string _getDesignTimeDynamicHtml = "GetDesignTimeDynamicHtml";
        private const string _getDesignTimeStaticHtml = "GetDesignTimeStaticHtml";
        private const int _maxDesignDepth = 10;
        private System.Web.UI.WebControls.Menu _menu;
        private TemplateGroupCollection _templateGroups;
        private static readonly string[] _templateNames = new string[] { "StaticItemTemplate", "DynamicItemTemplate" };
        private const string emptyDesignTimeHtml = "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface\">\r\n                  <tr><td><span style=\"font-weight:bold\">Menu</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ";
        private const string errorDesignTimeHtml = "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow\">\r\n                  <tr><td><span style=\"font-weight:bold\">Menu</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ";

        private void ConvertToDynamicTemplate()
        {
            ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConvertToDynamicTemplateChangeCallback), null, System.Design.SR.GetString("MenuDesigner_ConvertToDynamicTemplate"));
        }

        private bool ConvertToDynamicTemplateChangeCallback(object context)
        {
            string templateText = null;
            string dynamicItemFormatString = this._menu.DynamicItemFormatString;
            if ((dynamicItemFormatString != null) && (dynamicItemFormatString.Length != 0))
            {
                templateText = "<%# Eval(\"Text\", \"" + dynamicItemFormatString + "\") %>";
            }
            else
            {
                templateText = "<%# Eval(\"Text\") %>";
            }
            IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            if (service != null)
            {
                this._menu.DynamicItemTemplate = ControlParser.ParseTemplate(service, templateText);
            }
            return true;
        }

        private void ConvertToStaticTemplate()
        {
            ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConvertToStaticTemplateChangeCallback), null, System.Design.SR.GetString("MenuDesigner_ConvertToStaticTemplate"));
        }

        private bool ConvertToStaticTemplateChangeCallback(object context)
        {
            string templateText = null;
            string staticItemFormatString = this._menu.StaticItemFormatString;
            if ((staticItemFormatString != null) && (staticItemFormatString.Length != 0))
            {
                templateText = "<%# Eval(\"Text\", \"" + staticItemFormatString + "\") %>";
            }
            else
            {
                templateText = "<%# Eval(\"Text\") %>";
            }
            IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            if (service != null)
            {
                this._menu.StaticItemTemplate = ControlParser.ParseTemplate(service, templateText);
            }
            return true;
        }

        protected override void DataBind(BaseDataBoundControl dataBoundControl)
        {
            System.Web.UI.WebControls.Menu menu = (System.Web.UI.WebControls.Menu) dataBoundControl;
            if (((menu.DataSourceID != null) && (menu.DataSourceID.Length > 0)) || ((menu.DataSource != null) || (menu.Items.Count == 0)))
            {
                menu.Items.Clear();
                base.DataBind(menu);
            }
        }

        private void EditBindings()
        {
            IServiceProvider site = this._menu.Site;
            MenuBindingsEditorForm form = new MenuBindingsEditorForm(site, this._menu, this);
            UIServiceHelper.ShowDialog(site, form);
        }

        private void EditMenuItems()
        {
            PropertyDescriptor member = TypeDescriptor.GetProperties(base.Component)["Items"];
            ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EditMenuItemsChangeCallback), null, System.Design.SR.GetString("MenuDesigner_EditNodesTransactionDescription"), member);
        }

        private bool EditMenuItemsChangeCallback(object context)
        {
            IServiceProvider site = this._menu.Site;
            MenuItemCollectionEditorDialog form = new MenuItemCollectionEditorDialog(this._menu, this);
            return (UIServiceHelper.ShowDialog(site, form) == DialogResult.OK);
        }

        public override string GetDesignTimeHtml()
        {
            try
            {
                System.Web.UI.WebControls.Menu viewControl = (System.Web.UI.WebControls.Menu) base.ViewControl;
                ListDictionary data = new ListDictionary();
                data.Add("DesignTimeTextWriterType", typeof(DesignTimeHtmlTextWriter));
                ((IControlDesignerAccessor) base.ViewControl).SetDesignModeState(data);
                int maximumDynamicDisplayLevels = viewControl.MaximumDynamicDisplayLevels;
                if (maximumDynamicDisplayLevels > 10)
                {
                    viewControl.MaximumDynamicDisplayLevels = 10;
                }
                this.DataBind((BaseDataBoundControl) base.ViewControl);
                IDictionary designModeState = ((IControlDesignerAccessor) base.ViewControl).GetDesignModeState();
                switch (this._currentView)
                {
                    case ViewType.Static:
                        return (string) designModeState["GetDesignTimeStaticHtml"];

                    case ViewType.Dynamic:
                        return (string) designModeState["GetDesignTimeDynamicHtml"];
                }
                if (maximumDynamicDisplayLevels > 10)
                {
                    viewControl.MaximumDynamicDisplayLevels = maximumDynamicDisplayLevels;
                }
                return base.GetDesignTimeHtml();
            }
            catch (Exception exception)
            {
                return this.GetErrorDesignTimeHtml(exception);
            }
        }

        protected override string GetEmptyDesignTimeHtml()
        {
            string name = this._menu.Site.Name;
            return string.Format(CultureInfo.CurrentUICulture, "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface\">\r\n                  <tr><td><span style=\"font-weight:bold\">Menu</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ", new object[] { name, System.Design.SR.GetString("MenuDesigner_Empty") });
        }

        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            string name = this._menu.Site.Name;
            return string.Format(CultureInfo.CurrentUICulture, "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow\">\r\n                  <tr><td><span style=\"font-weight:bold\">Menu</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ", new object[] { name, System.Design.SR.GetString("MenuDesigner_Error", new object[] { e.Message }) });
        }

        protected override IHierarchicalEnumerable GetSampleDataSource()
        {
            return new MenuSampleData(this._menu, 0, string.Empty);
        }

        public override void Initialize(IComponent component)
        {
            ControlDesigner.VerifyInitializeArgument(component, typeof(System.Web.UI.WebControls.Menu));
            base.Initialize(component);
            this._menu = (System.Web.UI.WebControls.Menu) component;
            base.SetViewFlags(ViewFlags.TemplateEditing, true);
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal void InvokeMenuBindingsEditor()
        {
            this.EditBindings();
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal void InvokeMenuItemCollectionEditor()
        {
            this.EditMenuItems();
        }

        protected void RefreshSchema(bool preferSilent)
        {
        }

        private void ResetDynamicTemplate()
        {
            ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ResetDynamicTemplateChangeCallback), null, System.Design.SR.GetString("MenuDesigner_ResetDynamicTemplate"));
        }

        private bool ResetDynamicTemplateChangeCallback(object context)
        {
            this._menu.Controls.Clear();
            this._menu.DynamicItemTemplate = null;
            return true;
        }

        private void ResetStaticTemplate()
        {
            ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ResetStaticTemplateChangeCallback), null, System.Design.SR.GetString("MenuDesigner_ResetStaticTemplate"));
        }

        private bool ResetStaticTemplateChangeCallback(object context)
        {
            this._menu.Controls.Clear();
            this._menu.StaticItemTemplate = null;
            return true;
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        void IDataBindingSchemaProvider.RefreshSchema(bool preferSilent)
        {
            this.RefreshSchema(preferSilent);
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                DesignerActionListCollection lists = new DesignerActionListCollection();
                lists.AddRange(base.ActionLists);
                lists.Add(new MenuDesignerActionList(this));
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
                    _autoFormats = ControlDesigner.CreateAutoFormats(AutoFormatSchemes.MENU_SCHEME_NAMES, CS9__CachedAnonymousMethodDelegate1);
                }
                return _autoFormats;
            }
        }

        protected bool CanRefreshSchema
        {
            get
            {
                return false;
            }
        }

        private bool DynamicTemplated
        {
            get
            {
                return (this._menu.DynamicItemTemplate != null);
            }
        }

        protected IDataSourceViewSchema Schema
        {
            get
            {
                return new MenuItemSchema();
            }
        }

        private bool StaticTemplated
        {
            get
            {
                return (this._menu.StaticItemTemplate != null);
            }
        }

        bool IDataBindingSchemaProvider.CanRefreshSchema
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.CanRefreshSchema;
            }
        }

        IDataSourceViewSchema IDataBindingSchemaProvider.Schema
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.Schema;
            }
        }

        public override TemplateGroupCollection TemplateGroups
        {
            get
            {
                TemplateGroupCollection templateGroups = base.TemplateGroups;
                if (this._templateGroups == null)
                {
                    this._templateGroups = new TemplateGroupCollection();
                    TemplateGroup group = new TemplateGroup("Item Templates", ((WebControl) base.ViewControl).ControlStyle);
                    TemplateDefinition templateDefinition = new TemplateDefinition(this, _templateNames[0], this._menu, _templateNames[0], ((System.Web.UI.WebControls.Menu) base.ViewControl).StaticMenuStyle) {
                        SupportsDataBinding = true
                    };
                    group.AddTemplateDefinition(templateDefinition);
                    TemplateDefinition definition2 = new TemplateDefinition(this, _templateNames[1], this._menu, _templateNames[1], ((System.Web.UI.WebControls.Menu) base.ViewControl).DynamicMenuStyle) {
                        SupportsDataBinding = true
                    };
                    group.AddTemplateDefinition(definition2);
                    this._templateGroups.Add(group);
                }
                templateGroups.AddRange(this._templateGroups);
                return templateGroups;
            }
        }

        protected override bool UsePreviewControl
        {
            get
            {
                return true;
            }
        }

        private class MenuDesignerActionList : DesignerActionList
        {
            private MenuDesigner _parent;

            public MenuDesignerActionList(MenuDesigner parent) : base(parent.Component)
            {
                this._parent = parent;
            }

            public void ConvertToDynamicTemplate()
            {
                this._parent.ConvertToDynamicTemplate();
            }

            public void ConvertToStaticTemplate()
            {
                this._parent.ConvertToStaticTemplate();
            }

            public void EditBindings()
            {
                this._parent.EditBindings();
            }

            public void EditMenuItems()
            {
                this._parent.EditMenuItems();
            }

            public override DesignerActionItemCollection GetSortedActionItems()
            {
                DesignerActionItemCollection items = new DesignerActionItemCollection();
                string category = System.Design.SR.GetString("MenuDesigner_DataActionGroup");
                DesignerActionPropertyItem item = new DesignerActionPropertyItem("View", System.Design.SR.GetString("WebControls_Views"), category, System.Design.SR.GetString("MenuDesigner_ViewsDescription")) {
                    ShowInSourceView = false
                };
                items.Add(item);
                if (string.IsNullOrEmpty(this._parent.DataSourceID))
                {
                    items.Add(new DesignerActionMethodItem(this, "EditMenuItems", System.Design.SR.GetString("MenuDesigner_EditMenuItems"), category, System.Design.SR.GetString("MenuDesigner_EditMenuItemsDescription"), true));
                }
                else
                {
                    items.Add(new DesignerActionMethodItem(this, "EditBindings", System.Design.SR.GetString("MenuDesigner_EditBindings"), category, System.Design.SR.GetString("MenuDesigner_EditBindingsDescription"), true));
                }
                if (this._parent.DynamicTemplated)
                {
                    items.Add(new DesignerActionMethodItem(this, "ResetDynamicTemplate", System.Design.SR.GetString("MenuDesigner_ResetDynamicTemplate"), category, System.Design.SR.GetString("MenuDesigner_ResetDynamicTemplateDescription"), true));
                }
                else
                {
                    items.Add(new DesignerActionMethodItem(this, "ConvertToDynamicTemplate", System.Design.SR.GetString("MenuDesigner_ConvertToDynamicTemplate"), category, System.Design.SR.GetString("MenuDesigner_ConvertToDynamicTemplateDescription"), true));
                }
                if (this._parent.StaticTemplated)
                {
                    items.Add(new DesignerActionMethodItem(this, "ResetStaticTemplate", System.Design.SR.GetString("MenuDesigner_ResetStaticTemplate"), category, System.Design.SR.GetString("MenuDesigner_ResetStaticTemplateDescription"), true));
                    return items;
                }
                items.Add(new DesignerActionMethodItem(this, "ConvertToStaticTemplate", System.Design.SR.GetString("MenuDesigner_ConvertToStaticTemplate"), category, System.Design.SR.GetString("MenuDesigner_ConvertToStaticTemplateDescription"), true));
                return items;
            }

            public void ResetDynamicTemplate()
            {
                this._parent.ResetDynamicTemplate();
            }

            public void ResetStaticTemplate()
            {
                this._parent.ResetStaticTemplate();
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

            [TypeConverter(typeof(MenuViewTypeConverter))]
            public string View
            {
                get
                {
                    if (this._parent._currentView == MenuDesigner.ViewType.Static)
                    {
                        return System.Design.SR.GetString("Menu_StaticView");
                    }
                    if (this._parent._currentView == MenuDesigner.ViewType.Dynamic)
                    {
                        return System.Design.SR.GetString("Menu_DynamicView");
                    }
                    return string.Empty;
                }
                set
                {
                    if (string.Compare(value, System.Design.SR.GetString("Menu_StaticView"), StringComparison.Ordinal) == 0)
                    {
                        this._parent._currentView = MenuDesigner.ViewType.Static;
                    }
                    else if (string.Compare(value, System.Design.SR.GetString("Menu_DynamicView"), StringComparison.Ordinal) == 0)
                    {
                        this._parent._currentView = MenuDesigner.ViewType.Dynamic;
                    }
                    TypeDescriptor.Refresh(this._parent.Component);
                    this._parent.UpdateDesignTimeHtml();
                }
            }

            private class MenuViewTypeConverter : TypeConverter
            {
                public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
                {
                    return new TypeConverter.StandardValuesCollection(new string[] { System.Design.SR.GetString("Menu_StaticView"), System.Design.SR.GetString("Menu_DynamicView") });
                }

                public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
                {
                    return true;
                }

                public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
                {
                    return true;
                }
            }
        }

        private class MenuItemSchema : IDataSourceViewSchema
        {
            private static IDataSourceFieldSchema[] _fieldSchema;

            static MenuItemSchema()
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(System.Web.UI.WebControls.MenuItem));
                _fieldSchema = new IDataSourceFieldSchema[] { new TypeFieldSchema(properties["DataPath"]), new TypeFieldSchema(properties["Depth"]), new TypeFieldSchema(properties["Enabled"]), new TypeFieldSchema(properties["ImageUrl"]), new TypeFieldSchema(properties["NavigateUrl"]), new TypeFieldSchema(properties["PopOutImageUrl"]), new TypeFieldSchema(properties["Selectable"]), new TypeFieldSchema(properties["Selected"]), new TypeFieldSchema(properties["SeparatorImageUrl"]), new TypeFieldSchema(properties["Target"]), new TypeFieldSchema(properties["Text"]), new TypeFieldSchema(properties["ToolTip"]), new TypeFieldSchema(properties["Value"]), new TypeFieldSchema(properties["ValuePath"]) };
            }

            IDataSourceViewSchema[] IDataSourceViewSchema.GetChildren()
            {
                return new IDataSourceViewSchema[0];
            }

            IDataSourceFieldSchema[] IDataSourceViewSchema.GetFields()
            {
                return _fieldSchema;
            }

            string IDataSourceViewSchema.Name
            {
                get
                {
                    return "MenuItem";
                }
            }
        }

        private class MenuSampleData : IHierarchicalEnumerable, IEnumerable
        {
            private ArrayList _list = new ArrayList();
            private System.Web.UI.WebControls.Menu _menu;

            public MenuSampleData(System.Web.UI.WebControls.Menu menu, int depth, string path)
            {
                this._menu = menu;
                int num = this._menu.StaticDisplayLevels + this._menu.MaximumDynamicDisplayLevels;
                if ((num < this._menu.StaticDisplayLevels) || (num < this._menu.MaximumDynamicDisplayLevels))
                {
                    num = 0x7fffffff;
                }
                if (depth == 0)
                {
                    this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, System.Design.SR.GetString("HierarchicalDataBoundControlDesigner_SampleRoot"), depth, path, false));
                    this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, System.Design.SR.GetString("HierarchicalDataBoundControlDesigner_SampleRoot"), depth, path));
                    this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, System.Design.SR.GetString("HierarchicalDataBoundControlDesigner_SampleRoot"), depth, path, false));
                    this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, System.Design.SR.GetString("HierarchicalDataBoundControlDesigner_SampleRoot"), depth, path, false));
                    this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, System.Design.SR.GetString("HierarchicalDataBoundControlDesigner_SampleRoot"), depth, path, false));
                }
                else if ((depth <= this._menu.StaticDisplayLevels) && (depth < 10))
                {
                    this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, System.Design.SR.GetString("HierarchicalDataBoundControlDesigner_SampleParent", new object[] { depth }), depth, path));
                }
                else if ((depth < num) && (depth < 10))
                {
                    this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, System.Design.SR.GetString("HierarchicalDataBoundControlDesigner_SampleLeaf", new object[] { 1 }), depth, path));
                    this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, System.Design.SR.GetString("HierarchicalDataBoundControlDesigner_SampleLeaf", new object[] { 2 }), depth, path));
                    this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, System.Design.SR.GetString("HierarchicalDataBoundControlDesigner_SampleLeaf", new object[] { 3 }), depth, path));
                    this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, System.Design.SR.GetString("HierarchicalDataBoundControlDesigner_SampleLeaf", new object[] { 4 }), depth, path));
                }
            }

            public IEnumerator GetEnumerator()
            {
                return this._list.GetEnumerator();
            }

            public IHierarchyData GetHierarchyData(object enumeratedItem)
            {
                return (IHierarchyData) enumeratedItem;
            }
        }

        private class MenuSampleDataNode : IHierarchyData
        {
            private int _depth;
            private bool _hasChildren;
            private System.Web.UI.WebControls.Menu _menu;
            private string _path;
            private string _text;

            public MenuSampleDataNode(System.Web.UI.WebControls.Menu menu, string text, int depth, string path) : this(menu, text, depth, path, true)
            {
            }

            public MenuSampleDataNode(System.Web.UI.WebControls.Menu menu, string text, int depth, string path, bool hasChildren)
            {
                this._text = text;
                this._depth = depth;
                this._path = path + '\\' + text;
                this._menu = menu;
                this._hasChildren = hasChildren;
            }

            public IHierarchicalEnumerable GetChildren()
            {
                return new MenuDesigner.MenuSampleData(this._menu, this._depth + 1, this._path);
            }

            public IHierarchyData GetParent()
            {
                return null;
            }

            public override string ToString()
            {
                return this._text;
            }

            public bool HasChildren
            {
                get
                {
                    if (!this._hasChildren)
                    {
                        return false;
                    }
                    int num = this._menu.StaticDisplayLevels + this._menu.MaximumDynamicDisplayLevels;
                    if ((num < this._menu.StaticDisplayLevels) || (num < this._menu.MaximumDynamicDisplayLevels))
                    {
                        num = 0x7fffffff;
                    }
                    return ((this._depth < num) && (this._depth < 10));
                }
            }

            public object Item
            {
                get
                {
                    return this;
                }
            }

            public string Path
            {
                get
                {
                    return this._path;
                }
            }

            public string Type
            {
                get
                {
                    return "SampleData";
                }
            }
        }

        private enum ViewType
        {
            Static,
            Dynamic
        }
    }
}

