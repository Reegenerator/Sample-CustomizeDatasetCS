namespace System.Web.UI.Design.WebControls
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
    public class LoginDesigner : CompositeControlDesigner
    {
        private static DesignerAutoFormatCollection _autoFormats;
        private const string _failureTextID = "FailureText";
        private Login _login;
        private static readonly string[] _nonTemplateProperties = new string[] { 
            "BorderPadding", "CheckBoxStyle", "CreateUserIconUrl", "CreateUserText", "CreateUserUrl", "DisplayRememberMe", "FailureTextStyle", "HelpPageIconUrl", "HelpPageText", "HelpPageUrl", "HyperLinkStyle", "InstructionText", "InstructionTextStyle", "LabelStyle", "Orientation", "PasswordLabelText", 
            "PasswordRecoveryIconUrl", "PasswordRecoveryText", "PasswordRecoveryUrl", "PasswordRequiredErrorMessage", "RememberMeText", "LoginButtonImageUrl", "LoginButtonStyle", "LoginButtonText", "LoginButtonType", "TextBoxStyle", "TextLayout", "TitleText", "TitleTextStyle", "UserNameLabelText", "UserNameRequiredErrorMessage", "ValidatorTextStyle"
         };
        private const string _templateName = "LayoutTemplate";

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
                this.TemplateDescriptor.SetValue(this._login, template);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override string GetDesignTimeHtml(DesignerRegionCollection regions)
        {
            if (base.UseRegions(regions, this._login.LayoutTemplate))
            {
                ((WebControl) base.ViewControl).Enabled = true;
                IDictionary data = new HybridDictionary(1);
                data.Add("RegionEditing", true);
                ((IControlDesignerAccessor) base.ViewControl).SetDesignModeState(data);
                EditableDesignerRegion region = new TemplatedEditableDesignerRegion(this.TemplateDefinition) {
                    Description = System.Design.SR.GetString("ContainerControlDesigner_RegionWatermark")
                };
                regions.Add(region);
            }
            return this.GetDesignTimeHtml();
        }

        public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
        {
            IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            return ControlPersister.PersistTemplate(this._login.LayoutTemplate, service);
        }

        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            return base.CreatePlaceHolderDesignTimeHtml(System.Design.SR.GetString("Control_ErrorRenderingShort") + "<br />" + e.Message);
        }

        public override void Initialize(IComponent component)
        {
            ControlDesigner.VerifyInitializeArgument(component, typeof(Login));
            this._login = (Login) component;
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
            this.TemplateDescriptor.SetValue(this._login, null);
            return true;
        }

        public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
        {
            IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
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
                lists.Add(new LoginDesignerActionList(this));
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
                    _autoFormats = ControlDesigner.CreateAutoFormats(AutoFormatSchemes.LOGIN_SCHEME_NAMES, CS9__CachedAnonymousMethodDelegate1);
                }
                return _autoFormats;
            }
        }

        public bool RenderOuterTable
        {
            get
            {
                return ((Login) base.Component).RenderOuterTable;
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
                return (this._login.LayoutTemplate != null);
            }
        }

        private System.Web.UI.Design.TemplateDefinition TemplateDefinition
        {
            get
            {
                return new System.Web.UI.Design.TemplateDefinition(this, "LayoutTemplate", this._login, "LayoutTemplate", ((WebControl) base.ViewControl).ControlStyle);
            }
        }

        private PropertyDescriptor TemplateDescriptor
        {
            get
            {
                return TypeDescriptor.GetProperties(base.Component).Find("LayoutTemplate", false);
            }
        }

        public override TemplateGroupCollection TemplateGroups
        {
            get
            {
                TemplateGroupCollection templateGroups = base.TemplateGroups;
                TemplateGroup group = new TemplateGroup("LayoutTemplate", ((WebControl) base.ViewControl).ControlStyle);
                group.AddTemplateDefinition(new System.Web.UI.Design.TemplateDefinition(this, "LayoutTemplate", this._login, "LayoutTemplate", ((WebControl) base.ViewControl).ControlStyle));
                templateGroups.Add(group);
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

        private sealed class ConvertToTemplateHelper : LoginDesignerUtil.GenericConvertToTemplateHelper<Login, LoginDesigner>
        {
            private static readonly string[] _persistedControlIDs = new string[] { "UserName", "UserNameRequired", "Password", "PasswordRequired", "RememberMe", "LoginButton", "LoginImageButton", "LoginLinkButton", "FailureText", "CreateUserLink", "PasswordRecoveryLink", "HelpLink" };
            private static readonly string[] _persistedIfNotVisibleControlIDs = new string[] { "FailureText" };

            public ConvertToTemplateHelper(LoginDesigner designer, IDesignerHost designerHost) : base(designer, designerHost)
            {
            }

            protected override System.Web.UI.Control GetDefaultTemplateContents()
            {
                System.Web.UI.Control control = base.Designer.ViewControl.Controls[0];
                return (Table) control.Controls[0];
            }

            protected override Style GetFailureTextStyle(Login control)
            {
                return control.FailureTextStyle;
            }

            protected override ITemplate GetTemplate(Login control)
            {
                return control.LayoutTemplate;
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

        private class LoginDesignerActionList : DesignerActionList
        {
            private LoginDesigner _parent;

            public LoginDesignerActionList(LoginDesigner parent) : base(parent.Component)
            {
                this._parent = parent;
            }

            public void ConvertToTemplate()
            {
                Cursor current = Cursor.Current;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    this._parent.ConvertToTemplate();
                }
                finally
                {
                    Cursor.Current = current;
                }
            }

            public override DesignerActionItemCollection GetSortedActionItems()
            {
                if (this._parent.InTemplateMode)
                {
                    return new DesignerActionItemCollection();
                }
                DesignerActionItemCollection items = new DesignerActionItemCollection();
                if (!this._parent.Templated)
                {
                    items.Add(new DesignerActionMethodItem(this, "ConvertToTemplate", System.Design.SR.GetString("WebControls_ConvertToTemplate"), string.Empty, System.Design.SR.GetString("WebControls_ConvertToTemplateDescription"), true));
                }
                else
                {
                    items.Add(new DesignerActionMethodItem(this, "Reset", System.Design.SR.GetString("WebControls_Reset"), string.Empty, System.Design.SR.GetString("WebControls_ResetDescription"), true));
                }
                items.Add(new DesignerActionMethodItem(this, "LaunchWebAdmin", System.Design.SR.GetString("Login_LaunchWebAdmin"), string.Empty, System.Design.SR.GetString("Login_LaunchWebAdminDescription"), true));
                return items;
            }

            public void LaunchWebAdmin()
            {
                this._parent.LaunchWebAdmin();
            }

            public void Reset()
            {
                this._parent.Reset();
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
        }
    }
}

