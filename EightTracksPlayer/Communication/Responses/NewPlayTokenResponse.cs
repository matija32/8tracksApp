using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace EightTracksPlayer.Communication.Responses
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("response", Namespace = "", IsNullable = false)]
    public class NewPlayTokenResponse : ResponseBase
    {
        private string playToken;

        [XmlElement("play-token", Form = XmlSchemaForm.Unqualified)]
        public string PlayToken
        {
            get { return playToken; }
            set { playToken = value; }
        }
    }
}