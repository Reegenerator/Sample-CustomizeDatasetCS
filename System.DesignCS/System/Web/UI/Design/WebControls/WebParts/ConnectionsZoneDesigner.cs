namespace System.Web.UI.Design.WebControls.WebParts
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Security.Permissions;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.Design.WebControls;
    using System.Web.UI.WebControls.WebParts;

    [SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.UnmanagedCode)]
    public class ConnectionsZoneDesigner : ToolZoneDesigner
    {
        private static DesignerAutoFormatCollection _autoFormats;
        private static readonly string[] _hiddenProperties = new string[] { "EmptyZoneTextStyle", "PartChromeStyle", "PartStyle", "PartTitleStyle" };
        private ConnectionsZone _zone;

        public override string GetDesignTimeHtml()
        {
            string designTimeHtml;
            try
            {
                ConnectionsZone viewControl = (ConnectionsZone) base.ViewControl;
                designTimeHtml = base.GetDesignTimeHtml();
                if (base.ViewInBrowseMode && (viewControl.ID != "AutoFormatPreviewControl"))
                {
                    designTimeHtml = base.CreatePlaceHolderDesignTimeHtml();
                }
            }
            catch (Exception exception)
            {
                designTimeHtml = this.GetErrorDesignTimeHtml(exception);
            }
            return designTimeHtml;
        }

        public override void Initialize(IComponent component)
        {
            ControlDesigner.VerifyInitializeArgument(component, typeof(ConnectionsZone));
            base.Initialize(component);
            this._zone = (ConnectionsZone) component;
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            Attribute[] attributes = new Attribute[] { new BrowsableAttribute(false), new EditorBrowsableAttribute(EditorBrowsableState.Never), new ThemeableAttribute(false) };
            foreach (string str in _hiddenProperties)
            {
                PropertyDescriptor oldPropertyDescriptor = (PropertyDescriptor) properties[str];
                if (oldPropertyDescriptor != null)
                {
                    properties[str] = TypeDescriptor.CreateProperty(oldPropertyDescriptor.ComponentType, oldPropertyDescriptor, attributes);
                }
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
                    _autoFormats = ControlDesigner.CreateAutoFormats(AutoFormatSchemes.CONNECTIONSZONE_SCHEME_NAMES, CS9__CachedAnonymousMethodDelegate1);
                }
                return _autoFormats;
            }
        }
    }
}

