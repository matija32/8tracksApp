using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Communication.Responses;
using EightTracksPlayer.Communication.Serialization;
using EightTracksTests.TestUtils;
using NUnit.Framework;
using StoryQ;
using StoryQ.Formatting.Parameters;

namespace EightTracksTests
{
    [TestFixture]
    public class ResponseDeserializersMappingTests
    {
        private AggregatedResponseDeserializer aggregatedResponseDeserializer = new AggregatedResponseDeserializer();
        
        private Feature playingAmix = Features.PlayingAMix;
        private Feature authenticateAnUser = Features.AuthenticatingAnUser;
        private Feature favoritingItemsInTheMediaLibrary = Features.FavoritingItemsInTheMediaLibrary;

        private XmlSerializer serializer;
        private XmlWriter stream;

        [SetUp]
        public void ResetStream()
        {
            StringWriter stringWriter = new StringWriter(new StringBuilder());
            stream = XmlWriter.Create(stringWriter);
        }

        [Test]
        public void TestRetrievesMixesRequestDeserializerForMixesRequest()
        {
            playingAmix.
                WithScenario("selecting a deserializer for the request for mixes").Tag(Tags.XmlDeserialization).Tag(Tags.Infrastructure)
                .Given(AnyConditions)
                .When(TheApplicationRequestsASerializerThatMatches_, typeof (MixesRequest))
                .Then(TheSerializerIsObtained)
                    .And(TheSerializerWillSupport_, typeof(MixesResponse))
                .ExecuteWithReport();
        }

        [Test]
        public void TestRetrievesPlaySongResponseRequestDeserializerForPlayMixRequest()
        {
            playingAmix.
                WithScenario("selecting a deserializer for the request for playing a certain mix").Tag(Tags.XmlDeserialization).Tag(Tags.Infrastructure)
                .Given(AnyConditions)
                .When(TheApplicationRequestsASerializerThatMatches_, typeof(PlayMixRequest))
                .Then(TheSerializerIsObtained)
                    .And(TheSerializerWillSupport_, typeof(PlaySongResponse))
                .ExecuteWithReport();
        }

        [Test]
        public void TestRetrievesPlaySongResponseRequestDeserializerForNextSongRequest()
        {
            playingAmix.
                WithScenario("selecting a deserializer for the request for playing the next song in a mix").Tag(Tags.XmlDeserialization).Tag(Tags.Infrastructure)
                .Given(AnyConditions)
                .When(TheApplicationRequestsASerializerThatMatches_, typeof(NextSongRequest))
                .Then(TheSerializerIsObtained)
                    .And(TheSerializerWillSupport_, typeof(PlaySongResponse))
                .ExecuteWithReport();
        }

        [Test]
        public void TestRetrievesNextMixResponseRequestDeserializerForNextMixRequest()
        {
            playingAmix.
                WithScenario("selecting a deserializer for the request for the next mix in a mix set").Tag(Tags.XmlDeserialization).Tag(Tags.Infrastructure)
                .Given(AnyConditions)
                .When(TheApplicationRequestsASerializerThatMatches_, typeof(NextMixRequest))
                .Then(TheSerializerIsObtained)
                    .And(TheSerializerWillSupport_, typeof(NextMixResponse))
                .ExecuteWithReport();
        }

        [Test]
        public void TestRetrievesNewPlayTokenResponseDeserializerForNewPlayTokenRequest()
        {
            playingAmix.
                WithScenario("selecting a deserializer for the mixes request").Tag(Tags.XmlDeserialization).Tag(Tags.Infrastructure)
                .Given(AnyConditions)
                .When(TheApplicationRequestsASerializerThatMatches_, typeof(NewPlayTokenRequest))
                .Then(TheSerializerIsObtained)
                    .And(TheSerializerWillSupport_, typeof(NewPlayTokenResponse))
                .ExecuteWithReport();
        }

        [Test]
        public void TestRetrievesLoginResponseDeserializerForLoginRequest()
        {
            authenticateAnUser
                .WithScenario("selecting a deserializer for the login request").Tag(Tags.XmlDeserialization).Tag(Tags.Infrastructure)
                .Given(AnyConditions)
                .When(TheApplicationRequestsASerializerThatMatches_, typeof(LoginRequest))
                .Then(TheSerializerIsObtained)
                    .And(TheSerializerWillSupport_, typeof(LoginResponse))
                .ExecuteWithReport();
        }

        [Test]
        public void TestRetrievesToggleMixLikeResponseDeserializerForToggleMixLikeRequest()
        {
            favoritingItemsInTheMediaLibrary
                .WithScenario("selecting a deserializer for toggle mix like request").Tag(Tags.XmlDeserialization).Tag(
                    Tags.Infrastructure)
                .Given(AnyConditions)
                .When(TheApplicationRequestsASerializerThatMatches_, typeof(ToggleMixLikeRequest))
                .Then(TheSerializerIsObtained)
                    .And(TheSerializerWillSupport_, typeof(ToggleMixLikeResponse))
                .ExecuteWithReport();
        }

        private void TheSerializerIsObtained()
        {
            Assert.IsNotNull(serializer);
        }

        private void TheSerializerWillSupport_([TypeParameterFormat] Type responseType)
        {
            TestDelegate testDelegate = () => serializer.Serialize(stream, Activator.CreateInstance(responseType));
            Assert.DoesNotThrow(testDelegate);
        }

        private void TheApplicationRequestsASerializerThatMatches_([TypeParameterFormat] Type requestType)
        {
            serializer = aggregatedResponseDeserializer.GetResponseDeserializer(requestType);
        }

        private void AnyConditions()
        {
            
        }
    }
}