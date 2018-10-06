using System;
using System.IO;
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
    public class ResponseDeserializersTests
    {

        private string responseString;

        private AggregatedResponseDeserializer deserializer;

        private NextMixResponse nextMixResponse;
        private NewPlayTokenResponse newPlayTokenResponse;
        private MixesResponse mixesResponse;
        private PlaySongResponse playSongResponse;
        private LoginResponse loginResponse;

        private string properMixesXmlResponse;
        private string properNewPlayTokenResponse;
        private string properPlaySongResponse;
        private string properPlaySongResponseNoYear;
        private string properSuccessfulLoginResponse;
        private string properUnsuccessfulLoginResponse;
        private string properLikedMixesXmlResponse;
        private string properLikedMixesXmlResponse2;
        private string properEndOfMixResponse;

        private Feature playingAMixFeature;
        private Feature authenticatingAnUser;
        
        private Feature continuousAudioPlayback;

        private Exception exception;

        private string xmlDeserializationTag;
        private string infrastructureTag;
        private string properNextMixResponse;
        private ToggleMixLikeResponse toggleMixLikeResponse;
        private string properToggleLikedMixResponse;

        private const string randomXmlResponse = 
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<response>" +
                    "<status>Whatever</status>" +
                    "<text>This is a random XML response.</text>" +
                "</response>";

        [TestFixtureSetUp]
        public void Initialize()
        {
            playingAMixFeature = Features.PlayingAMix;
            authenticatingAnUser = Features.AuthenticatingAnUser;
            continuousAudioPlayback = Features.ContinuousAudioPlayback;
            xmlDeserializationTag = Tags.XmlDeserialization;
            infrastructureTag = Tags.Infrastructure;

            StreamReader streamReader = new StreamReader(@"..\..\..\Data\Sample XML responses\sampleMixesResponse.xml");
            properMixesXmlResponse = streamReader.ReadToEnd();
            streamReader.Close();

            streamReader = new StreamReader(@"..\..\..\Data\Sample XML responses\sampleNewPlayTokenResponse.xml");
            properNewPlayTokenResponse = streamReader.ReadToEnd();
            streamReader.Close();

            streamReader = new StreamReader(@"..\..\..\Data\Sample XML responses\samplePlaySongResponse.xml");
            properPlaySongResponse = streamReader.ReadToEnd();
            streamReader.Close();

            streamReader = new StreamReader(@"..\..\..\Data\Sample XML responses\samplePlaySongResponse2.xml");
            properPlaySongResponseNoYear = streamReader.ReadToEnd();
            streamReader.Close();

            streamReader = new StreamReader(@"..\..\..\Data\Sample XML responses\sampleUnsuccessfulLoginResponse.xml");
            properUnsuccessfulLoginResponse = streamReader.ReadToEnd();
            streamReader.Close();

            streamReader = new StreamReader(@"..\..\..\Data\Sample XML responses\sampleSuccessfulLoginResponse.xml");
            properSuccessfulLoginResponse = streamReader.ReadToEnd();
            streamReader.Close();

            streamReader = new StreamReader(@"..\..\..\Data\Sample XML responses\sampleLikedMixesResponse.xml");
            properLikedMixesXmlResponse = streamReader.ReadToEnd();
            streamReader.Close();

            streamReader = new StreamReader(@"..\..\..\Data\Sample XML responses\sampleLikedMixesResponse2.xml");
            properLikedMixesXmlResponse2 = streamReader.ReadToEnd();
            streamReader.Close();

            streamReader = new StreamReader(@"..\..\..\Data\Sample XML responses\sampleEndOfMixResponse.xml");
            properEndOfMixResponse = streamReader.ReadToEnd();
            streamReader.Close();

            streamReader = new StreamReader(@"..\..\..\Data\Sample XML responses\sampleNextMixResponse.xml");
            properNextMixResponse = streamReader.ReadToEnd();
            streamReader.Close();

            streamReader = new StreamReader(@"..\..\..\Data\Sample XML responses\sampleMixLikedResponse.xml");
            properToggleLikedMixResponse = streamReader.ReadToEnd();
            streamReader.Close();
            
        }

        [SetUp]
        public void ResetFields()
        {
            responseString = null;
            newPlayTokenResponse = null;
            mixesResponse = null;
            playSongResponse = null;
            loginResponse = null;
            exception = null;

            deserializer = new AggregatedResponseDeserializer();
        }

        [Test]
        public void TestProperlyDeserializeValidMixesResponse()
        {
            playingAMixFeature
                .WithScenario("deserializing a valid XML response containing a list of mixes").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedValidXmlMixesResponse)
                    .When(TheApplicationDeserializesTheMixesResponse)
                    .Then(TheMixesResponse_Created, true)
                    .And(ADeserializationException_Thrown, false)
                    .ExecuteWithReport();
        }

        [Test]
        public void TestProperlyDeserializeValidMixesResponseContainingLikedMixes()
        {
            playingAMixFeature
                .WithScenario("deserializing a valid XML response containing a list of liked mixes").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedValidXmlMixesResponseContainingLikedMixes)
                    .When(TheApplicationDeserializesTheMixesResponse)
                    .Then(TheMixesResponse_Created, true)
                    .And(ADeserializationException_Thrown, false)
                    .ExecuteWithReport();
        }

        [Test]
        public void TestProperlyDeserializeValidMixesResponseContainingOnlyOneLikedMixes()
        {
            playingAMixFeature
                .WithScenario("deserializing a valid XML response containing only one liked mixes").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedValidXmlMixesResponseContainingOnlyOneLikedMixes)
                    .When(TheApplicationDeserializesTheMixesResponse)
                    .Then(TheMixesResponse_Created, true)
                    .And(ADeserializationException_Thrown, false)
                    .ExecuteWithReport();
        }

        private void TheApplicationHasReceivedValidXmlMixesResponseContainingOnlyOneLikedMixes()
        {
            responseString = properLikedMixesXmlResponse2;
        }

        private void TheApplicationHasReceivedValidXmlMixesResponseContainingLikedMixes()
        {
            responseString = properLikedMixesXmlResponse;
        }

        [Test]
        public void TestFailsToDeserializeAnInvalidMixesResponse()
        {
            playingAMixFeature
                .WithScenario("deserializing an invalid XML response - one that is not containing a list of mixes").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedAnInvalidXmlResponse)
                    .When(TheApplicationDeserializesTheMixesResponse)
                    .Then(TheMixesResponse_Created, false)
                    .And(ADeserializationException_Thrown, true)
                    .ExecuteWithReport();
        }

        [Test]
        public void TestFailsToDeserializeANonXmlAsMixesResponse()
        {
            playingAMixFeature
                .WithScenario("deserializing a non-XML response").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedANonXmlResponse)
                    .When(TheApplicationDeserializesTheMixesResponse)
                    .Then(TheMixesResponse_Created, false)
                    .And(ADeserializationException_Thrown, true)
                    .ExecuteWithReport();
        }

        [Test]
        public void TestProperlyDeserializeValidNewPlayTokenResponse()
        {
            playingAMixFeature
                .WithScenario("deserializing a valid XML response containing a token for playing the mixes").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedValidNewPlayTokenXmlResponse)
                    .When(TheApplicationDeserializesTheNewPlayTokenResponse)
                    .Then(TheNewPlayTokenResponse_Created, true)
                    .And(ADeserializationException_Thrown, false)
                    .ExecuteWithReport();      
        }

        [Test]
        public void TestFailsToDeserializeAnInvalidNewPlayTokenResponse()
        {
            playingAMixFeature
                .WithScenario("deserializing an invalid XML response - one that is not containing a token for playing the mixes").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedAnInvalidXmlResponse)
                    .When(TheApplicationDeserializesTheNewPlayTokenResponse)
                    .Then(TheNewPlayTokenResponse_Created, false)
                    .And(ADeserializationException_Thrown, true)
                    .ExecuteWithReport();
        }

        [Test]
        public void TestFailsToDeserializeANonXmlAsNewPlayTokenResponse()
        {
            playingAMixFeature
                .WithScenario("deserializing a non-XML response").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedANonXmlResponse)
                    .When(TheApplicationDeserializesTheNewPlayTokenResponse)
                    .Then(TheNewPlayTokenResponse_Created, false)
                    .And(ADeserializationException_Thrown, true)
                    .ExecuteWithReport();
        }

        [Test]
        public void TestProperlyDeserializeValidPlaySongResponse()
        {
            playingAMixFeature
                .WithScenario("deserializing a valid XML response for playing a song").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedValidPlaySongXmlResponse)
                    .When(TheApplicationDeserializesThePlaySongResponse)
                    .Then(ThePlaySongResponse_Created, true)
                    .And(ADeserializationException_Thrown, false)
                    .ExecuteWithReport();
        }

        [Test]
        public void TestProperlyDeserializeValidPlaySongResponseThatDoesNotContainYear()
        {
            playingAMixFeature
                .WithScenario("deserializing a valid XML response for playing a song, year value missing").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedValidPlaySongXmlResponseWithNoYear)
                    .When(TheApplicationDeserializesThePlaySongResponse)
                    .Then(ThePlaySongResponse_Created, true)
                        .And(ADeserializationException_Thrown, false)

                    .ExecuteWithReport();
        }

        [Test]
        public void TestProperlyDeserializeValidPlaySongResponseForEndOfMix()
        {
            playingAMixFeature
                .WithScenario("deserializing a valid XML response for playing a song, when the end of mix is reached").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedValidPlaySongXmlResponseEndOfMix)
                    .When(TheApplicationDeserializesThePlaySongResponse)
                    .Then(ThePlaySongResponse_Created, true)
                        .And(ADeserializationException_Thrown, false)

                    .ExecuteWithReport();
        }

        private void TheApplicationHasReceivedValidPlaySongXmlResponseEndOfMix()
        {
            responseString = properEndOfMixResponse;
        }

        private void TheApplicationHasReceivedValidPlaySongXmlResponseWithNoYear()
        {
            responseString = properPlaySongResponseNoYear;
        }


        private void TheApplicationHasReceivedValidPlaySongXmlResponse()
        {
            responseString = properPlaySongResponse;
        }

        [Test]
        public void TestFailsToDeserializeAnInvalidPlaySongResponse()
        {
            playingAMixFeature
                .WithScenario("deserializing an invalid XML response - one that is not a response for playing a song").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedAnInvalidXmlResponse)
                    .When(TheApplicationDeserializesThePlaySongResponse)
                    .Then(ThePlaySongResponse_Created, false)
                    .And(ADeserializationException_Thrown, true)
                    .ExecuteWithReport();
        }

        [Test]
        public void TestFailsToDeserializeANonXmlAsPlaySongResponse()
        {
            playingAMixFeature
                .WithScenario("deserializing a non-XML response").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedANonXmlResponse)
                    .When(TheApplicationDeserializesThePlaySongResponse)
                    .Then(ThePlaySongResponse_Created, false)
                    .And(ADeserializationException_Thrown, true)
                    .ExecuteWithReport();
        }

        private void ThePlaySongResponse_Created([BooleanParameterFormat("should be", "should not be")] bool isCreated)
        {
            Assert.That(playSongResponse != null, Is.EqualTo(isCreated));
        }

        private void TheApplicationDeserializesThePlaySongResponse()
        {
            try
            {
                playSongResponse = (PlaySongResponse)deserializer.Deserialize(typeof(PlayMixRequest), responseString);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        private void TheApplicationHasReceivedANonXmlResponse()
        {
            responseString = "random bunch of characters";
        }

        private void TheMixesResponse_Created([BooleanParameterFormat("should be", "should not be")]bool isCreated)
        {
            Assert.That(mixesResponse != null, Is.EqualTo(isCreated));
        }

        private void TheNewPlayTokenResponse_Created([BooleanParameterFormat("should be", "should not be")]bool isCreated)
        {
            Assert.That(newPlayTokenResponse != null, Is.EqualTo(isCreated));
        }

        private void TheApplicationDeserializesTheNewPlayTokenResponse()
        {
            try
            {
                newPlayTokenResponse = (NewPlayTokenResponse)deserializer.Deserialize(typeof(NewPlayTokenRequest), responseString);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        private void TheApplicationHasReceivedValidNewPlayTokenXmlResponse()
        {
            responseString = properNewPlayTokenResponse;
        }

        private void TheApplicationHasReceivedAnInvalidXmlResponse()
        {
            responseString = randomXmlResponse;
        }

        private void ADeserializationException_Thrown([BooleanParameterFormat("should be", "should not be")]bool isThrown)
        {
            Assert.That(exception != null, Is.EqualTo(isThrown));
        }

        private void TheApplicationDeserializesTheMixesResponse()
        {
            try
            {
                mixesResponse = (MixesResponse)deserializer.Deserialize(typeof(MixesRequest), responseString);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            
        }

        private void TheApplicationHasReceivedValidXmlMixesResponse()
        {
            responseString = properMixesXmlResponse;
        }

        [Test]
        public void TestProperlyDeserializeValidSuccessfulLoginResponse()
        {
            authenticatingAnUser
                .WithScenario("deserializing a valid XML response about successful user login").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedValidXmlSuccessfulLoginResponse)
                    .When(TheApplicationDeserializesTheLoginResponse)
                    .Then(TheLoginResponse_Created, true)
                        .And(ADeserializationException_Thrown, false)
                    .ExecuteWithReport();
        }

        private void TheApplicationHasReceivedValidXmlSuccessfulLoginResponse()
        {
            responseString = properSuccessfulLoginResponse;
        }

        [Test]
        public void TestProperlyDeserializeValidUnsuccessfulLoginResponse()
        {
            authenticatingAnUser
                .WithScenario("deserializing a valid XML response about unsuccessful user login").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedValidXmlUnsuccessfulLoginResponse)
                    .When(TheApplicationDeserializesTheLoginResponse)
                    .Then(TheLoginResponse_Created, true)
                    .And(ADeserializationException_Thrown, false)
                    .ExecuteWithReport();
        }

        private void TheApplicationHasReceivedValidXmlUnsuccessfulLoginResponse()
        {
            responseString = properUnsuccessfulLoginResponse;
        }

        [Test]
        public void TestFailsToDeserializeAnInvalidLoginResponse()
        {
            authenticatingAnUser
                .WithScenario("deserializing an invalid XML response - one that is not a response for login").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedAnInvalidXmlResponse)
                    .When(TheApplicationDeserializesTheLoginResponse)
                    .Then(TheLoginResponse_Created, false)
                    .And(ADeserializationException_Thrown, true)
                    .ExecuteWithReport();
        }

        [Test]
        public void TestFailsToDeserializeANonXmlAsLoginResponse()
        {
            authenticatingAnUser
                .WithScenario("deserializing a non-XML response").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedANonXmlResponse)
                    .When(TheApplicationDeserializesTheLoginResponse)
                    .Then(TheLoginResponse_Created, false)
                    .And(ADeserializationException_Thrown, true)
                    .ExecuteWithReport();
        }

        private void TheLoginResponse_Created([BooleanParameterFormat("should be", "should not be")]bool isCreated)
        {
            Assert.That(loginResponse != null, Is.EqualTo(isCreated));
        }

        private void TheApplicationDeserializesTheLoginResponse()
        {
            try
            {
                loginResponse = (LoginResponse)deserializer.Deserialize(typeof(LoginRequest), responseString);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

        }

        [Test]
        public void TestProperlyDeserializeValidNextMixResponse()
        {
            playingAMixFeature
                .WithScenario("deserializing a valid XML response containing next mix in the set").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedValidNextMixXmlResponse)
                    .When(TheApplicationDeserializesTheNextMixResponse)
                    .Then(TheNextMixResponse_Created, true)
                    .And(ADeserializationException_Thrown, false)
                    .ExecuteWithReport();
        }

        private void TheApplicationHasReceivedValidNextMixXmlResponse()
        {
            responseString = properNextMixResponse;
        }

        [Test]
        public void TestFailsToDeserializeAnInvalidNextMixResponse()
        {
            playingAMixFeature
                .WithScenario("deserializing an invalid XML response - one that is not containing next mix in the set").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedAnInvalidXmlResponse)
                    .When(TheApplicationDeserializesTheNextMixResponse)
                    .Then(TheNextMixResponse_Created, false)
                    .And(ADeserializationException_Thrown, true)
                    .ExecuteWithReport();
        }

        [Test]
        public void TestFailsToDeserializeANonXmlAsNextMixResponse()
        {
            playingAMixFeature
                .WithScenario("deserializing a non-XML response").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedANonXmlResponse)
                    .When(TheApplicationDeserializesTheNextMixResponse)
                    .Then(TheNextMixResponse_Created, false)
                    .And(ADeserializationException_Thrown, true)
                    .ExecuteWithReport();
        }

        private void TheApplicationDeserializesTheNextMixResponse()
        {
            try
            {
                nextMixResponse = (NextMixResponse)deserializer.Deserialize(typeof(NextMixRequest), responseString);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

        }

        private void TheNextMixResponse_Created([BooleanParameterFormat("should be", "should not be")]bool isCreated)
        {
            Assert.That(nextMixResponse != null, Is.EqualTo(isCreated));
        }


        [Test]
        public void TestProperlyDeserializeToggleMixMixResponse()
        {
            continuousAudioPlayback
                .WithScenario("deserializing a valid XML response containing like-toggled mix").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedValidToggleLikedMixXmlResponse)
                    .When(TheApplicationDeserializesTheToggleLikedMixResponse)
                    .Then(TheToggleLikedMixResponse_Created, true)
                    .And(ADeserializationException_Thrown, false)
                    .ExecuteWithReport();
        }

        private void TheApplicationHasReceivedValidToggleLikedMixXmlResponse()
        {
            responseString = properToggleLikedMixResponse;
        }

        [Test]
        public void TestFailsToDeserializeAnInvalidToggleLikedMixResponse()
        {
            continuousAudioPlayback
                .WithScenario("deserializing an invalid XML response - one that is not containing like-toggled mix").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedAnInvalidXmlResponse)
                    .When(TheApplicationDeserializesTheToggleLikedMixResponse)
                    .Then(TheToggleLikedMixResponse_Created, false)
                    .And(ADeserializationException_Thrown, true)
                    .ExecuteWithReport();
        }

        [Test]
        public void TestFailsToDeserializeANonXmlAsToggleLikedMixResponse()
        {
            playingAMixFeature
                .WithScenario("deserializing a non-XML response").Tag(xmlDeserializationTag).Tag(infrastructureTag)
                    .Given(TheApplicationHasReceivedANonXmlResponse)
                    .When(TheApplicationDeserializesTheToggleLikedMixResponse)
                    .Then(TheToggleLikedMixResponse_Created, false)
                    .And(ADeserializationException_Thrown, true)
                    .ExecuteWithReport();
        }

        private void TheApplicationDeserializesTheToggleLikedMixResponse()
        {
            try
            {
                toggleMixLikeResponse = (ToggleMixLikeResponse)deserializer.Deserialize(typeof(ToggleMixLikeRequest), responseString);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

        }

        private void TheToggleLikedMixResponse_Created([BooleanParameterFormat("should be", "should not be")]bool isCreated)
        {
            Assert.That(toggleMixLikeResponse != null, Is.EqualTo(isCreated));
        }

    }
}
