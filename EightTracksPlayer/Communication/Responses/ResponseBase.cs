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
    public class ResponseBase : IResponse
    {
        private int apiVersionField;
        private ApiWarningElement[] apiwarningField = new ApiWarningElement[0];

        private ErrorElement errorsElementField = new ErrorElement();
        private NoticeElement noticesElementField = new NoticeElement();

        private string statusField;

        #region IResponse Members

        /// <remarks/>
        [XmlElementAttribute("errors", Form = XmlSchemaForm.Unqualified)]
        public ErrorElement ErrorsElement
        {
            get { return errorsElementField; }
            set { errorsElementField = value; }
        }

        public bool HasErrors
        {
            get { return ErrorsElement != null && !String.IsNullOrEmpty(ErrorsElement.Value); }
        }

        /// <remarks/>
        [XmlElementAttribute("notices", Form = XmlSchemaForm.Unqualified)]
        public NoticeElement NoticesElement
        {
            get { return noticesElementField; }
            set { noticesElementField = value; }
        }

        public bool HasNotices
        {
            get { return NoticesElement != null && !String.IsNullOrEmpty(ErrorsElement.Value); }
        }

        /// <remarks/>
        [XmlArrayAttribute("api-warning", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItemAttribute("api-warning", typeof (ApiWarningElement), Form = XmlSchemaForm.Unqualified,
            IsNullable = true)]
        public ApiWarningElement[] ApiWarningsElement
        {
            get { return apiwarningField; }
            set { apiwarningField = value; }
        }

        public bool HasApiWarnings
        {
            get { return ApiWarningsElement != null && ApiWarningsElement.Length > 0; }
        }

        [XmlElementAttribute("status", Form = XmlSchemaForm.Unqualified)]
        public string StatusString
        {
            get { return statusField; }
            set { statusField = value; }
        }

        [XmlElement("api-version", Form = XmlSchemaForm.Unqualified)]
        public int ApiVersion
        {
            get { return apiVersionField; }
            set { apiVersionField = value; }
        }

        public ResponseStatusEnum Status
        {
            get { return ResponseStatusEncoder.Decode(statusField); }
            set { statusField = ResponseStatusEncoder.Encode(value); }
        }

        #endregion
    }
}