using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace EightTracksPlayer.Communication.Responses.SupportingElements
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class CoverUrlsElement
    {
        private string max1024Field;
        private string max133wField;
        private string max200Field;
        private string originalField;
        private string sq100Field;
        private string sq133Field;
        private string sq250Field;
        private string sq500Field;
        private string sq56Field;
        private string originalUrlField;
        private bool useOriginalOnMixpageField;
        private string croppedImgixUrlField;
        private string originalImgixUrlField;

        /// <remarks/>
        [XmlElement("sq56", Form = XmlSchemaForm.Unqualified)]
        public string Sq56
        {
            get { return sq56Field; }
            set { sq56Field = value; }
        }

        /// <remarks/>
        [XmlElement("sq100", Form = XmlSchemaForm.Unqualified)]
        public string Sq100
        {
            get { return sq100Field; }
            set { sq100Field = value; }
        }

        /// <remarks/>
        [XmlElement("sq133", Form = XmlSchemaForm.Unqualified)]
        public string Sq133
        {
            get { return sq133Field; }
            set { sq133Field = value; }
        }

        /// <remarks/>
        [XmlElement("max133w", Form = XmlSchemaForm.Unqualified)]
        public string Max133w
        {
            get { return max133wField; }
            set { max133wField = value; }
        }

        /// <remarks/>
        [XmlElement("max200", Form = XmlSchemaForm.Unqualified)]
        public string Max200
        {
            get { return max200Field; }
            set { max200Field = value; }
        }

        /// <remarks/>
        [XmlElement("sq250", Form = XmlSchemaForm.Unqualified)]
        public string sq250
        {
            get { return sq250Field; }
            set { sq250Field = value; }
        }

        /// <remarks/>
        [XmlElement("sq500", Form = XmlSchemaForm.Unqualified)]
        public string Sq500
        {
            get { return sq500Field; }
            set { sq500Field = value; }
        }

        /// <remarks/>
        [XmlElement("max1024", Form = XmlSchemaForm.Unqualified)]
        public string Max1024
        {
            get { return max1024Field; }
            set { max1024Field = value; }
        }

        /// <remarks/>
        [XmlElement("original", Form = XmlSchemaForm.Unqualified)]
        public string Original
        {
            get { return originalField; }
            set { originalField = value; }
        }

        /// <remarks/>
        [XmlElement("original-url", Form = XmlSchemaForm.Unqualified)]
        public string OriginalUrl
        {
            get { return originalUrlField; }
            set { originalUrlField = value; }
        }

        /// <remarks/>
        [XmlElement("use-original-on-mixpage", Form = XmlSchemaForm.Unqualified)]
        public bool UseOriginalOnMixpage
        {
            get { return useOriginalOnMixpageField; }
            set { useOriginalOnMixpageField = value; }
        }

        /// <remarks/>
        [XmlElement("cropped-imgix-url", Form = XmlSchemaForm.Unqualified)]
        public string CroppedImgixUrl
        {
            get { return croppedImgixUrlField; }
            set { croppedImgixUrlField = value; }
        }

        /// <remarks/>
        [XmlElement("original-imgix-url", Form = XmlSchemaForm.Unqualified)]
        public string OriginalImgixUrl
        {
            get { return originalImgixUrlField; }
            set { originalImgixUrlField = value; }
        }
    }
}