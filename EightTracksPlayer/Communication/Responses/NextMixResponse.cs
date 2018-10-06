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
    public class NextMixResponse : ResponseBase
    {
        private MixElement mixElement = new MixElement();
        private string mixSetIdField = "";

        [XmlElement("next-mix", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public MixElement NextMix
        {
            get { return mixElement; }
            set { mixElement = value; }
        }

        [XmlElement("mix-set-id", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public string MixSetId
        {
            get { return mixSetIdField; }
            set { mixSetIdField = value; }
        }
    }
}