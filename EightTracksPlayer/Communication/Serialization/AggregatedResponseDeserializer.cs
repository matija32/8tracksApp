using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Communication.Responses;
using log4net;

namespace EightTracksPlayer.Communication.Serialization
{
    public class AggregatedResponseDeserializer
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); private Dictionary<Type, Type> messageMapping = new Dictionary<Type, Type>();
        private Dictionary<Type, XmlSerializer> serializers = new Dictionary<Type, XmlSerializer>();

        public AggregatedResponseDeserializer()
        {
            AddTypeMapping(typeof (MixesRequest), typeof (MixesResponse));
            AddTypeMapping(typeof (NewPlayTokenRequest), typeof (NewPlayTokenResponse));
            AddTypeMapping(typeof (PlayMixRequest), typeof (PlaySongResponse));
            AddTypeMapping(typeof (NextSongRequest), typeof (PlaySongResponse));
            AddTypeMapping(typeof (LoginRequest), typeof (LoginResponse));
            AddTypeMapping(typeof (ToggleMixLikeRequest), typeof (ToggleMixLikeResponse));
            AddTypeMapping(typeof (NextMixRequest), typeof (NextMixResponse));
            AddTypeMapping(typeof (ReportMixRequest), typeof(ResponseBase));

            foreach (XmlSerializer xmlSerializer in serializers.Values)
            {
                xmlSerializer.UnknownAttribute += (sender, args) => {
                    try
                    {
                        serializer_UnknownAttribute(sender, args);
                    }
                    catch (Exception e)
                    {
                        log.Error("Deserialization error", e);
                    }

                };
                xmlSerializer.UnknownElement += (sender, args) =>
                {
                    try
                    {
                        serializer_UnknownElement(sender, args);
                    }
                    catch (Exception e)
                    {
                        log.Error("Deserialization error", e);
                    }

                };
                xmlSerializer.UnknownNode += (sender, args) => {
                    try
                    {
                        serializer_UnknownNode(sender, args);
                    }
                    catch (Exception e)
                    {
                        log.Error("Deserialization error", e);
                    }

                };;
            }
        }

        private void AddTypeMapping(Type requestType, Type responseType)
        {
            serializers.Add(requestType, new XmlSerializer(responseType));
            messageMapping.Add(requestType, responseType);
        }


        public IResponse Deserialize(Type requestType, string xmlResponse)
        {
            try
            {
                XmlSerializer serializer = GetResponseDeserializer(requestType);
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlResponse)))
                {
                    return (IResponse) serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                Type responseType = messageMapping[requestType];
                throw new XmlException(
                    String.Format("Unable to deserialize {0} from the response {1}", responseType.Name, xmlResponse), e);
            }
        }

        public IResponse Deserialize(Type requestType, HttpWebResponse httpWebResponse)
        {
            StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream());
            string stringXmlResponse = reader.ReadToEnd();
            reader.Close();
            return Deserialize(requestType, stringXmlResponse);
        }

        public XmlSerializer GetResponseDeserializer(Type requestType)
        {
            return serializers[requestType];
        }


        private void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            // if the nil attribute is recognized as unknown, just skip it.
            // it's the uncanny 8tracks xml syntax...
            if (e.LocalName != "nil")
            {
                throw new SerializationException(
                    String.Format(
                        "Unknown node detected while deserializing a response at {0}:{1}. Detected node is {2}={3}.",
                        e.LineNumber, e.LinePosition, e.LocalName, e.Text));
            }
        }

        private void serializer_UnknownElement(object sender, XmlElementEventArgs e)
        {
            if (e.Element.LocalName != "nil")
            {
                throw new SerializationException(
                    String.Format(
                        "Unknown element detected while deserializing a response at {0}:{1}, expected element = {2}. Detected element is {3}={4}.",
                        e.LineNumber, e.LinePosition, e.ExpectedElements, e.Element.LocalName, e.Element.InnerXml));
            }
        }

        private void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            if (e.Attr.LocalName != "nil")
            {
                throw new SerializationException(
                    String.Format(
                        "Unknown attribute detected while deserializing a response at {0}:{1}, expected attributes = {2}. Detected attribute is {3}={4}.",
                        e.LineNumber, e.LinePosition, e.ExpectedAttributes, e.Attr.LocalName, e.Attr.Value));
            }
        }
    }
}