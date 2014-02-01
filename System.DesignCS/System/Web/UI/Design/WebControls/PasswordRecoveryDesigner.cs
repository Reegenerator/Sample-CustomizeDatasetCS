﻿namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Design;
    using System.Security.Permissions;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;
    using System.Windows.Forms;

    [SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.UnmanagedCode)]
    public class PasswordRecoveryDesigner : ControlDesigner
    {
        private static DesignerAutoFormatCollection _autoFormats;
        private const string _failureTextID = "FailureText";
        private static readonly string[] _nonTemplateProperties = new string[] { 
            "AnswerLabelText", "AnswerRequiredErrorMessage", "BorderPadding", "HelpPageIconUrl", "FailureTextStyle", "HelpPageText", "HelpPageUrl", "HyperLinkStyle", "InstructionTextStyle", "LabelStyle", "QuestionInstructionText", "QuestionLabelText", "QuestionTitleText", "SubmitButtonImageUrl", "SubmitButtonStyle", "SubmitButtonText", 
            "SubmitButtonType", "SuccessText", "SuccessTextStyle", "TextBoxStyle", "TextLayout", "TitleTextStyle", "UserNameInstructionText", "UserNameLabelText", "UserNameRequiredErrorMessage", "UserNameTitleText", "ValidatorTextStyle"
         };
        private PasswordRecovery _passwordRecovery;
        private static readonly string[] _questionViewRegionToPropertyMap = new string[] { "UserNameLabelText", "QuestionTitleText", "QuestionLabelText", "QuestionInstructionText", "AnswerLabelText" };
        private static readonly string[] _successViewRegionToPropertyMap = new string[] { "SuccessText" };
        private static readonly string[] _templateNames = new string[] { "UserNameTemplate", "QuestionTemplate", "SuccessTemplate" };
        private static readonly string[] _userNameViewRegionToPropertyMap = new string[] { "UserNameLabelText", "UserNameTitleText", "UserNameInstructionText" };

        private void ConvertToTemplate()
        {
            ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConvertToTemplateChangeCallback), null, System.Design.SR.GetString("WebControls_ConvertToTemplate"), this.TemplateDescriptor);
        }

        private bool ConvertToTemplateChangeCallback(object context)
        {
            try
            {
                IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
                ITemplate template = new ConvertToTemplateHelper(this, service).ConvertToTemplate();
                this.TemplateDescriptor.SetValue(this._passwordRecovery, template);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override string GetDesignTimeHtml()
        {
            try
            {
                IDictionary data = new HybridDictionary(1);
                data["CurrentView"] = this.CurrentView;
                ((IControlDesignerAccessor) base.ViewControl).SetDesignModeState(data);
                ((ICompositeControlDesignerAccessor) base.ViewControl).RecreateChildControls();
                return base.GetDesignTimeHtml();
            }
            catch (Exception exception)
            {
                return this.GetErrorDesignTimeHtml(exception);
            }
        }

        public override string GetDesignTimeHtml(DesignerRegionCollection regions)
        {
            if (base.UseRegions(regions, this.GetTemplate(this._passwordRecovery)))
            {
                EditableDesignerRegion region = new TemplatedEditableDesignerRegion(this.TemplateDefinition) {
                    Description = System.Design.SR.GetString("ContainerControlDesigner_RegionWatermark")
                };
                regions.Add(region);
                ((WebControl) base.ViewControl).Enabled = true;
                IDictionary data = new HybridDictionary(1);
                data.Add("RegionEditing", true);
                ((IControlDesignerAccessor) base.ViewControl).SetDesignModeState(data);
            }
            return this.GetDesignTimeHtml();
        }

        public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
        {
            ITemplate template = this.GetTemplate(this._passwordRecovery);
            if (template == null)
            {
                return string.Empty;
            }
            IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            return ControlPersister.PersistTemplate(template, service);
        }

        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            return base.CreatePlaceHolderDesignTimeHtml(System.Design.SR.GetString("Control_ErrorRenderingShort") + "<br />" + e.Message);
        }

        private ITemplate GetTemplate(PasswordRecovery passwordRecovery)
        {
            switch (this.CurrentView)
            {
                case ViewType.UserName:
                    return passwordRecovery.UserNameTemplate;

                case ViewType.Question:
                    return passwordRecovery.QuestionTemplate;

                case ViewType.Success:
                    return passwordRecovery.SuccessTemplate;
            }
            return null;
        }

        public override void Initialize(IComponent component)
        {
            ControlDesigner.VerifyInitializeArgument(component, typeof(PasswordRecovery));
            this._passwordRecovery = (PasswordRecovery) component;
            base.Initialize(component);
        }

        private void LaunchWebAdmin()
        {
            IDesignerHost host = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            if (host != null)
            {
                IWebAdministrationService service = (IWebAdministrationService) host.GetService(typeof(IWebAdministrationService));
                if (service != null)
                {
                    service.Start(null);
                }
            }
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            if (this.Templated)
            {
                foreach (string str in _nonTemplateProperties)
                {
                    PropertyDescriptor oldPropertyDescriptor = (PropertyDescriptor) properties[str];
                    if (oldPropertyDescriptor != null)
                    {
                        properties[str] = TypeDescriptor.CreateProperty(oldPropertyDescriptor.ComponentType, oldPropertyDescriptor, new Attribute[] { BrowsableAttribute.No });
                    }
                }
            }
            RenderOuterTableHelper.SetupRenderOuterTable(properties, base.Component, false, base.GetType());
        }

        private void Reset()
        {
            this.UpdateDesignTimeHtml();
            ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ResetChangeCallback), null, System.Design.SR.GetString("WebControls_Reset"), this.TemplateDescriptor);
        }

        private bool ResetChangeCallback(object context)
        {
            try
            {
                this.TemplateDescriptor.SetValue(this._passwordRecovery, null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
        {
            IDesignerHost service = (IDesignerHost) base.Component.Site.GetService(typeof(IDesignerHost));
            ITemplate template = ControlParser.ParseTemplate(service, content);
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(base.Component)[region.Name];
            using (DesignerTransaction transaction = service.CreateTransaction("SetEditableDesignerRegionContent"))
            {
                descriptor.SetValue(base.Component, template);
                transaction.Commit();
            }
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                DesignerActionListCollection lists = new DesignerActionListCollection();
                lists.AddRange(base.ActionLists);
                lists.Add(new PasswordRecoveryDesignerActionList(this));
                return lists;
            }
        }

        public override bool AllowResize
        {
            get
            {
                if (!this.RenderOuterTable)
                {
                    return false;
                }
                return base.AllowResize;
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
                    _autoFormats = ControlDesigner.CreateAutoFormats(AutoFormatSchemes.PASSWORDRECOVERY_SCHEME_NAMES, CS9__CachedAnonymousMethodDelegate1);
                }
                return _autoFormats;
            }
        }

        private ViewType CurrentView
        {
            get
            {
                object obj2 = base.DesignerState["CurrentView"];
                if (obj2 != null)
                {
                    return (ViewType) obj2;
                }
                return ViewType.UserName;
            }
            set
            {
                base.DesignerState["CurrentView"] = value;
            }
        }

        public bool RenderOuterTable
        {
            get
            {
                return ((PasswordRecovery) base.Component).RenderOuterTable;
            }
            set
            {
                RenderOuterTableHelper.SetRenderOuterTable(value, this, false);
            }
        }

        private bool Templated
        {
            get
            {
                return (this.GetTemplate(this._passwordRecovery) != null);
            }
        }

        private System.Web.UI.Design.TemplateDefinition TemplateDefinition
        {
            get
            {
                string name = _templateNames[(int) this.CurrentView];
                return new System.Web.UI.Design.TemplateDefinition(this, name, this._passwordRecovery, name, ((WebControl) base.ViewControl).ControlStyle);
            }
        }

        private PropertyDescriptor TemplateDescriptor
        {
            get
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Component);
                string name = _templateNames[(int) this.CurrentView];
                return properties.Find(name, false);
            }
        }

        public override TemplateGroupCollection TemplateGroups
        {
            get
            {
                TemplateGroupCollection templateGroups = base.TemplateGroups;
                TemplateGroupCollection groups = new TemplateGroupCollection();
                for (int i = 0; i < _templateNames.Length; i++)
                {
                    string groupName = _templateNames[i];
                    TemplateGroup group = new TemplateGroup(groupName, ((WebControl) base.ViewControl).ControlStyle);
                    group.AddTemplateDefinition(new System.Web.UI.Design.TemplateDefinition(this, groupName, this._passwordRecovery, groupName, ((WebControl) base.ViewControl).ControlStyle));
                    groups.Add(group);
                }
                templateGroups.AddRange(groups);
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

        private sealed class ConvertToTemplateHelper : LoginDesignerUtil.GenericConvertToTemplateHelper<PasswordRecovery, PasswordRecoveryDesigner>
        {
            private static readonly string[] _persistedControlIDs = new string[] { "UserName", "UserNameRequired", "Question", "Answer", "AnswerRequired", "SubmitButton", "SubmitImageButton", "SubmitLinkButton", "FailureText", "HelpLink" };
            private static readonly string[] _persistedIfNotVisibleControlIDs = new string[] { "UserName", "Question", "FailureText" };

            public ConvertToTemplateHelper(PasswordRecoveryDesigner designer, IDesignerHost designerHost) : base(designer, designerHost)
            {
            }

            protected override System.Web.UI.Control GetDefaultTemplateContents()
            {
                System.Web.UI.Control control = null;
                switch (base.Designer.CurrentView)
                {
                    case PasswordRecoveryDesigner.ViewType.UserName:
                        control = base.Designer.ViewControl.Controls[0];
                        break;

                    case PasswordRecoveryDesigner.ViewType.Question:
                        control = base.Designer.ViewControl.Controls[1];
                        break;

                    case PasswordRecoveryDesigner.ViewType.Success:
                        control = base.Designer.ViewControl.Controls[2];
                        break;
                }
                return (Table) control.Controls[0];
            }

            protected override Style GetFailureTextStyle(PasswordRecovery control)
            {
                return control.FailureTextStyle;
            }

            protected override ITemplate GetTemplate(PasswordRecovery control)
            {
                return base.Designer.GetTemplate(control);
            }

            protected override string[] PersistedControlIDs
            {
                get
                {
                    return _persistedControlIDs;
                }
            }

            protected override string[] PersistedIfNotVisibleControlIDs
            {
                get
                {
                    return _persistedIfNotVisibleControlIDs;
                }
            }
        }

        private class PasswordRecoveryDesignerActionList : DesignerActionList
        {
            private PasswordRecoveryDesigner _designer;

            public PasswordRecoveryDesignerActionList(PasswordRecoveryDesigner designer) : base(designer.Component)
            {
                this._designer = designer;
            }

            public void ConvertToTemplate()
            {
                Cursor current = Cursor.Current;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    this._designer.ConvertToTemplate();
                }
                finally
                {
                    Cursor.Current = current;
                }
            }

            public override DesignerActionItemCollection GetSortedActionItems()
            {
                DesignerActionItemCollection items = new DesignerActionItemCollection();
                DesignerActionPropertyItem item = new DesignerActionPropertyItem("View", System.Design.SR.GetString("WebControls_Views"), string.Empty, System.Design.SR.GetString("WebControls_ViewsDescription")) {
                    ShowInSourceView = false
                };
                items.Add(item);
                if (!this._designer.InTemplateMode)
                {
                    if (this._designer.Templated)
                    {
                        items.Add(new DesignerActionMethodItem(this, "Reset", System.Design.SR.GetString("WebControls_Reset"), string.Empty, System.Design.SR.GetString("WebControls_ResetDescriptionViews"), true));
                    }
                    else
                    {
                        items.Add(new DesignerActionMethodItem(this, "ConvertToTemplate", System.Design.SR.GetString("WebControls_ConvertToTemplate"), string.Empty, System.Design.SR.GetString("WebControls_ConvertToTemplateDescriptionViews"), true));
                    }
                }
                items.Add(new DesignerActionMethodItem(this, "LaunchWebAdmin", System.Design.SR.GetString("Login_LaunchWebAdmin"), string.Empty, System.Design.SR.GetString("Login_LaunchWebAdminDescription"), true));
                return items;
            }

            public void LaunchWebAdmin()
            {
                this._designer.LaunchWebAdmin();
            }

            public void Reset()
            {
                this._designer.Reset();
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

            [TypeConverter(typeof(PasswordRecoveryViewTypeConverter))]
            public string View
            {
                get
                {
                    if (this._designer.CurrentView == PasswordRecoveryDesigner.ViewType.UserName)
                    {
                        return System.Design.SR.GetString("PasswordRecovery_UserNameView");
                    }
                    if (this._designer.CurrentView == PasswordRecoveryDesigner.ViewType.Question)
                    {
                        return System.Design.SR.GetString("PasswordRecovery_QuestionView");
                    }
                    if (this._designer.CurrentView == PasswordRecoveryDesigner.ViewType.Success)
                    {
                        return System.Design.SR.GetString("PasswordRecovery_SuccessView");
                    }
                    return string.Empty;
                }
                set
                {
                    if (string.Compare(value, System.Design.SR.GetString("PasswordRecovery_UserNameView"), StringComparison.Ordinal) == 0)
                    {
                        this._designer.CurrentView = PasswordRecoveryDesigner.ViewType.UserName;
                    }
                    else if (string.Compare(value, System.Design.SR.GetString("PasswordRecovery_QuestionView"), StringComparison.Ordinal) == 0)
                    {
                        this._designer.CurrentView = PasswordRecoveryDesigner.ViewType.Question;
                    }
                    else if (string.Compare(value, System.Design.SR.GetString("PasswordRecovery_SuccessView"), StringComparison.Ordinal) == 0)
                    {
                        this._designer.CurrentView = PasswordRecoveryDesigner.ViewType.Success;
                    }
                    TypeDescriptor.Refresh(this._designer.Component);
                    this._designer.UpdateDesignTimeHtml();
                }
            }

            private class PasswordRecoveryViewTypeConverter : TypeConverter
            {
                public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
                {
                    return new TypeConverter.StandardValuesCollection(new string[] { System.Design.SR.GetString("PasswordRecovery_UserNameView"), System.Design.SR.GetString("PasswordRecovery_QuestionView"), System.Design.SR.GetString("PasswordRecovery_SuccessView") });
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

        private enum ViewType
        {
            UserName,
            Question,
            Success
        }
    }
}

