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
    public class SetElement
    {
        private bool atBeginning;
        private bool atEnd;
        private bool atLastTrack;
        private bool skipAllowed;
        private TrackElement trackElement = new TrackElement();

        [XmlElement("at-beginning", Form = XmlSchemaForm.Unqualified)]
        public bool AtBeginning
        {
            get { return atBeginning; }
            set { atBeginning = value; }
        }

        [XmlElement("at-end", Form = XmlSchemaForm.Unqualified)]
        public bool AtEnd
        {
            get { return atEnd; }
            set { atEnd = value; }
        }

        [XmlElement("at-last-track", Form = XmlSchemaForm.Unqualified)]
        public bool AtLastTrack
        {
            get { return atLastTrack; }
            set { atLastTrack = value; }
        }

        [XmlElement("skip-allowed", Form = XmlSchemaForm.Unqualified)]
        public bool SkipAllowed
        {
            get { return skipAllowed; }
            set { skipAllowed = value; }
        }

        [XmlElement("track", Form = XmlSchemaForm.Unqualified)]
        public TrackElement TrackElement
        {
            get { return trackElement; }
            set { trackElement = value; }
        }
    }
}