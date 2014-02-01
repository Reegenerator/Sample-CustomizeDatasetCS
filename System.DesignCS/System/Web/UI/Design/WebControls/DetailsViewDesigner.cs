namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.Collections;
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

    public class DetailsViewDesigner : DataBoundControlDesigner
    {
        private DetailsViewActionList _actionLists;
        private static DesignerAutoFormatCollection _autoFormats;
        private static string[] _controlTemplateNames = new string[] { "FooterTemplate", "HeaderTemplate", "EmptyDataTemplate", "PagerTemplate" };
        private static bool[] _controlTemplateSupportsDataBinding = new bool[] { true, true, true, true };
        private bool _currentDeleteState;
        private bool _currentEditState;
        private bool _currentInsertState;
        internal bool _ignoreSchemaRefreshedEvent;
        private static string[] _rowTemplateNames = new string[] { "ItemTemplate", "AlternatingItemTemplate", "EditItemTemplate", "InsertItemTemplate", "HeaderTemplate" };
        private static bool[] _rowTemplateSupportsDataBinding = new bool[] { true, true, true, true, false };
        private const int BASE_INDEX = 0x3e8;
        private const int IDX_CONTROL_EMPTY_DATA_TEMPLATE = 2;
        private const int IDX_CONTROL_FOOTER_TEMPLATE = 0;
        private const int IDX_CONTROL_HEADER_TEMPLATE = 1;
        private const int IDX_CONTROL_PAGER_TEMPLATE = 3;
        private const int IDX_ROW_ALTITEM_TEMPLATE = 1;
        private const int IDX_ROW_EDITITEM_TEMPLATE = 2;
        private const int IDX_ROW_HEADER_TEMPLATE = 4;
        private const int IDX_ROW_INSERTITEM_TEMPLATE = 3;
        private const int IDX_ROW_ITEM_TEMPLATE = 0;

        private void AddKeysAndBoundFields(IDataSourceViewSchema schema)
        {
            DataControlFieldCollection fields = ((DetailsView) base.Component).Fields;
            if (schema != null)
            {
                IDataSourceFieldSchema[] schemaArray = schema.GetFields();
                if ((schemaArray != null) && (schemaArray.Length > 0))
                {
                    ArrayList list = new ArrayList();
                    foreach (IDataSourceFieldSchema schema2 in schemaArray)
                    {
                        if (DataBinder.IsBindableType(schema2.DataType))
                        {
                            BoundField field;
                            if ((schema2.DataType == typeof(bool)) || (schema2.DataType == typeof(bool?)))
                            {
                                field = new CheckBoxField();
                            }
                            else
                            {
                                field = new BoundField();
                            }
                            string name = schema2.Name;
                            if (schema2.PrimaryKey)
                            {
                                list.Add(name);
                            }
                            field.DataField = name;
                            field.HeaderText = name;
                            field.SortExpression = name;
                            field.ReadOnly = schema2.PrimaryKey || schema2.IsReadOnly;
                            field.InsertVisible = !schema2.Identity;
                            fields.Add(field);
                        }
                    }
                    ((DetailsView) base.Component).AutoGenerateRows = false;
                    int count = list.Count;
                    if (count > 0)
                    {
                        string[] array = new string[count];
                        list.CopyTo(array, 0);
                        ((DetailsView) base.Component).DataKeyNames = array;
                    }
                }
            }
        }

        internal void AddNewField()
        {
            Cursor current = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this._ignoreSchemaRefreshedEvent = true;
                ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.AddNewFieldChangeCallback), null, System.Design.SR.GetString("DetailsView_AddNewFieldTransaction"));
                this._ignoreSchemaRefreshedEvent = false;
                this.UpdateDesignTimeHtml();
            }
            finally
            {
                Cursor.Current = current;
            }
        }

        private bool AddNewFieldChangeCallback(object context)
        {
            if (base.DataSourceDesigner != null)
            {
                base.DataSourceDesigner.SuppressDataSourceEvents();
            }
            AddDataControlFieldDialog form = new AddDataControlFieldDialog(this);
            DialogResult result = UIServiceHelper.ShowDialog(base.Component.Site, form);
            if (base.DataSourceDesigner != null)
            {
                base.DataSourceDesigner.ResumeDataSourceEvents();
            }
            return (result == DialogResult.OK);
        }

        protected override void DataBind(BaseDataBoundControl dataBoundControl)
        {
            base.DataBind(dataBoundControl);
            DetailsView view = (DetailsView) dataBoundControl;
            Table table = view.Controls[0] as Table;
            int autoGeneratedRows = 0;
            int num2 = 1;
            int num3 = 1;
            int num4 = 0;
            if (view.AllowPaging)
            {
                if (view.PagerSettings.Position == PagerPosition.TopAndBottom)
                {
                    num4 = 2;
                }
                else
                {
                    num4 = 1;
                }
            }
            if (view.AutoGenerateRows)
            {
                int num5 = 0;
                if ((view.AutoGenerateInsertButton || view.AutoGenerateDeleteButton) || view.AutoGenerateEditButton)
                {
                    num5 = 1;
                }
                autoGeneratedRows = ((((table.Rows.Count - view.Fields.Count) - num5) - num2) - num3) - num4;
            }
            this.SetRegionAttributes(autoGeneratedRows);
        }

        internal void EditFields()
        {
            Cursor current = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this._ignoreSchemaRefreshedEvent = true;
                ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EditFieldsChangeCallback), null, System.Design.SR.GetString("DetailsView_EditFieldsTransaction"));
                this._ignoreSchemaRefreshedEvent = false;
                this.UpdateDesignTimeHtml();
            }
            finally
            {
                Cursor.Current = current;
            }
        }

        private bool EditFieldsChangeCallback(object context)
        {
            if (base.DataSourceDesigner != null)
            {
                base.DataSourceDesigner.SuppressDataSourceEvents();
            }
            DataControlFieldsEditor form = new DataControlFieldsEditor(this);
            DialogResult result = UIServiceHelper.ShowDialog(base.Component.Site, form);
            if (base.DataSourceDesigner != null)
            {
                base.DataSourceDesigner.ResumeDataSourceEvents();
            }
            return (result == DialogResult.OK);
        }

        private bool EnableDeletingCallback(object context)
        {
            bool newState = !this._currentDeleteState;
            if (context is bool)
            {
                newState = (bool) context;
            }
            this.SaveManipulationSetting(ManipulationMode.Delete, newState);
            return true;
        }

        private bool EnableEditingCallback(object context)
        {
            bool newState = !this._currentEditState;
            if (context is bool)
            {
                newState = (bool) context;
            }
            this.SaveManipulationSetting(ManipulationMode.Edit, newState);
            return true;
        }

        private bool EnableInsertingCallback(object context)
        {
            bool newState = !this._currentInsertState;
            if (context is bool)
            {
                newState = (bool) context;
            }
            this.SaveManipulationSetting(ManipulationMode.Insert, newState);
            return true;
        }

        private bool EnablePagingCallback(object context)
        {
            bool flag2 = !((DetailsView) base.Component).AllowPaging;
            if (context is bool)
            {
                flag2 = (bool) context;
            }
            TypeDescriptor.GetProperties(typeof(DetailsView))["AllowPaging"].SetValue(base.Component, flag2);
            return true;
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
            DetailsView viewControl = (DetailsView) base.ViewControl;
            if (viewControl.Fields.Count == 0)
            {
                viewControl.AutoGenerateRows = true;
            }
            bool flag = false;
            IDataSourceViewSchema dataSourceSchema = this.GetDataSourceSchema();
            if (dataSourceSchema != null)
            {
                IDataSourceFieldSchema[] fields = dataSourceSchema.GetFields();
                if ((fields != null) && (fields.Length > 0))
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                viewControl.DataKeyNames = new string[0];
            }
            TypeDescriptor.Refresh(base.Component);
            return base.GetDesignTimeHtml();
        }

        public override string GetDesignTimeHtml(DesignerRegionCollection regions)
        {
            string designTimeHtml = this.GetDesignTimeHtml();
            DetailsView viewControl = (DetailsView) base.ViewControl;
            int count = viewControl.Rows.Count;
            int selectedFieldIndex = this.SelectedFieldIndex;
            DetailsViewRowCollection rows = viewControl.Rows;
            for (int i = 0; i < viewControl.Fields.Count; i++)
            {
                string name = System.Design.SR.GetString("DetailsView_Field", new object[] { i.ToString(NumberFormatInfo.InvariantInfo) });
                string headerText = viewControl.Fields[i].HeaderText;
                if (headerText.Length == 0)
                {
                    name = name + " - " + headerText;
                }
                if (i < count)
                {
                    DetailsViewRow row = rows[i];
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        TableCell cell1 = row.Cells[j];
                        if (j == 0)
                        {
                            DesignerRegion region = new DesignerRegion(this, name, true) {
                                UserData = i
                            };
                            if (i == selectedFieldIndex)
                            {
                                region.Highlight = true;
                            }
                            regions.Add(region);
                        }
                        else
                        {
                            DesignerRegion region2 = new DesignerRegion(this, i.ToString(NumberFormatInfo.InvariantInfo), false) {
                                UserData = -1
                            };
                            if (i == selectedFieldIndex)
                            {
                                region2.Highlight = true;
                            }
                            regions.Add(region2);
                        }
                    }
                }
            }
            return designTimeHtml;
        }

        public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
        {
            return string.Empty;
        }

        private Style GetTemplateStyle(int templateIndex, TemplateField templateField)
        {
            Style style = new Style();
            style.CopyFrom(((DetailsView) base.ViewControl).ControlStyle);
            switch (templateIndex)
            {
                case 0:
                    style.CopyFrom(((DetailsView) base.ViewControl).FooterStyle);
                    return style;

                case 1:
                    style.CopyFrom(((DetailsView) base.ViewControl).HeaderStyle);
                    return style;

                case 2:
                    style.CopyFrom(((DetailsView) base.ViewControl).EmptyDataRowStyle);
                    return style;

                case 3:
                    style.CopyFrom(((DetailsView) base.ViewControl).PagerStyle);
                    return style;

                case 0x3e8:
                    style.CopyFrom(((DetailsView) base.ViewControl).RowStyle);
                    style.CopyFrom(templateField.ItemStyle);
                    return style;

                case 0x3e9:
                    style.CopyFrom(((DetailsView) base.ViewControl).RowStyle);
                    style.CopyFrom(((DetailsView) base.ViewControl).AlternatingRowStyle);
                    style.CopyFrom(templateField.ItemStyle);
                    return style;

                case 0x3ea:
                    style.CopyFrom(((DetailsView) base.ViewControl).RowStyle);
                    style.CopyFrom(((DetailsView) base.ViewControl).EditRowStyle);
                    style.CopyFrom(templateField.ItemStyle);
                    return style;

                case 0x3eb:
                    style.CopyFrom(((DetailsView) base.ViewControl).RowStyle);
                    style.CopyFrom(((DetailsView) base.ViewControl).InsertRowStyle);
                    style.CopyFrom(templateField.ItemStyle);
                    return style;

                case 0x3ec:
                    style.CopyFrom(((DetailsView) base.ViewControl).HeaderStyle);
                    style.CopyFrom(templateField.HeaderStyle);
                    return style;
            }
            return style;
        }

        public override void Initialize(IComponent component)
        {
            ControlDesigner.VerifyInitializeArgument(component, typeof(DetailsView));
            base.Initialize(component);
            if (base.View != null)
            {
                base.View.SetFlags(ViewFlags.TemplateEditing, true);
            }
        }

        internal void MoveDown()
        {
            Cursor current = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.MoveDownCallback), null, System.Design.SR.GetString("DetailsView_MoveDownTransaction"));
                this.UpdateDesignTimeHtml();
            }
            finally
            {
                Cursor.Current = current;
            }
        }

        private bool MoveDownCallback(object context)
        {
            DataControlFieldCollection fields = ((DetailsView) base.Component).Fields;
            int selectedFieldIndex = this.SelectedFieldIndex;
            if ((selectedFieldIndex >= 0) && (fields.Count > (selectedFieldIndex + 1)))
            {
                DataControlField field = fields[selectedFieldIndex];
                fields.RemoveAt(selectedFieldIndex);
                fields.Insert(selectedFieldIndex + 1, field);
                this.SelectedFieldIndex++;
                this.UpdateDesignTimeHtml();
                return true;
            }
            return false;
        }

        internal void MoveUp()
        {
            Cursor current = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.MoveUpCallback), null, System.Design.SR.GetString("DetailsView_MoveUpTransaction"));
                this.UpdateDesignTimeHtml();
            }
            finally
            {
                Cursor.Current = current;
            }
        }

        private bool MoveUpCallback(object context)
        {
            DataControlFieldCollection fields = ((DetailsView) base.Component).Fields;
            int selectedFieldIndex = this.SelectedFieldIndex;
            if (selectedFieldIndex > 0)
            {
                DataControlField field = fields[selectedFieldIndex];
                fields.RemoveAt(selectedFieldIndex);
                fields.Insert(selectedFieldIndex - 1, field);
                this.SelectedFieldIndex--;
                this.UpdateDesignTimeHtml();
                return true;
            }
            return false;
        }

        protected override void OnClick(DesignerRegionMouseEventArgs e)
        {
            if (e.Region != null)
            {
                this.SelectedFieldIndex = (int) e.Region.UserData;
                this.UpdateDesignTimeHtml();
            }
        }

        protected override void OnSchemaRefreshed()
        {
            if (!base.InTemplateMode && !this._ignoreSchemaRefreshedEvent)
            {
                Cursor current = Cursor.Current;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.SchemaRefreshedCallback), null, System.Design.SR.GetString("DataControls_SchemaRefreshedTransaction"));
                    this.UpdateDesignTimeHtml();
                }
                finally
                {
                    Cursor.Current = current;
                }
            }
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            if (base.InTemplateMode)
            {
                PropertyDescriptor oldPropertyDescriptor = (PropertyDescriptor) properties["Fields"];
                properties["Fields"] = TypeDescriptor.CreateProperty(oldPropertyDescriptor.ComponentType, oldPropertyDescriptor, new Attribute[] { BrowsableAttribute.No });
            }
        }

        private bool RemoveCallback(object context)
        {
            int selectedFieldIndex = this.SelectedFieldIndex;
            if (selectedFieldIndex < 0)
            {
                return false;
            }
            ((DetailsView) base.Component).Fields.RemoveAt(selectedFieldIndex);
            if (selectedFieldIndex == ((DetailsView) base.Component).Fields.Count)
            {
                this.SelectedFieldIndex--;
                this.UpdateDesignTimeHtml();
            }
            return true;
        }

        internal void RemoveField()
        {
            Cursor current = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.RemoveCallback), null, System.Design.SR.GetString("DetailsView_RemoveFieldTransaction"));
                this.UpdateDesignTimeHtml();
            }
            finally
            {
                Cursor.Current = current;
            }
        }

        private void SaveManipulationSetting(ManipulationMode mode, bool newState)
        {
            DataControlFieldCollection fields = ((DetailsView) base.Component).Fields;
            bool flag = false;
            ArrayList list = new ArrayList();
            foreach (DataControlField field in fields)
            {
                CommandField field2 = field as CommandField;
                if (field2 != null)
                {
                    switch (mode)
                    {
                        case ManipulationMode.Edit:
                            field2.ShowEditButton = newState;
                            break;

                        case ManipulationMode.Delete:
                            field2.ShowDeleteButton = newState;
                            break;

                        case ManipulationMode.Insert:
                            field2.ShowInsertButton = newState;
                            break;
                    }
                    if (((!newState && !field2.ShowEditButton) && (!field2.ShowDeleteButton && !field2.ShowInsertButton)) && !field2.ShowSelectButton)
                    {
                        list.Add(field2);
                    }
                    flag = true;
                }
            }
            foreach (object obj2 in list)
            {
                fields.Remove((DataControlField) obj2);
            }
            if (!flag && newState)
            {
                CommandField field3 = new CommandField();
                switch (mode)
                {
                    case ManipulationMode.Edit:
                        field3.ShowEditButton = newState;
                        break;

                    case ManipulationMode.Delete:
                        field3.ShowDeleteButton = newState;
                        break;

                    case ManipulationMode.Insert:
                        field3.ShowInsertButton = newState;
                        break;
                }
                fields.Add(field3);
            }
            if (!newState)
            {
                PropertyDescriptor descriptor;
                DetailsView component = (DetailsView) base.Component;
                switch (mode)
                {
                    case ManipulationMode.Edit:
                        descriptor = TypeDescriptor.GetProperties(typeof(DetailsView))["AutoGenerateEditButton"];
                        descriptor.SetValue(base.Component, newState);
                        return;

                    case ManipulationMode.Delete:
                        descriptor = TypeDescriptor.GetProperties(typeof(DetailsView))["AutoGenerateDeleteButton"];
                        descriptor.SetValue(base.Component, newState);
                        return;

                    case ManipulationMode.Insert:
                        TypeDescriptor.GetProperties(typeof(DetailsView))["AutoGenerateInsertButton"].SetValue(base.Component, newState);
                        break;

                    default:
                        return;
                }
            }
        }

        private bool SchemaRefreshedCallback(object context)
        {
            IDataSourceViewSchema dataSourceSchema = this.GetDataSourceSchema();
            if ((base.DataSourceID.Length > 0) && (dataSourceSchema != null))
            {
                if ((((DetailsView) base.Component).Fields.Count > 0) || (((DetailsView) base.Component).DataKeyNames.Length > 0))
                {
                    if (DialogResult.Yes == UIServiceHelper.ShowMessage(base.Component.Site, System.Design.SR.GetString("DataBoundControl_SchemaRefreshedWarning", new object[] { System.Design.SR.GetString("DataBoundControl_DetailsView"), System.Design.SR.GetString("DataBoundControl_Row") }), System.Design.SR.GetString("DataBoundControl_SchemaRefreshedCaption", new object[] { ((DetailsView) base.Component).ID }), MessageBoxButtons.YesNo))
                    {
                        ((DetailsView) base.Component).DataKeyNames = new string[0];
                        ((DetailsView) base.Component).Fields.Clear();
                        this.SelectedFieldIndex = -1;
                        this.AddKeysAndBoundFields(dataSourceSchema);
                    }
                }
                else
                {
                    this.AddKeysAndBoundFields(dataSourceSchema);
                }
            }
            else if (((((DetailsView) base.Component).Fields.Count > 0) || (((DetailsView) base.Component).DataKeyNames.Length > 0)) && (DialogResult.Yes == UIServiceHelper.ShowMessage(base.Component.Site, System.Design.SR.GetString("DataBoundControl_SchemaRefreshedWarningNoDataSource", new object[] { System.Design.SR.GetString("DataBoundControl_DetailsView"), System.Design.SR.GetString("DataBoundControl_Row") }), System.Design.SR.GetString("DataBoundControl_SchemaRefreshedCaption", new object[] { ((DetailsView) base.Component).ID }), MessageBoxButtons.YesNo)))
            {
                ((DetailsView) base.Component).DataKeyNames = new string[0];
                ((DetailsView) base.Component).Fields.Clear();
                this.SelectedFieldIndex = -1;
            }
            return true;
        }

        public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
        {
        }

        private void SetRegionAttributes(int autoGeneratedRows)
        {
            int num = 0;
            DetailsView viewControl = (DetailsView) base.ViewControl;
            Table table = viewControl.Controls[0] as Table;
            if (table != null)
            {
                int num2 = 0;
                if (viewControl.AllowPaging && (viewControl.PagerSettings.Position != PagerPosition.Bottom))
                {
                    num2 = 1;
                }
                int num3 = (autoGeneratedRows + 1) + num2;
                TableRowCollection rows = table.Rows;
                for (int i = num3; (i < (viewControl.Fields.Count + num3)) && (i < rows.Count); i++)
                {
                    TableRow row = rows[i];
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.Attributes[DesignerRegion.DesignerRegionAttributeName] = num.ToString(NumberFormatInfo.InvariantInfo);
                        num++;
                    }
                }
            }
        }

        private void UpdateFieldsCurrentState()
        {
            this._currentInsertState = ((DetailsView) base.Component).AutoGenerateInsertButton;
            this._currentEditState = ((DetailsView) base.Component).AutoGenerateEditButton;
            this._currentDeleteState = ((DetailsView) base.Component).AutoGenerateDeleteButton;
            foreach (DataControlField field in ((DetailsView) base.Component).Fields)
            {
                CommandField field2 = field as CommandField;
                if (field2 != null)
                {
                    if (field2.ShowInsertButton)
                    {
                        this._currentInsertState = true;
                    }
                    if (field2.ShowEditButton)
                    {
                        this._currentEditState = true;
                    }
                    if (field2.ShowDeleteButton)
                    {
                        this._currentDeleteState = true;
                    }
                }
            }
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                DesignerActionListCollection lists = new DesignerActionListCollection();
                lists.AddRange(base.ActionLists);
                if (this._actionLists == null)
                {
                    this._actionLists = new DetailsViewActionList(this);
                }
                bool inTemplateMode = base.InTemplateMode;
                int selectedFieldIndex = this.SelectedFieldIndex;
                this.UpdateFieldsCurrentState();
                this._actionLists.AllowRemoveField = ((((DetailsView) base.Component).Fields.Count > 0) && (selectedFieldIndex >= 0)) && !inTemplateMode;
                this._actionLists.AllowMoveUp = ((((DetailsView) base.Component).Fields.Count > 0) && (selectedFieldIndex > 0)) && !inTemplateMode;
                this._actionLists.AllowMoveDown = (((((DetailsView) base.Component).Fields.Count > 0) && (selectedFieldIndex >= 0)) && (((DetailsView) base.Component).Fields.Count > (selectedFieldIndex + 1))) && !inTemplateMode;
                DesignerDataSourceView designerView = base.DesignerView;
                this._actionLists.AllowPaging = !inTemplateMode && (designerView != null);
                this._actionLists.AllowInserting = !inTemplateMode && ((designerView != null) && designerView.CanInsert);
                this._actionLists.AllowEditing = !inTemplateMode && ((designerView != null) && designerView.CanUpdate);
                this._actionLists.AllowDeleting = !inTemplateMode && ((designerView != null) && designerView.CanDelete);
                lists.Add(this._actionLists);
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
                    _autoFormats = ControlDesigner.CreateAutoFormats(AutoFormatSchemes.DETAILSVIEW_SCHEME_NAMES, CS9__CachedAnonymousMethodDelegate1);
                }
                return _autoFormats;
            }
        }

        internal bool EnableDeleting
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._currentDeleteState;
            }
            set
            {
                Cursor current = Cursor.Current;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EnableDeletingCallback), value, System.Design.SR.GetString("DetailsView_EnableDeletingTransaction"));
                }
                finally
                {
                    Cursor.Current = current;
                }
            }
        }

        internal bool EnableEditing
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._currentEditState;
            }
            set
            {
                Cursor current = Cursor.Current;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EnableEditingCallback), value, System.Design.SR.GetString("DetailsView_EnableEditingTransaction"));
                }
                finally
                {
                    Cursor.Current = current;
                }
            }
        }

        internal bool EnableInserting
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._currentInsertState;
            }
            set
            {
                Cursor current = Cursor.Current;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EnableInsertingCallback), value, System.Design.SR.GetString("DetailsView_EnableInsertingTransaction"));
                }
                finally
                {
                    Cursor.Current = current;
                }
            }
        }

        internal bool EnablePaging
        {
            get
            {
                return ((DetailsView) base.Component).AllowPaging;
            }
            set
            {
                Cursor current = Cursor.Current;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EnablePagingCallback), value, System.Design.SR.GetString("DetailsView_EnablePagingTransaction"));
                }
                finally
                {
                    Cursor.Current = current;
                }
            }
        }

        protected override int SampleRowCount
        {
            get
            {
                return 2;
            }
        }

        private int SelectedFieldIndex
        {
            get
            {
                object obj2 = base.DesignerState["SelectedFieldIndex"];
                int count = ((DetailsView) base.Component).Fields.Count;
                if (((obj2 != null) && (count != 0)) && ((((int) obj2) >= 0) && (((int) obj2) < count)))
                {
                    return (int) obj2;
                }
                return -1;
            }
            set
            {
                base.DesignerState["SelectedFieldIndex"] = value;
            }
        }

        public override TemplateGroupCollection TemplateGroups
        {
            get
            {
                TemplateGroupCollection templateGroups = base.TemplateGroups;
                DataControlFieldCollection fields = ((DetailsView) base.Component).Fields;
                int count = fields.Count;
                if (count > 0)
                {
                    for (int j = 0; j < count; j++)
                    {
                        TemplateField templateField = fields[j] as TemplateField;
                        if (templateField != null)
                        {
                            string headerText = fields[j].HeaderText;
                            string groupName = System.Design.SR.GetString("DetailsView_Field", new object[] { j.ToString(NumberFormatInfo.InvariantInfo) });
                            if ((headerText != null) && (headerText.Length != 0))
                            {
                                groupName = groupName + " - " + headerText;
                            }
                            TemplateGroup group = new TemplateGroup(groupName);
                            for (int k = 0; k < _rowTemplateNames.Length; k++)
                            {
                                string name = _rowTemplateNames[k];
                                TemplateDefinition templateDefinition = new TemplateDefinition(this, name, fields[j], name, this.GetTemplateStyle(k + 0x3e8, templateField)) {
                                    SupportsDataBinding = _rowTemplateSupportsDataBinding[k]
                                };
                                group.AddTemplateDefinition(templateDefinition);
                            }
                            templateGroups.Add(group);
                        }
                    }
                }
                for (int i = 0; i < _controlTemplateNames.Length; i++)
                {
                    string str4 = _controlTemplateNames[i];
                    TemplateGroup group2 = new TemplateGroup(_controlTemplateNames[i], this.GetTemplateStyle(i, null));
                    TemplateDefinition definition2 = new TemplateDefinition(this, str4, base.Component, str4) {
                        SupportsDataBinding = _controlTemplateSupportsDataBinding[i]
                    };
                    group2.AddTemplateDefinition(definition2);
                    templateGroups.Add(group2);
                }
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

        private enum ManipulationMode
        {
            Edit,
            Delete,
            Insert
        }
    }
}

