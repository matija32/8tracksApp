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
    public class TrackElement
    {
        private string buyIcon;
        private string buyLink;
        private bool favoritedByCurrentUser;
        private long id;
        private string name;
        private string performer;
        private double playDuration;
        private string releaseName;
        private string url;
        private string youTubeIdField;
        private string youTubeStatusField;

        [XmlElement("name", Form = XmlSchemaForm.Unqualified)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlElement("performer", Form = XmlSchemaForm.Unqualified)]
        public string Performer
        {
            get { return performer; }
            set { performer = value; }
        }

        [XmlElement("release-name", Form = XmlSchemaForm.Unqualified)]
        public string ReleaseName
        {
            get { return releaseName; }
            set { releaseName = value; }
        }

        [XmlIgnore]
        public int Year { get; set; }

        [XmlElement("year")]
        public string YearAsText
        {
            get { return Year.ToString(); }
            set { Year = !String.IsNullOrEmpty(value) ? int.Parse(value) : 0; }
        }

        [XmlElement("you-tube-id")]
        public string YouTubeId
        {
            get { return youTubeIdField; }
            set { youTubeIdField = !String.IsNullOrEmpty(value) ? value : ""; }
        }

        [XmlElement("you-tube-status", Form = XmlSchemaForm.Unqualified)]
        public string YouTubeStatus
        {
            get { return youTubeStatusField; }
            set { youTubeStatusField = value; }
        }

        [XmlElement("url", Form = XmlSchemaForm.Unqualified)]
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        [XmlElement("faved-by-current-user", Form = XmlSchemaForm.Unqualified)]
        public bool FavoritedByCurrentUser
        {
            get { return favoritedByCurrentUser; }
            set { favoritedByCurrentUser = value; }
        }

        [XmlElement("buy-link", Form = XmlSchemaForm.Unqualified)]
        public string BuyLink
        {
            get { return buyLink; }
            set { buyLink = value; }
        }

        [XmlElement("buy-icon", Form = XmlSchemaForm.Unqualified)]
        public string BuyIcon
        {
            get { return buyIcon; }
            set { buyIcon = value; }
        }

        [XmlElement("play-duration", Form = XmlSchemaForm.Unqualified)]
        public double PlayDuration
        {
            get { return playDuration; }
            set { playDuration = value; }
        }

        [XmlElement("id", Form = XmlSchemaForm.Unqualified)]
        public long Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}