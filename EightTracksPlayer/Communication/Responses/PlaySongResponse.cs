using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using EightTracksPlayer.Communication.Responses.SupportingElements;

namespace EightTracksPlayer.Communication.Responses
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("response", Namespace = "", IsNullable = false)]
    public class PlaySongResponse : ResponseBase
    {
        private SetElement setElement = new SetElement();

        [XmlElement("set", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public SetElement SetElement
        {
            get { return setElement; }
            set { setElement = value; }
        }
    }
}