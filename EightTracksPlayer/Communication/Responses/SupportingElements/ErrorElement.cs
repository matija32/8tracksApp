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
    public class ErrorElement
    {
        private string value;

        /// <remarks/>
        [XmlText]
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}