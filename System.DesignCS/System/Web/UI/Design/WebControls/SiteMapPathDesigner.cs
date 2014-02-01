namespace System.Web.UI.Design.WebControls
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Design;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;

    [SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.UnmanagedCode)]
    public class SiteMapPathDesigner : ControlDesigner
    {
        private DesignerAutoFormatCollection _autoFormats;
        private static string[] _controlTemplateNames = new string[] { "NodeTemplate", "CurrentNodeTemplate", "RootNodeTemplate", "PathSeparatorTemplate" };
        private SiteMapPath _navigationPath;
        private SiteMapProvider _siteMapProvider;
        private static Style[] _templateStyleArray;

        public override string GetDesignTimeHtml()
        {
            SiteMapPath viewControl = (SiteMapPath) base.ViewControl;
            try
            {
                viewControl.Provider = this.DesignTimeSiteMapProvider;
                ICompositeControlDesignerAccessor accessor = viewControl;
                accessor.RecreateChildControls();
                return base.GetDesignTimeHtml();
            }
            catch (Exception exception)
            {
                return this.GetErrorDesignTimeHtml(exception);
            }
        }

        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            return base.CreatePlaceHolderDesignTimeHtml(System.Design.SR.GetString("Control_ErrorRendering") + e.Message);
        }

        public override void Initialize(IComponent component)
        {
            ControlDesigner.VerifyInitializeArgument(component, typeof(SiteMapPath));
            base.Initialize(component);
            this._navigationPath = (SiteMapPath) component;
            if (base.View != null)
            {
                base.View.SetFlags(ViewFlags.TemplateEditing, true);
            }
        }

        public override DesignerAutoFormatCollection AutoFormats
        {
            get
            {
                if (this._autoFormats == null)
                {
                    if (CS9__CachedAnonymousMethodDelegate1 == null)
                    {
                        CS9__CachedAnonymousMethodDelegate1 = new Func<string, DesignerAutoFormat>(null, (IntPtr) <get_AutoFormats>b__0);
                    }
                    this._autoFormats = ControlDesigner.CreateAutoFormats(AutoFormatSchemes.SITEMAPPATH_SCHEME_NAMES, CS9__CachedAnonymousMethodDelegate1);
                }
                return this._autoFormats;
            }
        }

        private SiteMapProvider DesignTimeSiteMapProvider
        {
            get
            {
                if (this._siteMapProvider == null)
                {
                    IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
                    this._siteMapProvider = new System.Web.UI.Design.WebControls.DesignTimeSiteMapProvider(service);
                }
                return this._siteMapProvider;
            }
        }

        public override TemplateGroupCollection TemplateGroups
        {
            get
            {
                TemplateGroupCollection templateGroups = base.TemplateGroups;
                for (int i = 0; i < _controlTemplateNames.Length; i++)
                {
                    string groupName = _controlTemplateNames[i];
                    TemplateGroup group = new TemplateGroup(groupName);
                    group.AddTemplateDefinition(new TemplateDefinition(this, groupName, base.Component, groupName, this.TemplateStyleArray[i]));
                    templateGroups.Add(group);
                }
                return templateGroups;
            }
        }

        private Style[] TemplateStyleArray
        {
            get
            {
                if (_templateStyleArray == null)
                {
                    _templateStyleArray = new Style[] { ((SiteMapPath) base.ViewControl).NodeStyle, ((SiteMapPath) base.ViewControl).CurrentNodeStyle, ((SiteMapPath) base.ViewControl).RootNodeStyle, ((SiteMapPath) base.ViewControl).PathSeparatorStyle };
                }
                return _templateStyleArray;
            }
        }

        protected override bool UsePreviewControl
        {
            get
            {
                return true;
            }
        }
    }
}

