using System.Xml.Schema;
using System.Xml.Serialization;
using EightTracksPlayer.Communication.Responses.SupportingElements;

namespace EightTracksPlayer.Communication.Responses
{
    public interface IResponse
    {
        /// <remarks/>
        [XmlAttribute("errors", Form = XmlSchemaForm.Unqualified)]
        ErrorElement ErrorsElement { get; set; }

        bool HasErrors { get; }

        /// <remarks/>
        [XmlAttribute("notices", Form = XmlSchemaForm.Unqualified)]
        NoticeElement NoticesElement { get; set; }

        bool HasNotices { get; }

        [XmlArrayAttribute("api-warning", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItemAttribute("api-warning", typeof (ApiWarningElement), Form = XmlSchemaForm.Unqualified,
            IsNullable = false)]
        ApiWarningElement[] ApiWarningsElement { get; set; }

        bool HasApiWarnings { get; }

        [XmlElementAttribute("status", Form = XmlSchemaForm.Unqualified)]
        string StatusString { get; set; }

        [XmlElement("api-version", Form = XmlSchemaForm.Unqualified)]
        int ApiVersion { get; set; }

        ResponseStatusEnum Status { get; set; }
    }
}