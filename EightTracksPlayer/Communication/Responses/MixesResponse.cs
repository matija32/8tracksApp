using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;
using EightTracksPlayer.Communication.Responses.SupportingElements;

namespace EightTracksPlayer.Communication.Responses
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("response", Namespace = "", IsNullable = false)]
    public class MixesResponse : ResponseBase
    {
        private string canonicalPathField;
        private long idField;
        private bool loggedInField;
        private MixElement[] mixesElementField = new MixElement[0];
        private string nameField;
        private PageElement nextPageField = new PageElement();
        private int pageField;
        private string pathField;
        private int perPageField;
        private PageElement previousPageField = new PageElement();

        private string restfulUrlField;

        private int totalEntriesField;

        private int totalPagesField;

        /// <remarks/>
        [XmlElement("id", Form = XmlSchemaForm.Unqualified)]
        public long Id
        {
            get { return idField; }
            set { idField = value; }
        }

        /// <remarks/>
        [XmlElement("path", Form = XmlSchemaForm.Unqualified)]
        public string Path
        {
            get { return pathField; }
            set { pathField = value; }
        }

        /// <remarks/>
        [XmlElement("restful-url", Form = XmlSchemaForm.Unqualified)]
        public string RestfulUrl
        {
            get { return restfulUrlField; }
            set { restfulUrlField = value; }
        }

        /// <remarks/>
        [XmlElement("total-entries", Form = XmlSchemaForm.Unqualified)]
        public int TotalEntries
        {
            get { return totalEntriesField; }
            set { totalEntriesField = value; }
        }

        /// <remarks/>
        [XmlElement("page", Form = XmlSchemaForm.Unqualified)]
        public int Page
        {
            get { return pageField; }
            set { pageField = value; }
        }

        /// <remarks/>
        [XmlElement("per-page", Form = XmlSchemaForm.Unqualified)]
        public int PerPage
        {
            get { return perPageField; }
            set { perPageField = value; }
        }

        /// <remarks/>
        [XmlElement("next-page", Form = XmlSchemaForm.Unqualified)]
        public PageElement NextPage
        {
            get { return nextPageField; }
            set { nextPageField = value; }
        }

        /// <remarks/>
        [XmlElement("total-pages", Form = XmlSchemaForm.Unqualified)]
        public int TotalPages
        {
            get { return totalPagesField; }
            set { totalPagesField = value; }
        }

        /// <remarks/>
        [XmlElement("canonical-path", Form = XmlSchemaForm.Unqualified)]
        public string CanonicalPath
        {
            get { return canonicalPathField; }
            set { canonicalPathField = value; }
        }

        /// <remarks/>
        [XmlElement("name", Form = XmlSchemaForm.Unqualified)]
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }

        /// <remarks/>
        [XmlElement("logged-in", Form = XmlSchemaForm.Unqualified)]
        public bool LoggedIn
        {
            get { return loggedInField; }
            set { loggedInField = value; }
        }

        /// <remarks/>
        [XmlElement("previous-page", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public PageElement PreviousPage
        {
            get { return previousPageField; }
            set { previousPageField = value; }
        }

        /// <remarks/>
        [XmlArrayAttribute("mixes", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("mix", typeof (MixElement), Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public MixElement[] MixesElement
        {
            get { return mixesElementField; }
            set { mixesElementField = value; }
        }
    }
}