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
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;

    [SupportsPreviewControl(true), SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.UnmanagedCode)]
    public class DataGridDesigner : BaseDataListDesigner
    {
        private static DesignerAutoFormatCollection _autoFormats;
        private static string[] ColumnTemplateNames = new string[] { "ItemTemplate", "EditItemTemplate", "HeaderTemplate", "FooterTemplate" };
        internal static TraceSwitch DataGridDesignerSwitch = new TraceSwitch("DATAGRIDDESIGNER", "Enable DataGrid designer general purpose traces.");
        private const int IDX_EDITITEM_TEMPLATE = 1;
        private const int IDX_FOOTER_TEMPLATE = 3;
        private const int IDX_HEADER_TEMPLATE = 2;
        private const int IDX_ITEM_TEMPLATE = 0;
        private TemplateEditingVerb[] templateVerbs;
        private bool templateVerbsDirty = true;

        [Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
        protected override ITemplateEditingFrame CreateTemplateEditingFrame(TemplateEditingVerb verb)
        {
            ITemplateEditingService service = (ITemplateEditingService) this.GetService(typeof(ITemplateEditingService));
            DataGrid viewControl = (DataGrid) base.ViewControl;
            Style[] templateStyles = new Style[] { viewControl.ItemStyle, viewControl.EditItemStyle, viewControl.HeaderStyle, viewControl.FooterStyle };
            return service.CreateFrame(this, verb.Text, ColumnTemplateNames, viewControl.ControlStyle, templateStyles);
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
                DataGridColumnCollection columns = ((DataGrid) base.Component).Columns;
                int count = columns.Count;
                if (count > 0)
                {
                    int num3;
                    int num2 = 0;
                    for (num3 = 0; num3 < count; num3++)
                    {
                        if (columns[num3] is TemplateColumn)
                        {
                            num2++;
                        }
                    }
                    if (num2 > 0)
                    {
                        this.templateVerbs = new TemplateEditingVerb[num2];
                        num3 = 0;
                        int index = 0;
                        while (num3 < count)
                        {
                            if (columns[num3] is TemplateColumn)
                            {
                                string headerText = columns[num3].HeaderText;
                                string text = "Columns[" + num3.ToString(NumberFormatInfo.CurrentInfo) + "]";
                                if ((headerText != null) && (headerText.Length != 0))
                                {
                                    text = text + " - " + headerText;
                                }
                                this.templateVerbs[index] = new TemplateEditingVerb(text, num3, this);
                                index++;
                            }
                            num3++;
                        }
                    }
                }
                this.templateVerbsDirty = false;
            }
            return this.templateVerbs;
        }

        public override string GetDesignTimeHtml()
        {
            int minimumRows = 5;
            DataGrid viewControl = (DataGrid) base.ViewControl;
            if (viewControl.AllowPaging && (viewControl.PageSize != 0))
            {
                minimumRows = Math.Min(viewControl.PageSize, 100) + 1;
            }
            bool dummyDataSource = false;
            IEnumerable designTimeDataSource = null;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            DesignerDataSourceView designerView = base.DesignerView;
            bool autoGenerateColumns = viewControl.AutoGenerateColumns;
            string dataKeyField = string.Empty;
            string dataSourceID = string.Empty;
            string designTimeHtml = null;
            if (designerView == null)
            {
                designTimeDataSource = base.GetDesignTimeDataSource(minimumRows, out dummyDataSource);
            }
            else
            {
                try
                {
                    designTimeDataSource = designerView.GetDesignTimeData(minimumRows, out dummyDataSource);
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
                }
                if (designTimeDataSource == null)
                {
                    return this.GetEmptyDesignTimeHtml();
                }
            }
            if (!autoGenerateColumns && (viewControl.Columns.Count == 0))
            {
                flag2 = true;
                viewControl.AutoGenerateColumns = true;
            }
            if (dummyDataSource)
            {
                dataKeyField = viewControl.DataKeyField;
                if (dataKeyField.Length != 0)
                {
                    flag3 = true;
                    viewControl.DataKeyField = string.Empty;
                }
            }
            try
            {
                viewControl.DataSource = designTimeDataSource;
                dataSourceID = viewControl.DataSourceID;
                viewControl.DataSourceID = string.Empty;
                flag4 = true;
                viewControl.DataBind();
                designTimeHtml = base.GetDesignTimeHtml();
            }
            catch (Exception exception2)
            {
                designTimeHtml = this.GetErrorDesignTimeHtml(exception2);
            }
            finally
            {
                viewControl.DataSource = null;
                if (flag2)
                {
                    viewControl.AutoGenerateColumns = false;
                }
                if (flag3)
                {
                    viewControl.DataKeyField = dataKeyField;
                }
                if (flag4)
                {
                    viewControl.DataSourceID = dataSourceID;
                }
            }
            return designTimeHtml;
        }

        protected override string GetEmptyDesignTimeHtml()
        {
            return base.CreatePlaceHolderDesignTimeHtml(null);
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
            DataGrid component = (DataGrid) base.Component;
            int index = editingFrame.Verb.Index;
            TemplateColumn column = (TemplateColumn) component.Columns[index];
            ITemplate headerTemplate = null;
            string textFromTemplate = string.Empty;
            if (templateName.Equals(ColumnTemplateNames[2]))
            {
                headerTemplate = column.HeaderTemplate;
            }
            else if (templateName.Equals(ColumnTemplateNames[0]))
            {
                headerTemplate = column.ItemTemplate;
            }
            else if (templateName.Equals(ColumnTemplateNames[1]))
            {
                headerTemplate = column.EditItemTemplate;
            }
            else if (templateName.Equals(ColumnTemplateNames[3]))
            {
                headerTemplate = column.FooterTemplate;
            }
            if (headerTemplate != null)
            {
                textFromTemplate = base.GetTextFromTemplate(headerTemplate);
            }
            return textFromTemplate;
        }

        [Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
        public override Type GetTemplatePropertyParentType(string templateName)
        {
            return typeof(TemplateColumn);
        }

        public override void Initialize(IComponent component)
        {
            ControlDesigner.VerifyInitializeArgument(component, typeof(DataGrid));
            base.Initialize(component);
        }

        public virtual void OnColumnsChanged()
        {
            this.OnTemplateEditingVerbsChanged();
        }

        protected override void OnTemplateEditingVerbsChanged()
        {
            this.templateVerbsDirty = true;
        }

        [Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
        public override void SetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, string templateContent)
        {
            int index = editingFrame.Verb.Index;
            DataGrid component = (DataGrid) base.Component;
            TemplateColumn column = (TemplateColumn) component.Columns[index];
            ITemplate templateFromText = null;
            if ((templateContent != null) && (templateContent.Length != 0))
            {
                ITemplate currentTemplate = null;
                if (templateName.Equals(ColumnTemplateNames[2]))
                {
                    currentTemplate = column.HeaderTemplate;
                }
                else if (templateName.Equals(ColumnTemplateNames[0]))
                {
                    currentTemplate = column.ItemTemplate;
                }
                else if (templateName.Equals(ColumnTemplateNames[1]))
                {
                    currentTemplate = column.EditItemTemplate;
                }
                else if (templateName.Equals(ColumnTemplateNames[3]))
                {
                    currentTemplate = column.FooterTemplate;
                }
                templateFromText = base.GetTemplateFromText(templateContent, currentTemplate);
            }
            if (templateName.Equals(ColumnTemplateNames[2]))
            {
                column.HeaderTemplate = templateFromText;
            }
            else if (templateName.Equals(ColumnTemplateNames[0]))
            {
                column.ItemTemplate = templateFromText;
            }
            else if (templateName.Equals(ColumnTemplateNames[1]))
            {
                column.EditItemTemplate = templateFromText;
            }
            else if (templateName.Equals(ColumnTemplateNames[3]))
            {
                column.FooterTemplate = templateFromText;
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
    }
}

