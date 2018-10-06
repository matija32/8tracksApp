using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace EightTracksPlayer.Communication.Responses.SupportingElements
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class PageElement
    {
        private string value;

        /// <remarks/>
        [XmlText]
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public int PageNumber
        {
            get { return String.IsNullOrEmpty(value) ? 0 : Int32.Parse(value); }
        }
    }
}