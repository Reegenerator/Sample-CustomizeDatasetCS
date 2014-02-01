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
    public class EditorZoneDesigner : ToolZoneDesigner
    {
        private static DesignerAutoFormatCollection _autoFormats;
        private TemplateGroup _templateGroup;
        private EditorZone _zone;

        public override string GetDesignTimeHtml()
        {
            return this.GetDesignTimeHtml(null);
        }

        public override string GetDesignTimeHtml(DesignerRegionCollection regions)
        {
            string emptyDesignTimeHtml;
            try
            {
                EditorZone viewControl = (EditorZone) base.ViewControl;
                bool flag = base.UseRegions(regions, this._zone.ZoneTemplate, viewControl.ZoneTemplate);
                if ((viewControl.ZoneTemplate == null) && !flag)
                {
                    emptyDesignTimeHtml = this.GetEmptyDesignTimeHtml();
                }
                else
                {
                    ((ICompositeControlDesignerAccessor) viewControl).RecreateChildControls();
                    if ((regions != null) && flag)
                    {
                        viewControl.Controls.Clear();
                        EditorPartEditableDesignerRegion region = new EditorPartEditableDesignerRegion(viewControl, base.TemplateDefinition);
                        region.Properties[typeof(Control)] = viewControl;
                        region.IsSingleInstanceTemplate = true;
                        region.Description = System.Design.SR.GetString("ContainerControlDesigner_RegionWatermark");
                        regions.Add(region);
                    }
                    emptyDesignTimeHtml = base.GetDesignTimeHtml();
                }
                if (base.ViewInBrowseMode && (viewControl.ID != "AutoFormatPreviewControl"))
                {
                    emptyDesignTimeHtml = base.CreatePlaceHolderDesignTimeHtml();
                }
            }
            catch (Exception exception)
            {
                emptyDesignTimeHtml = this.GetErrorDesignTimeHtml(exception);
            }
            return emptyDesignTimeHtml;
        }

        public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
        {
            return ControlPersister.PersistTemplate(this._zone.ZoneTemplate, (IDesignerHost) base.Component.Site.GetService(typeof(IDesignerHost)));
        }

        protected override string GetEmptyDesignTimeHtml()
        {
            return base.CreatePlaceHolderDesignTimeHtml(System.Design.SR.GetString("EditorZoneDesigner_Empty"));
        }

        public override void Initialize(IComponent component)
        {
            ControlDesigner.VerifyInitializeArgument(component, typeof(EditorZone));
            base.Initialize(component);
            this._zone = (EditorZone) component;
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
                    _autoFormats = ControlDesigner.CreateAutoFormats(AutoFormatSchemes.EDITORZONE_SCHEME_NAMES, CS9__CachedAnonymousMethodDelegate1);
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

        private sealed class EditorPartEditableDesignerRegion : TemplatedEditableDesignerRegion
        {
            private EditorZone _zone;

            public EditorPartEditableDesignerRegion(EditorZone zone, TemplateDefinition templateDefinition) : base(templateDefinition)
            {
                this._zone = zone;
            }

            public override ViewRendering GetChildViewRendering(Control control)
            {
                if (control == null)
                {
                    throw new ArgumentNullException("control");
                }
                DesignerEditorPartChrome chrome = new DesignerEditorPartChrome(this._zone);
                return chrome.GetViewRendering(control);
            }
        }
    }
}

