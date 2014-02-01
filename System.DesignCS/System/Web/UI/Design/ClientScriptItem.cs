namespace System.Web.UI.Design
{
    using System;
    using System.Runtime;

    public sealed class ClientScriptItem
    {
        private string _id;
        private string _language;
        private string _source;
        private string _text;
        private string _type;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ClientScriptItem(string text, string source, string language, string type, string id)
        {
            this._text = text;
            this._source = source;
            this._language = language;
            this._type = type;
            this._id = id;
        }

        public string Id
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._id;
            }
        }

        public string Language
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._language;
            }
        }

        public string Source
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._source;
            }
        }

        public string Text
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._text;
            }
        }

        public string Type
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._type;
            }
        }
    }
}

