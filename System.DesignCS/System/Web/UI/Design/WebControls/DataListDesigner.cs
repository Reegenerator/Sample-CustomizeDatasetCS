namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Design;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.Design.Util;
    using System.Web.UI.WebControls;
    using System.Windows.Forms;

    [SupportsPreviewControl(true), SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.UnmanagedCode)]
    public class DataListDesigner : BaseDataListDesigner
    {
        private static DesignerAutoFormatCollection _autoFormats;
        private const string breakString = "<br />";
        internal static TraceSwitch DataListDesignerSwitch = new TraceSwitch("DATALISTDESIGNER", "Enable DataList designer general purpose traces.");
        private static string[] HeaderFooterTemplateNames = new string[] { "HeaderTemplate", "FooterTemplate" };
        private const int HeaderFooterTemplates = 1;
        private const int IDX_ALTITEM_TEMPLATE = 1;
        private const int IDX_EDITITEM_TEMPLATE = 3;
        private const int IDX_FOOTER_TEMPLATE = 1;
        private const int IDX_HEADER_TEMPLATE = 0;
        private const int IDX_ITEM_TEMPLATE = 0;
        private const int IDX_SELITEM_TEMPLATE = 2;
        private const int IDX_SEPARATOR_TEMPLATE = 0;
        private static string[] ItemTemplateNames = new string[] { "ItemTemplate", "AlternatingItemTemplate", "SelectedItemTemplate", "EditItemTemplate" };
        private const int ItemTemplates = 0;
        private const int SeparatorTemplate = 2;
        private static string[] SeparatorTemplateNames = new string[] { "SeparatorTemplate" };
        private const string templateFieldString = "{0}: <asp:Label Text='<%# {1} %>' runat=\"server\" id=\"{2}Label\"/><br />";
        private TemplateEditingVerb[] templateVerbs;
        private bool templateVerbsDirty = true;

        private void CreateDefaultTemplate()
        {
            string text = string.Empty;
            StringBuilder builder = new StringBuilder();
            DataList component = (DataList) base.Component;
            IDataSourceViewSchema dataSourceSchema = this.GetDataSourceSchema();
            IDataSourceFieldSchema[] fields = null;
            if (dataSourceSchema != null)
            {
                fields = dataSourceSchema.GetFields();
            }
            if ((fields != null) && (fields.Length > 0))
            {
                foreach (IDataSourceFieldSchema schema2 in fields)
                {
                    string name = schema2.Name;
                    char[] chArray = new char[name.Length];
                    for (int i = 0; i < name.Length; i++)
                    {
                        char c = name[i];
                        if (char.IsLetterOrDigit(c) || (c == '_'))
                        {
                            chArray[i] = c;
                        }
                        else
                        {
                            chArray[i] = '_';
                        }
                    }
                    string str3 = new string(chArray);
                    builder.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:Label Text='<%# {1} %>' runat=\"server\" id=\"{2}Label\"/><br />", new object[] { name, DesignTimeDataBinding.CreateEvalExpression(name, string.Empty), str3 }));
                    builder.Append(Environment.NewLine);
                    if (schema2.PrimaryKey && (component.DataKeyField.Length == 0))
                    {
                        component.DataKeyField = name;
                    }
                }
                builder.Append("<br />");
                builder.Append(Environment.NewLine);
                text = builder.ToString();
            }
            if ((text != null) && (text.Length > 0))
            {
                try
                {
                    component.ItemTemplate = base.GetTemplateFromText(text, component.ItemTemplate);
                }
                catch
                {
                }
            }
        }

        [Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
        protected override ITemplateEditingFrame CreateTemplateEditingFrame(TemplateEditingVerb verb)
        {
            ITemplateEditingService service = (ITemplateEditingService) this.GetService(typeof(ITemplateEditingService));
            DataList viewControl = (DataList) base.ViewControl;
            string[] templateNames = null;
            Style[] templateStyles = null;
            switch (verb.Index)
            {
                case 0:
                    templateNames = ItemTemplateNames;
                    templateStyles = new Style[] { viewControl.ItemStyle, viewControl.AlternatingItemStyle, viewControl.SelectedItemStyle, viewControl.EditItemStyle };
                    break;

                case 1:
                    templateNames = HeaderFooterTemplateNames;
                    templateStyles = new Style[] { viewControl.HeaderStyle, viewControl.FooterStyle };
                    break;

                case 2:
                    templateNames = SeparatorTemplateNames;
                    templateStyles = new Style[] { viewControl.SeparatorStyle };
                    break;
            }
            return service.CreateFrame(this, verb.Text, templateNames, viewControl.ControlStyle, templateStyles);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeTemplateVerbs();
            }
            base.Dispose(disposing);
        }

        private void DisposeTemplateVerbs()
        {
            if (this.templateVerbs != null)
            {
                for (int i = 0; i < this.templateVerbs.Length; i++)
                {
                    this.templateVerbs[i].Dispose();
                }
                this.templateVerbs = null;
                this.templateVerbsDirty = true;
            }
        }

        [Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
        protected override TemplateEditingVerb[] GetCachedTemplateEditingVerbs()
        {
            if (this.templateVerbsDirty)
            {
                this.DisposeTemplateVerbs();
                this.templateVerbs = new TemplateEditingVerb[] { new TemplateEditingVerb(System.Design.SR.GetString("DataList_ItemTemplates"), 0, this), new TemplateEditingVerb(System.Design.SR.GetString("DataList_HeaderFooterTemplates"), 1, this), new TemplateEditingVerb(System.Design.SR.GetString("DataList_SeparatorTemplate"), 2, this) };
                this.templateVerbsDirty = false;
            }
            return this.templateVerbs;
        }

        private IDataSourceViewSchema GetDataSourceSchema()
        {
            DesignerDataSourceView designerView = base.DesignerView;
            if (designerView != null)
            {
                try
                {
                    return designerView.Schema;
                }
                catch (Exception exception)
                {
                    IComponentDesignerDebugService service = (IComponentDesignerDebugService) base.Component.Site.GetService(typeof(IComponentDesignerDebugService));
                    if (service != null)
                    {
                        service.Fail(System.Design.SR.GetString("DataSource_DebugService_FailedCall", new object[] { "DesignerDataSourceView.Schema", exception.Message }));
                    }
                }
            }
            return null;
        }

        public override string GetDesignTimeHtml()
        {
            bool templatesExist = this.TemplatesExist;
            if (templatesExist)
            {
                IEnumerable designTimeDataSource;
                DataList viewControl = (DataList) base.ViewControl;
                bool dummyDataSource = false;
                DesignerDataSourceView designerView = base.DesignerView;
                if (designerView == null)
                {
                    designTimeDataSource = base.GetDesignTimeDataSource(5, out dummyDataSource);
                }
                else
                {
                    try
                    {
                        designTimeDataSource = designerView.GetDesignTimeData(5, out dummyDataSource);
                    }
                    catch (Exception exception)
                    {
                        if (base.Component.Site != null)
                        {
                            IComponentDesignerDebugService service = (IComponentDesignerDebugService) base.Component.Site.GetService(typeof(IComponentDesignerDebugService));
                            if (service != null)
                            {
                                service.Fail(System.Design.SR.GetString("DataSource_DebugService_FailedCall", new object[] { "DesignerDataSourceView.GetDesignTimeData", exception.Message }));
                            }
                        }
                        designTimeDataSource = null;
                    }
                }
                bool flag3 = false;
                string dataKeyField = null;
                bool flag4 = false;
                string dataSourceID = null;
                try
                {
                    viewControl.DataSource = designTimeDataSource;
                    dataKeyField = viewControl.DataKeyField;
                    if (dataKeyField.Length != 0)
                    {
                        flag3 = true;
                        viewControl.DataKeyField = string.Empty;
                    }
                    dataSourceID = viewControl.DataSourceID;
                    viewControl.DataSourceID = string.Empty;
                    flag4 = true;
                    viewControl.DataBind();
                    return base.GetDesignTimeHtml();
                }
                catch (Exception exception2)
                {
                    return this.GetErrorDesignTimeHtml(exception2);
                }
                finally
                {
                    viewControl.DataSource = null;
                    if (flag3)
                    {
                        viewControl.DataKeyField = dataKeyField;
                    }
                    if (flag4)
                    {
                        viewControl.DataSourceID = dataSourceID;
                    }
                }
            }
            return this.GetEmptyDesignTimeHtml();
        }

        protected override string GetEmptyDesignTimeHtml()
        {
            string str;
            if (base.CanEnterTemplateMode)
            {
                str = System.Design.SR.GetString("DataList_NoTemplatesInst");
            }
            else
            {
                str = System.Design.SR.GetString("DataList_NoTemplatesInst2");
            }
            return base.CreatePlaceHolderDesignTimeHtml(str);
        }

        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            return base.CreatePlaceHolderDesignTimeHtml(System.Design.SR.GetString("Control_ErrorRendering"));
        }

        [Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
        public override string GetTemplateContainerDataItemProperty(string templateName)
        {
            return "DataItem";
        }

        [Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
        public override string GetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, out bool allowEditing)
        {
            allowEditing = true;
            DataList component = (DataList) base.Component;
            ITemplate itemTemplate = null;
            string textFromTemplate = string.Empty;
            switch (editingFrame.Verb.Index)
            {
                case 0:
                    if (!templateName.Equals(ItemTemplateNames[0]))
                    {
                        if (templateName.Equals(ItemTemplateNames[1]))
                        {
                            itemTemplate = component.AlternatingItemTemplate;
                        }
                        else if (templateName.Equals(ItemTemplateNames[2]))
                        {
                            itemTemplate = component.SelectedItemTemplate;
                        }
                        else if (templateName.Equals(ItemTemplateNames[3]))
                        {
                            itemTemplate = component.EditItemTemplate;
                        }
                        break;
                    }
                    itemTemplate = component.ItemTemplate;
                    break;

                case 1:
                    if (!templateName.Equals(HeaderFooterTemplateNames[0]))
                    {
                        if (templateName.Equals(HeaderFooterTemplateNames[1]))
                        {
                            itemTemplate = component.FooterTemplate;
                        }
                        break;
                    }
                    itemTemplate = component.HeaderTemplate;
                    break;

                case 2:
                    itemTemplate = component.SeparatorTemplate;
                    break;
            }
            if (itemTemplate != null)
            {
                textFromTemplate = base.GetTextFromTemplate(itemTemplate);
            }
            return textFromTemplate;
        }

        public override void Initialize(IComponent component)
        {
            ControlDesigner.VerifyInitializeArgument(component, typeof(DataList));
            base.Initialize(component);
        }

        protected override void OnSchemaRefreshed()
        {
            if (!base.InTemplateModeInternal)
            {
                ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.RefreshSchemaCallback), null, System.Design.SR.GetString("DataList_RefreshSchemaTransaction"));
            }
        }

        protected override void OnTemplateEditingVerbsChanged()
        {
            this.templateVerbsDirty = true;
        }

        private bool RefreshSchemaCallback(object context)
        {
            DataList component = (DataList) base.Component;
            bool flag = (((component.ItemTemplate == null) && (component.EditItemTemplate == null)) && (component.AlternatingItemTemplate == null)) && (component.SelectedItemTemplate == null);
            IDataSourceViewSchema dataSourceSchema = this.GetDataSourceSchema();
            if ((base.DataSourceID.Length > 0) && (dataSourceSchema != null))
            {
                if (flag || (!flag && (DialogResult.Yes == UIServiceHelper.ShowMessage(base.Component.Site, System.Design.SR.GetString("DataList_RegenerateTemplates"), System.Design.SR.GetString("DataList_ClearTemplatesCaption"), MessageBoxButtons.YesNo))))
                {
                    component.ItemTemplate = null;
                    component.EditItemTemplate = null;
                    component.AlternatingItemTemplate = null;
                    component.SelectedItemTemplate = null;
                    component.DataKeyField = string.Empty;
                    this.CreateDefaultTemplate();
                    this.UpdateDesignTimeHtml();
                }
            }
            else if (flag || (!flag && (DialogResult.Yes == UIServiceHelper.ShowMessage(base.Component.Site, System.Design.SR.GetString("DataList_ClearTemplates"), System.Design.SR.GetString("DataList_ClearTemplatesCaption"), MessageBoxButtons.YesNo))))
            {
                component.ItemTemplate = null;
                component.EditItemTemplate = null;
                component.AlternatingItemTemplate = null;
                component.SelectedItemTemplate = null;
                component.DataKeyField = string.Empty;
                this.UpdateDesignTimeHtml();
            }
            return true;
        }

        [Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
        public override void SetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, string templateContent)
        {
            ITemplate templateFromText = null;
            DataList component = (DataList) base.Component;
            if ((templateContent != null) && (templateContent.Length != 0))
            {
                ITemplate currentTemplate = null;
                switch (editingFrame.Verb.Index)
                {
                    case 0:
                        if (!templateName.Equals(ItemTemplateNames[0]))
                        {
                            if (templateName.Equals(ItemTemplateNames[1]))
                            {
                                currentTemplate = component.AlternatingItemTemplate;
                            }
                            else if (templateName.Equals(ItemTemplateNames[2]))
                            {
                                currentTemplate = component.SelectedItemTemplate;
                            }
                            else if (templateName.Equals(ItemTemplateNames[3]))
                            {
                                currentTemplate = component.EditItemTemplate;
                            }
                            break;
                        }
                        currentTemplate = component.ItemTemplate;
                        break;

                    case 1:
                        if (!templateName.Equals(HeaderFooterTemplateNames[0]))
                        {
                            if (templateName.Equals(HeaderFooterTemplateNames[1]))
                            {
                                currentTemplate = component.FooterTemplate;
                            }
                            break;
                        }
                        currentTemplate = component.HeaderTemplate;
                        break;

                    case 2:
                        currentTemplate = component.SeparatorTemplate;
                        break;
                }
                templateFromText = base.GetTemplateFromText(templateContent, currentTemplate);
            }
            switch (editingFrame.Verb.Index)
            {
                case 0:
                    if (!templateName.Equals(ItemTemplateNames[0]))
                    {
                        if (templateName.Equals(ItemTemplateNames[1]))
                        {
                            component.AlternatingItemTemplate = templateFromText;
                            return;
                        }
                        if (templateName.Equals(ItemTemplateNames[2]))
                        {
                            component.SelectedItemTemplate = templateFromText;
                            return;
                        }
                        if (!templateName.Equals(ItemTemplateNames[3]))
                        {
                            break;
                        }
                        component.EditItemTemplate = templateFromText;
                        return;
                    }
                    component.ItemTemplate = templateFromText;
                    return;

                case 1:
                    if (!templateName.Equals(HeaderFooterTemplateNames[0]))
                    {
                        if (!templateName.Equals(HeaderFooterTemplateNames[1]))
                        {
                            break;
                        }
                        component.FooterTemplate = templateFromText;
                        return;
                    }
                    component.HeaderTemplate = templateFromText;
                    return;

                case 2:
                    component.SeparatorTemplate = templateFromText;
                    break;

                default:
                    return;
            }
        }

        public override bool AllowResize
        {
            get
            {
                if (!this.TemplatesExist)
                {
                    return base.InTemplateModeInternal;
                }
                return true;
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
                    _autoFormats = ControlDesigner.CreateAutoFormats(AutoFormatSchemes.BDL_SCHEME_NAMES, CS9__CachedAnonymousMethodDelegate1);
                }
                return _autoFormats;
            }
        }

        protected bool TemplatesExist
        {
            get
            {
                DataList viewControl = (DataList) base.ViewControl;
                ITemplate itemTemplate = viewControl.ItemTemplate;
                string textFromTemplate = null;
                if (itemTemplate == null)
                {
                    return false;
                }
                textFromTemplate = base.GetTextFromTemplate(itemTemplate);
                return ((textFromTemplate != null) && (textFromTemplate.Length > 0));
            }
        }
    }
}

