namespace System.Web.UI.Design
{
    using System;
    using System.Runtime;

    public class ContentDefinition
    {
        private string _contentPlaceHolderID;
        private string _defaultContent;
        private string _defaultDesignTimeHTML;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ContentDefinition(string id, string content, string designTimeHtml)
        {
            this._contentPlaceHolderID = id;
            this._defaultContent = content;
            this._defaultDesignTimeHTML = designTimeHtml;
        }

        public string ContentPlaceHolderID
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._contentPlaceHolderID;
            }
        }

        public string DefaultContent
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._defaultContent;
            }
        }

        public string DefaultDesignTimeHtml
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._defaultDesignTimeHTML;
            }
        }
    }
}

