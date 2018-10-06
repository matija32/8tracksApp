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
    public class UserElement
    {
        private AvatarUrlsElement[] avatarurlsField;
        private bool followedbycurrentuserField;
        private int idField;
        private string loginField;
        private string slugField;

        /// <remarks/>
        [XmlElement("id", Form = XmlSchemaForm.Unqualified)]
        public int Id
        {
            get { return idField; }
            set { idField = value; }
        }

        /// <remarks/>
        [XmlElement("login", Form = XmlSchemaForm.Unqualified)]
        public string Login
        {
            get { return loginField; }
            set { loginField = value; }
        }

        /// <remarks/>
        [XmlElement("slug", Form = XmlSchemaForm.Unqualified)]
        public string Slug
        {
            get { return slugField; }
            set { slugField = value; }
        }

        /// <remarks/>
        [XmlElement("followed-by-current-user", Form = XmlSchemaForm.Unqualified)]
        public bool FollowedByCurrentUser
        {
            get { return followedbycurrentuserField; }
            set { followedbycurrentuserField = value; }
        }

        /// <remarks/>
        [XmlElement("avatar-urls", Form = XmlSchemaForm.Unqualified)]
        public AvatarUrlsElement[] AvatarUrlsElement
        {
            get { return avatarurlsField; }
            set { avatarurlsField = value; }
        }
    }
}