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
    public class AvatarUrlsElement
    {
        private string max200Field;
        private string max250wField;
        private string sq100Field;
        private string sq56Field;
        private string sq72Field;
        private string originalField ;

        /// <remarks/>
        [XmlElement("sq56", Form = XmlSchemaForm.Unqualified)]
        public string Sq56
        {
            get { return sq56Field; }
            set { sq56Field = value; }
        }

        /// <remarks/>
        [XmlElement("sq72", Form = XmlSchemaForm.Unqualified)]
        public string Sq72
        {
            get { return sq72Field; }
            set { sq72Field = value; }
        }

        /// <remarks/>
        [XmlElement("sq100", Form = XmlSchemaForm.Unqualified)]
        public string Sq100
        {
            get { return sq100Field; }
            set { sq100Field = value; }
        }

        /// <remarks/>
        [XmlElement("max200", Form = XmlSchemaForm.Unqualified)]
        public string Max200
        {
            get { return max200Field; }
            set { max200Field = value; }
        }

        /// <remarks/>
        [XmlElement("max250w", Form = XmlSchemaForm.Unqualified)]
        public string Max250w
        {
            get { return max250wField; }
            set { max250wField = value; }
        }

        /// <remarks/>
        [XmlElement("original", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public string Original
        {
            get { return originalField; }
            set { originalField = value; }
        }
    }
}