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
    public class MixElement
    {
        private CoverUrlsElement coverUrlsField = new CoverUrlsElement();
        private string descriptionField = String.Empty;
        private DateTime firstPublishedAtField;
        private int idField;
        private bool likedByCurrentUserField;
        private int likesCountField;
        private string nameField;
        private bool nsfwField;

        private string pathField;
        private int playsCountField;
        private bool publishedField;

        private string restfulUrlField;
        private string slugField;
        private string tagListCacheField;
        private UserElement userElementField = new UserElement();
        private int userIdField;

        private int durationField;
        private int tracksCountField;

        /// <remarks/>
        [XmlElement("id", Form = XmlSchemaForm.Unqualified)]
        public int Id
        {
            get { return idField; }
            set { idField = value; }
        }

        /// <remarks/>
        [XmlElement("duration", Form = XmlSchemaForm.Unqualified)]
        public int Duration
        {
            get { return durationField; }
            set { durationField = value; }
        }

        /// <remarks/>
        [XmlElement("tracks-count", Form = XmlSchemaForm.Unqualified)]
        public int TracksCount
        {
            get { return tracksCountField; }
            set { tracksCountField = value; }
        }

        /// <remarks/>
        [XmlElement("name", Form = XmlSchemaForm.Unqualified)]
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }

        /// <remarks/>
        [XmlElement("published", Form = XmlSchemaForm.Unqualified)]
        public bool Published
        {
            get { return publishedField; }
            set { publishedField = value; }
        }

        /// <remarks/>
        [XmlElement("description", Form = XmlSchemaForm.Unqualified)]
        public string Description
        {
            get { return descriptionField; }
            set { descriptionField = value; }
        }

        /// <remarks/>
        [XmlElement("plays-count", Form = XmlSchemaForm.Unqualified)]
        public int PlaysCount
        {
            get { return playsCountField; }
            set { playsCountField = value; }
        }

        /// <remarks/>
        [XmlElement("likes-count", Form = XmlSchemaForm.Unqualified)]
        public int LikesCount
        {
            get { return likesCountField; }
            set { likesCountField = value; }
        }

        /// <remarks/>
        [XmlElement("slug", Form = XmlSchemaForm.Unqualified)]
        public string Slug
        {
            get { return slugField; }
            set { slugField = value; }
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
        [XmlElement("tag-list-cache", Form = XmlSchemaForm.Unqualified)]
        public string TagListCache
        {
            get { return tagListCacheField; }
            set { tagListCacheField = value; }
        }

        /// <remarks/>
        [XmlElement("first-published-at", Form = XmlSchemaForm.Unqualified)]
        public DateTime FirstPublishedAt
        {
            get { return firstPublishedAtField; }
            set { firstPublishedAtField = value; }
        }

        /// <remarks/>
        [XmlElement("liked-by-current-user", Form = XmlSchemaForm.Unqualified)]
        public bool LikedByCurrentUser
        {
            get { return likedByCurrentUserField; }
            set { likedByCurrentUserField = value; }
        }

        /// <remarks/>
        [XmlElement("nsfw", Form = XmlSchemaForm.Unqualified)]
        public bool Nsfw
        {
            get { return nsfwField; }
            set { nsfwField = value; }
        }

        /// <remarks/>
        [XmlElement("cover-urls", Form = XmlSchemaForm.Unqualified)]
        public CoverUrlsElement CoverUrls
        {
            get { return coverUrlsField; }
            set { coverUrlsField = value; }
        }

        /// <remarks/>
        [XmlElement("user", Form = XmlSchemaForm.Unqualified)]
        public UserElement UserElement
        {
            get { return userElementField; }
            set { userElementField = value; }
        }

        [XmlElement("user-id", Form = XmlSchemaForm.Unqualified)]
        public int UserId
        {
            get { return userIdField; }
            set { userIdField = value; }
        }
    }
}