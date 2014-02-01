namespace System.Web.UI.Design.WebControls.WebParts
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Design;
    using System.Security.Permissions;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.Design.WebControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;

    [SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.UnmanagedCode)]
    public class WebPartZoneDesigner : WebPartZoneBaseDesigner
    {
        private static DesignerAutoFormatCollection _autoFormats;
        private TemplateGroup _templateGroup;
        private WebPartZone _zone;

        public override string GetDesignTimeHtml()
        {
            return this.GetDesignTimeHtml(null);
        }

        public override string GetDesignTimeHtml(DesignerRegionCollection regions)
        {
            try
            {
                WebPartZone viewControl = (WebPartZone) base.ViewControl;
                bool flag = base.UseRegions(regions, this._zone.ZoneTemplate, viewControl.ZoneTemplate);
                if ((viewControl.ZoneTemplate == null) && !flag)
                {
                    return this.GetEmptyDesignTimeHtml();
                }
                ((ICompositeControlDesignerAccessor) viewControl).RecreateChildControls();
                if (flag)
                {
                    viewControl.Controls.Clear();
                    WebPartEditableDesignerRegion region = new WebPartEditableDesignerRegion(viewControl, base.TemplateDefinition) {
                        IsSingleInstanceTemplate = true,
                        Description = System.Design.SR.GetString("ContainerControlDesigner_RegionWatermark")
                    };
                    regions.Add(region);
                }
                return base.GetDesignTimeHtml();
            }
            catch (Exception exception)
            {
                return this.GetErrorDesignTimeHtml(exception);
            }
        }

        public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
        {
            return ControlPersister.PersistTemplate(this._zone.ZoneTemplate, (IDesignerHost) base.Component.Site.GetService(typeof(IDesignerHost)));
        }

        protected override string GetEmptyDesignTimeHtml()
        {
            return base.CreatePlaceHolderDesignTimeHtml(System.Design.SR.GetString("WebPartZoneDesigner_Empty"));
        }

        public override void Initialize(IComponent component)
        {
            ControlDesigner.VerifyInitializeArgument(component, typeof(WebPartZone));
            base.Initialize(component);
            this._zone = (WebPartZone) component;
        }

        public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
        {
            this._zone.ZoneTemplate = ControlParser.ParseTemplate((IDesignerHost) base.Component.Site.GetService(typeof(IDesignerHost)), content);
            base.IsDirtyInternal = true;
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
                    _autoFormats = ControlDesigner.CreateAutoFormats(AutoFormatSchemes.WEBPARTZONE_SCHEME_NAMES, CS9__CachedAnonymousMethodDelegate1);
                }
                return _autoFormats;
            }
        }

        public override TemplateGroupCollection TemplateGroups
        {
            get
            {
                TemplateGroupCollection templateGroups = base.TemplateGroups;
                if (this._templateGroup == null)
                {
                    this._templateGroup = base.CreateZoneTemplateGroup();
                }
                templateGroups.Add(this._templateGroup);
                return templateGroups;
            }
        }

        private sealed class WebPartEditableDesignerRegion : TemplatedEditableDesignerRegion
        {
            private WebPartZoneBase _zone;

            public WebPartEditableDesignerRegion(WebPartZoneBase zone, TemplateDefinition templateDefinition) : base(templateDefinition)
            {
                this._zone = zone;
            }

            public override ViewRendering GetChildViewRendering(Control control)
            {
                if (control == null)
                {
                    throw new ArgumentNullException("control");
                }
                DesignerWebPartChrome chrome = new DesignerWebPartChrome(this._zone);
                return chrome.GetViewRendering(control);
            }
        }
    }
}

