using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;
using EightTracksPlayer.Communication.Responses.SupportingElements;
using EightTracksPlayer.Domain.Entities;

namespace EightTracksPlayer.Communication.Responses
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("response", Namespace = "", IsNullable = false)]
    public class ToggleMixLikeResponse : ResponseBase
    {
        private bool loggedInField;
        private MixElement mixField = new MixElement();

        /// <remarks/>
        [XmlElement("mix", Form = XmlSchemaForm.Unqualified)]
        public MixElement Mix
        {
            get { return mixField; }
            set { mixField = value; }
        }

        [XmlElement("logged-in", Form = XmlSchemaForm.Unqualified)]
        public bool LoggedIn
        {
            get { return loggedInField; }
            set { loggedInField = value; }
        }
    }
}