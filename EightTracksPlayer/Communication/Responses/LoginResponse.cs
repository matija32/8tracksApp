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
    public class LoginResponse : ResponseBase
    {
        private string authTokenField;
        private CurrentUserElement currentUserElementField;
        private bool loggedIn;
        private string userTokenField;

        [XmlElement("logged-in", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public bool LoggedIn
        {
            get { return loggedIn; }
            set { loggedIn = value; }
        }

        /// <remarks/>
        [XmlElement("current-user", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public CurrentUserElement CurrentUserElement
        {
            get { return currentUserElementField; }
            set { currentUserElementField = value; }
        }

        /// <remarks/>
        [XmlElement("auth-token", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public string AuthToken
        {
            get { return authTokenField; }
            set { authTokenField = value; }
        }

        /// <remarks/>
        [XmlElement("user-token", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public string UserToken
        {
            get { return userTokenField; }
            set { userTokenField = value; }
        }
    }
}