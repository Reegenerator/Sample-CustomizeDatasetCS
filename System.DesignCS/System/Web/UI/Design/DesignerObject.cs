﻿namespace System.Web.UI.Design
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Runtime;

    public abstract class DesignerObject : IServiceProvider
    {
        private ControlDesigner _designer;
        private string _name;
        private IDictionary _properties;

        protected DesignerObject(ControlDesigner designer, string name)
        {
            if (designer == null)
            {
                throw new ArgumentNullException("designer");
            }
            if ((name == null) || (name.Length == 0))
            {
                throw new ArgumentNullException("name");
            }
            this._designer = designer;
            this._name = name;
        }

        protected object GetService(Type serviceType)
        {
            IServiceProvider site = this._designer.Component.Site;
            if (site != null)
            {
                return site.GetService(serviceType);
            }
            return null;
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        object IServiceProvider.GetService(Type serviceType)
        {
            return this.GetService(serviceType);
        }

        public ControlDesigner Designer
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._designer;
            }
        }

        public string Name
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._name;
            }
        }

        public IDictionary Properties
        {
            get
            {
                if (this._properties == null)
                {
                    this._properties = new HybridDictionary();
                }
                return this._properties;
            }
        }
    }
}

