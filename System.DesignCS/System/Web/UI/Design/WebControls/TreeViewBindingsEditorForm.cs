﻿namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Design;
    using System.Drawing;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.Design.Util;
    using System.Web.UI.WebControls;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal class TreeViewBindingsEditorForm : DesignerForm
    {
        private System.Windows.Forms.Button _addBindingButton;
        private System.Windows.Forms.Button _applyButton;
        private bool _autoBindInitialized;
        private System.Windows.Forms.CheckBox _autogenerateBindingsCheckBox;
        private System.Windows.Forms.Label _bindingsLabel;
        private System.Windows.Forms.ListBox _bindingsListView;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _deleteBindingButton;
        private System.Windows.Forms.Button _moveBindingDownButton;
        private System.Windows.Forms.Button _moveBindingUpButton;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Label _propertiesLabel;
        private PropertyGrid _propertyGrid;
        private IDataSourceSchema _schema;
        private System.Windows.Forms.Label _schemaLabel;
        private System.Windows.Forms.TreeView _schemaTreeView;
        private System.Web.UI.WebControls.TreeView _treeView;
        private Container components;

        public TreeViewBindingsEditorForm(IServiceProvider serviceProvider, System.Web.UI.WebControls.TreeView treeView, System.Web.UI.Design.WebControls.TreeViewDesigner treeViewDesigner) : base(serviceProvider)
        {
            this._treeView = treeView;
            this._autoBindInitialized = false;
            this.InitializeComponent();
            this.InitializeUI();
            foreach (TreeNodeBinding binding in this._treeView.DataBindings)
            {
                TreeNodeBinding clone = (TreeNodeBinding) ((ICloneable) binding).Clone();
                treeViewDesigner.RegisterClone(binding, clone);
                this._bindingsListView.Items.Add(clone);
            }
        }

        private void AddBinding()
        {
            System.Windows.Forms.TreeNode selectedNode = this._schemaTreeView.SelectedNode;
            if (selectedNode != null)
            {
                TreeNodeBinding binding = new TreeNodeBinding();
                if (selectedNode.Text != this._schemaTreeView.Nodes[0].Text)
                {
                    binding.DataMember = selectedNode.Text;
                    if (((SchemaTreeNode) selectedNode).Duplicate)
                    {
                        binding.Depth = selectedNode.FullPath.Split(new char[] { this._schemaTreeView.PathSeparator[0] }).Length - 1;
                    }
                    ((IDataSourceViewSchemaAccessor) binding).DataSourceViewSchema = ((SchemaTreeNode) selectedNode).Schema;
                    int index = this._bindingsListView.Items.IndexOf(binding);
                    if (index == -1)
                    {
                        this._bindingsListView.Items.Add(binding);
                        this._bindingsListView.SetSelected(this._bindingsListView.Items.Count - 1, true);
                    }
                    else
                    {
                        binding = (TreeNodeBinding) this._bindingsListView.Items[index];
                        this._bindingsListView.SetSelected(index, true);
                    }
                }
                else
                {
                    this._bindingsListView.Items.Add(binding);
                    this._bindingsListView.SetSelected(this._bindingsListView.Items.Count - 1, true);
                }
                this._propertyGrid.SelectedObject = binding;
                this._propertyGrid.Refresh();
                this.UpdateEnabledStates();
            }
            this._bindingsListView.Focus();
        }

        private void ApplyBindings()
        {
            System.Web.UI.Design.ControlDesigner.InvokeTransactedChange(this._treeView, new TransactedChangeCallback(this.ApplyBindingsChangeCallback), null, System.Design.SR.GetString("TreeViewDesigner_EditBindingsTransactionDescription"));
        }

        private bool ApplyBindingsChangeCallback(object context)
        {
            this._treeView.DataBindings.Clear();
            foreach (TreeNodeBinding binding in this._bindingsListView.Items)
            {
                this._treeView.DataBindings.Add(binding);
            }
            TypeDescriptor.GetProperties(this._treeView)["AutoGenerateDataBindings"].SetValue(this._treeView, this._autogenerateBindingsCheckBox.Checked);
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private IDataSourceViewSchema FindViewSchema(string viewName, int level)
        {
            return FindViewSchemaRecursive(this.Schema, 0, viewName, level, null);
        }

        internal static IDataSourceViewSchema FindViewSchemaRecursive(object schema, int depth, string viewName, int level, IDataSourceViewSchema bestBet)
        {
            if (!(schema is IDataSourceSchema) && !(schema is IDataSourceViewSchema))
            {
                return null;
            }
            IDataSourceViewSchema schema2 = schema as IDataSourceViewSchema;
            IDataSourceViewSchema[] schemaArray = (schema2 != null) ? schema2.GetChildren() : ((IDataSourceSchema) schema).GetViews();
            if (schemaArray != null)
            {
                if (depth == level)
                {
                    foreach (IDataSourceViewSchema schema3 in schemaArray)
                    {
                        if (string.Equals(schema3.Name, viewName, StringComparison.OrdinalIgnoreCase))
                        {
                            return schema3;
                        }
                        if (string.IsNullOrEmpty(schema3.Name) && (bestBet == null))
                        {
                            bestBet = schema3;
                        }
                        bestBet = FindViewSchemaRecursive(schema3, depth + 1, viewName, level, bestBet);
                    }
                    return bestBet;
                }
                foreach (IDataSourceViewSchema schema4 in schemaArray)
                {
                    if (((level == -1) && string.Equals(schema4.Name, viewName, StringComparison.OrdinalIgnoreCase)) && (bestBet == null))
                    {
                        return schema4;
                    }
                    bestBet = FindViewSchemaRecursive(schema4, depth + 1, viewName, level, bestBet);
                }
            }
            return bestBet;
        }

        private void InitializeComponent()
        {
            this._schemaLabel = new System.Windows.Forms.Label();
            this._bindingsLabel = new System.Windows.Forms.Label();
            this._bindingsListView = new System.Windows.Forms.ListBox();
            this._addBindingButton = new System.Windows.Forms.Button();
            this._propertiesLabel = new System.Windows.Forms.Label();
            this._cancelButton = new System.Windows.Forms.Button();
            this._propertyGrid = new VsPropertyGrid(base.ServiceProvider);
            this._schemaTreeView = new System.Windows.Forms.TreeView();
            this._moveBindingUpButton = new System.Windows.Forms.Button();
            this._moveBindingDownButton = new System.Windows.Forms.Button();
            this._deleteBindingButton = new System.Windows.Forms.Button();
            this._autogenerateBindingsCheckBox = new System.Windows.Forms.CheckBox();
            this._okButton = new System.Windows.Forms.Button();
            this._applyButton = new System.Windows.Forms.Button();
            base.SuspendLayout();
            this._schemaLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this._schemaLabel.Location = new Point(12, 12);
            this._schemaLabel.Name = "_schemaLabel";
            this._schemaLabel.Size = new Size(0xc4, 14);
            this._schemaLabel.TabIndex = 10;
            this._bindingsLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this._bindingsLabel.Location = new Point(12, 0xba);
            this._bindingsLabel.Name = "_bindingsLabel";
            this._bindingsLabel.Size = new Size(0xc4, 14);
            this._bindingsLabel.TabIndex = 0x19;
            this._bindingsListView.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this._bindingsListView.Location = new Point(12, 0xca);
            this._bindingsListView.Name = "_bindingsListView";
            this._bindingsListView.Size = new Size(0xa4, 0x70);
            this._bindingsListView.TabIndex = 30;
            this._bindingsListView.SelectedIndexChanged += new EventHandler(this.OnBindingsListViewSelectedIndexChanged);
            this._bindingsListView.GotFocus += new EventHandler(this.OnBindingsListViewGotFocus);
            this._addBindingButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._addBindingButton.FlatStyle = FlatStyle.System;
            this._addBindingButton.Location = new Point(0x85, 0x9a);
            this._addBindingButton.Name = "_addBindingButton";
            this._addBindingButton.Size = new Size(0x4b, 0x17);
            this._addBindingButton.TabIndex = 20;
            this._addBindingButton.Click += new EventHandler(this.OnAddBindingButtonClick);
            this._propertiesLabel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._propertiesLabel.Location = new Point(0xe5, 12);
            this._propertiesLabel.Name = "_propertiesLabel";
            this._propertiesLabel.Size = new Size(0x10a, 14);
            this._propertiesLabel.TabIndex = 50;
            this._cancelButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._cancelButton.FlatStyle = FlatStyle.System;
            this._cancelButton.Location = new Point(340, 0x15a);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 0x41;
            this._cancelButton.Click += new EventHandler(this.OnCancelButtonClick);
            this._okButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._okButton.FlatStyle = FlatStyle.System;
            this._okButton.Location = new Point(260, 0x15a);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 60;
            this._okButton.Click += new EventHandler(this.OnOKButtonClick);
            this._applyButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._applyButton.FlatStyle = FlatStyle.System;
            this._applyButton.Location = new Point(420, 0x15a);
            this._applyButton.Name = "_applyButton";
            this._applyButton.TabIndex = 60;
            this._applyButton.Click += new EventHandler(this.OnApplyButtonClick);
            this._applyButton.Enabled = false;
            this._propertyGrid.Anchor = AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            this._propertyGrid.CommandsVisibleIfAvailable = true;
            this._propertyGrid.Cursor = Cursors.HSplit;
            this._propertyGrid.LargeButtons = false;
            this._propertyGrid.LineColor = SystemColors.ScrollBar;
            this._propertyGrid.Location = new Point(0xe5, 0x1c);
            this._propertyGrid.Name = "_propertyGrid";
            this._propertyGrid.Size = new Size(0x10a, 0x135);
            this._propertyGrid.TabIndex = 0x37;
            this._propertyGrid.Text = System.Design.SR.GetString("MenuItemCollectionEditor_PropertyGrid");
            this._propertyGrid.ToolbarVisible = true;
            this._propertyGrid.ViewBackColor = SystemColors.Window;
            this._propertyGrid.ViewForeColor = SystemColors.WindowText;
            this._propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.OnPropertyGridPropertyValueChanged);
            this._propertyGrid.Site = this._treeView.Site;
            this._schemaTreeView.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._schemaTreeView.HideSelection = false;
            this._schemaTreeView.ImageIndex = -1;
            this._schemaTreeView.Location = new Point(12, 0x1c);
            this._schemaTreeView.Name = "_schemaTreeView";
            this._schemaTreeView.SelectedImageIndex = -1;
            this._schemaTreeView.Size = new Size(0xc4, 120);
            this._schemaTreeView.TabIndex = 15;
            this._schemaTreeView.AfterSelect += new TreeViewEventHandler(this.OnSchemaTreeViewAfterSelect);
            this._schemaTreeView.GotFocus += new EventHandler(this.OnSchemaTreeViewGotFocus);
            this._moveBindingUpButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._moveBindingUpButton.Location = new Point(0xb6, 0xca);
            this._moveBindingUpButton.Name = "_moveBindingUpButton";
            this._moveBindingUpButton.Size = new Size(0x1a, 0x17);
            this._moveBindingUpButton.TabIndex = 0x23;
            this._moveBindingUpButton.Click += new EventHandler(this.OnMoveBindingUpButtonClick);
            this._moveBindingDownButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._moveBindingDownButton.Location = new Point(0xb6, 0xe2);
            this._moveBindingDownButton.Name = "_moveBindingDownButton";
            this._moveBindingDownButton.Size = new Size(0x1a, 0x17);
            this._moveBindingDownButton.TabIndex = 40;
            this._moveBindingDownButton.Click += new EventHandler(this.OnMoveBindingDownButtonClick);
            this._deleteBindingButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._deleteBindingButton.Location = new Point(0xb6, 0xff);
            this._deleteBindingButton.Name = "_deleteBindingButton";
            this._deleteBindingButton.Size = new Size(0x1a, 0x17);
            this._deleteBindingButton.TabIndex = 0x2d;
            this._deleteBindingButton.Click += new EventHandler(this.OnDeleteBindingButtonClick);
            this._autogenerateBindingsCheckBox.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this._autogenerateBindingsCheckBox.Location = new Point(12, 320);
            this._autogenerateBindingsCheckBox.Name = "_autogenerateBindingsCheckBox";
            this._autogenerateBindingsCheckBox.Size = new Size(0xc4, 0x12);
            this._autogenerateBindingsCheckBox.TabIndex = 5;
            this._autogenerateBindingsCheckBox.Text = System.Design.SR.GetString("TreeViewBindingsEditor_AutoGenerateBindings");
            this._autogenerateBindingsCheckBox.CheckedChanged += new EventHandler(this.OnAutoGenerateChanged);
            this._autoBindInitialized = false;
            base.AcceptButton = this._okButton;
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(0x1fb, 0x17d);
            base.Controls.AddRange(new System.Windows.Forms.Control[] { this._autogenerateBindingsCheckBox, this._deleteBindingButton, this._moveBindingDownButton, this._moveBindingUpButton, this._okButton, this._cancelButton, this._applyButton, this._propertiesLabel, this._addBindingButton, this._bindingsListView, this._bindingsLabel, this._schemaTreeView, this._schemaLabel, this._propertyGrid });
            this.MinimumSize = new Size(0x1fb, 0x17d);
            base.Name = "TreeViewBindingsEditor";
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.InitializeForm();
            base.ResumeLayout(false);
        }

        private void InitializeUI()
        {
            this._bindingsLabel.Text = System.Design.SR.GetString("TreeViewBindingsEditor_Bindings");
            this._schemaLabel.Text = System.Design.SR.GetString("TreeViewBindingsEditor_Schema");
            this._okButton.Text = System.Design.SR.GetString("TreeViewBindingsEditor_OK");
            this._applyButton.Text = System.Design.SR.GetString("TreeViewBindingsEditor_Apply");
            this._cancelButton.Text = System.Design.SR.GetString("TreeViewBindingsEditor_Cancel");
            this._propertiesLabel.Text = System.Design.SR.GetString("TreeViewBindingsEditor_BindingProperties");
            this._autogenerateBindingsCheckBox.Text = System.Design.SR.GetString("TreeViewBindingsEditor_AutoGenerateBindings");
            this._addBindingButton.Text = System.Design.SR.GetString("TreeViewBindingsEditor_AddBinding");
            this.Text = System.Design.SR.GetString("TreeViewBindingsEditor_Title");
            Bitmap bitmap = System.Drawing.BitmapSelector.CreateIcon(typeof(TreeViewBindingsEditor), "SortUp.ico").ToBitmap();
            bitmap.MakeTransparent();
            this._moveBindingUpButton.Image = bitmap;
            this._moveBindingUpButton.AccessibleName = System.Design.SR.GetString("TreeViewBindingsEditor_MoveBindingUpName");
            this._moveBindingUpButton.AccessibleDescription = System.Design.SR.GetString("TreeViewBindingsEditor_MoveBindingUpDescription");
            Bitmap bitmap2 = System.Drawing.BitmapSelector.CreateIcon(typeof(TreeViewBindingsEditor), "SortDown.ico").ToBitmap();
            bitmap2.MakeTransparent();
            this._moveBindingDownButton.Image = bitmap2;
            this._moveBindingDownButton.AccessibleName = System.Design.SR.GetString("TreeViewBindingsEditor_MoveBindingDownName");
            this._moveBindingDownButton.AccessibleDescription = System.Design.SR.GetString("TreeViewBindingsEditor_MoveBindingDownDescription");
            Bitmap bitmap3 = System.Drawing.BitmapSelector.CreateIcon(typeof(TreeViewBindingsEditor), "Delete.ico").ToBitmap();
            bitmap3.MakeTransparent();
            this._deleteBindingButton.Image = bitmap3;
            this._deleteBindingButton.AccessibleName = System.Design.SR.GetString("TreeViewBindingsEditor_DeleteBindingName");
            this._deleteBindingButton.AccessibleDescription = System.Design.SR.GetString("TreeViewBindingsEditor_DeleteBindingDescription");
            base.Icon = null;
        }

        private void OnAddBindingButtonClick(object sender, EventArgs e)
        {
            this._applyButton.Enabled = true;
            this.AddBinding();
        }

        private void OnApplyButtonClick(object sender, EventArgs e)
        {
            this.ApplyBindings();
            this._applyButton.Enabled = false;
        }

        private void OnAutoGenerateChanged(object sender, EventArgs e)
        {
            if (this._autoBindInitialized)
            {
                this._applyButton.Enabled = true;
            }
        }

        private void OnBindingsListViewGotFocus(object sender, EventArgs e)
        {
            this.UpdateSelectedBinding();
        }

        private void OnBindingsListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateSelectedBinding();
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void OnDeleteBindingButtonClick(object sender, EventArgs e)
        {
            if (this._bindingsListView.SelectedIndices.Count > 0)
            {
                this._applyButton.Enabled = true;
                int index = this._bindingsListView.SelectedIndices[0];
                this._bindingsListView.Items.RemoveAt(index);
                if (index >= this._bindingsListView.Items.Count)
                {
                    index--;
                }
                if ((index >= 0) && (this._bindingsListView.Items.Count > 0))
                {
                    this._bindingsListView.SetSelected(index, true);
                }
            }
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            System.Windows.Forms.TreeNode node = this._schemaTreeView.Nodes.Add(System.Design.SR.GetString("TreeViewBindingsEditor_EmptyBindingText"));
            if (this.Schema != null)
            {
                this.PopulateSchema(this.Schema);
                this._schemaTreeView.ExpandAll();
            }
            this._schemaTreeView.SelectedNode = node;
            this._autoBindInitialized = false;
            this._autogenerateBindingsCheckBox.Checked = this._treeView.AutoGenerateDataBindings;
            this._autoBindInitialized = true;
            this.UpdateEnabledStates();
        }

        private void OnMoveBindingDownButtonClick(object sender, EventArgs e)
        {
            if (this._bindingsListView.SelectedIndices.Count > 0)
            {
                this._applyButton.Enabled = true;
                int index = this._bindingsListView.SelectedIndices[0];
                if ((index + 1) < this._bindingsListView.Items.Count)
                {
                    TreeNodeBinding item = (TreeNodeBinding) this._bindingsListView.Items[index];
                    this._bindingsListView.Items.RemoveAt(index);
                    this._bindingsListView.Items.Insert(index + 1, item);
                    this._bindingsListView.SetSelected(index + 1, true);
                }
            }
        }

        private void OnMoveBindingUpButtonClick(object sender, EventArgs e)
        {
            if (this._bindingsListView.SelectedIndices.Count > 0)
            {
                this._applyButton.Enabled = true;
                int index = this._bindingsListView.SelectedIndices[0];
                if (index > 0)
                {
                    TreeNodeBinding item = (TreeNodeBinding) this._bindingsListView.Items[index];
                    this._bindingsListView.Items.RemoveAt(index);
                    this._bindingsListView.Items.Insert(index - 1, item);
                    this._bindingsListView.SetSelected(index - 1, true);
                }
            }
        }

        private void OnOKButtonClick(object sender, EventArgs e)
        {
            this.ApplyBindings();
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void OnPropertyGridPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            this._applyButton.Enabled = true;
            if (e.ChangedItem.PropertyDescriptor.Name == "DataMember")
            {
                string viewName = (string) e.ChangedItem.Value;
                TreeNodeBinding binding = (TreeNodeBinding) this._bindingsListView.Items[this._bindingsListView.SelectedIndex];
                this._bindingsListView.Items[this._bindingsListView.SelectedIndex] = binding;
                this._bindingsListView.Refresh();
                IDataSourceViewSchema schema = this.FindViewSchema(viewName, binding.Depth);
                if (schema != null)
                {
                    ((IDataSourceViewSchemaAccessor) binding).DataSourceViewSchema = schema;
                }
                this._propertyGrid.SelectedObject = binding;
                this._propertyGrid.Refresh();
            }
            else if (e.ChangedItem.PropertyDescriptor.Name == "Depth")
            {
                int level = (int) e.ChangedItem.Value;
                TreeNodeBinding binding2 = (TreeNodeBinding) this._bindingsListView.Items[this._bindingsListView.SelectedIndex];
                IDataSourceViewSchema schema2 = this.FindViewSchema(binding2.DataMember, level);
                if (schema2 != null)
                {
                    ((IDataSourceViewSchemaAccessor) binding2).DataSourceViewSchema = schema2;
                }
                this._propertyGrid.SelectedObject = binding2;
                this._propertyGrid.Refresh();
            }
        }

        private void OnSchemaTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            this.UpdateEnabledStates();
        }

        private void OnSchemaTreeViewGotFocus(object sender, EventArgs e)
        {
            this._propertyGrid.SelectedObject = null;
        }

        private void PopulateSchema(IDataSourceSchema schema)
        {
            if (schema != null)
            {
                IDictionary duplicates = new Hashtable();
                IDataSourceViewSchema[] views = schema.GetViews();
                if (views != null)
                {
                    for (int i = 0; i < views.Length; i++)
                    {
                        this.PopulateSchemaRecursive(this._schemaTreeView.Nodes, views[i], 0, duplicates);
                    }
                }
            }
        }

        private void PopulateSchemaRecursive(System.Windows.Forms.TreeNodeCollection nodes, IDataSourceViewSchema viewSchema, int depth, IDictionary duplicates)
        {
            if (viewSchema != null)
            {
                SchemaTreeNode node = new SchemaTreeNode(viewSchema);
                nodes.Add(node);
                SchemaTreeNode node2 = (SchemaTreeNode) duplicates[viewSchema.Name];
                if (node2 != null)
                {
                    node2.Duplicate = true;
                    node.Duplicate = true;
                }
                foreach (TreeNodeBinding binding in this._bindingsListView.Items)
                {
                    if (string.Compare(binding.DataMember, viewSchema.Name, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        IDataSourceViewSchemaAccessor accessor = binding;
                        if ((depth == binding.Depth) || (accessor.DataSourceViewSchema == null))
                        {
                            accessor.DataSourceViewSchema = viewSchema;
                        }
                    }
                }
                IDataSourceViewSchema[] children = viewSchema.GetChildren();
                if (children != null)
                {
                    for (int i = 0; i < children.Length; i++)
                    {
                        this.PopulateSchemaRecursive(node.Nodes, children[i], depth + 1, duplicates);
                    }
                }
            }
        }

        private void UpdateEnabledStates()
        {
            if (this._bindingsListView.SelectedIndices.Count > 0)
            {
                int num = this._bindingsListView.SelectedIndices[0];
                this._moveBindingDownButton.Enabled = (num + 1) < this._bindingsListView.Items.Count;
                this._moveBindingUpButton.Enabled = num > 0;
                this._deleteBindingButton.Enabled = true;
            }
            else
            {
                this._moveBindingDownButton.Enabled = false;
                this._moveBindingUpButton.Enabled = false;
                this._deleteBindingButton.Enabled = false;
            }
            this._addBindingButton.Enabled = this._schemaTreeView.SelectedNode != null;
        }

        private void UpdateSelectedBinding()
        {
            TreeNodeBinding binding = null;
            if (this._bindingsListView.SelectedItems.Count > 0)
            {
                binding = (TreeNodeBinding) this._bindingsListView.SelectedItems[0];
            }
            this._propertyGrid.SelectedObject = binding;
            this._propertyGrid.Refresh();
            this.UpdateEnabledStates();
        }

        protected override string HelpTopic
        {
            get
            {
                return "net.Asp.TreeView.BindingsEditorForm";
            }
        }

        private IDataSourceSchema Schema
        {
            get
            {
                if (this._schema == null)
                {
                    IDesignerHost host = (IDesignerHost) base.ServiceProvider.GetService(typeof(IDesignerHost));
                    if (host != null)
                    {
                        HierarchicalDataBoundControlDesigner designer = host.GetDesigner(this._treeView) as HierarchicalDataBoundControlDesigner;
                        if (designer != null)
                        {
                            DesignerHierarchicalDataSourceView designerView = designer.DesignerView;
                            if (designerView != null)
                            {
                                try
                                {
                                    this._schema = designerView.Schema;
                                }
                                catch (Exception exception)
                                {
                                    IComponentDesignerDebugService service = (IComponentDesignerDebugService) base.ServiceProvider.GetService(typeof(IComponentDesignerDebugService));
                                    if (service != null)
                                    {
                                        service.Fail(System.Design.SR.GetString("DataSource_DebugService_FailedCall", new object[] { "DesignerHierarchicalDataSourceView.Schema", exception.Message }));
                                    }
                                }
                            }
                        }
                    }
                }
                return this._schema;
            }
        }

        private class SchemaTreeNode : System.Windows.Forms.TreeNode
        {
            private bool _duplicate;
            private IDataSourceViewSchema _schema;

            public SchemaTreeNode(IDataSourceViewSchema schema) : base(schema.Name)
            {
                this._schema = schema;
            }

            public bool Duplicate
            {
                get
                {
                    return this._duplicate;
                }
                set
                {
                    this._duplicate = value;
                }
            }

            public object Schema
            {
                get
                {
                    return this._schema;
                }
            }
        }
    }
}

