using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace EightTracksPlayer.Communication.Responses.SupportingElements
{
    /// <remarks/>
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class CurrentUserElement : UserElement
    {
        private string bioHtmlField;
        private int followsCountField;
        private int hideNSFWField;
        private string locationField;
        private string locationNotNilField;
        private string nextMixPrefsField;
        private string pathField;
        private string popupPrefsField;
        private string subscribedField;

        [XmlElementAttribute("popup-prefs", Form = XmlSchemaForm.Unqualified)]
        public string PopupPrefs
        {
            get { return popupPrefsField; }
            set { popupPrefsField = value; }
        }

        /// <remarks/>
        [XmlElementAttribute("next-mix-prefs", Form = XmlSchemaForm.Unqualified)]
        public string NextMixPrefs
        {
            get { return nextMixPrefsField; }
            set { nextMixPrefsField = value; }
        }

        /// <remarks/>
        [XmlElementAttribute("location", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public string Location
        {
            get { return locationField; }
            set { locationField = value; }
        }

        /// <remarks/>
        [XmlElementAttribute("location-not-nil", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public string LocationNotNil
        {
            get { return locationNotNilField; }
            set { locationNotNilField = value; }
        }

        /// <remarks/>
        [XmlElementAttribute("path", Form = XmlSchemaForm.Unqualified)]
        public string Path
        {
            get { return pathField; }
            set { pathField = value; }
        }

        /// <remarks/>
        [XmlElementAttribute("subscribed", Form = XmlSchemaForm.Unqualified)]
        public string Subscribed
        {
            get { return subscribedField; }
            set { subscribedField = value; }
        }

        /// <remarks/>
        [XmlElementAttribute("bio-html", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public string BioHtml
        {
            get { return bioHtmlField; }
            set { bioHtmlField = value; }
        }

        /// <remarks/>
        [XmlElementAttribute("follows-count", Form = XmlSchemaForm.Unqualified)]
        public int FollowsCount
        {
            get { return followsCountField; }
            set { followsCountField = value; }
        }

        /// <remarks/>
        [XmlElementAttribute("hide-nsfw", Form = XmlSchemaForm.Unqualified)]
        public int HideNsfw
        {
            get { return hideNSFWField; }
            set { hideNSFWField = value; }
        }
    }
}