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
    [XmlRoot("api-warning", Namespace = "", IsNullable = false)]
    public class ApiWarningElement
    {
        private string value;

        [XmlText]
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}