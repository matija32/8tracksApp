using System;
using StoryQ;

namespace EightTracksTests.TestUtils
{
    public class Features
    {
        private const string listener = "listener";

        public static Feature PlayingAMix
        {
            get
            {
                return new Story("Playing a mix")
                    .InOrderTo("enjoy the music of a certain mix")
                    .AsA(listener)
                    .IWant("to play a mix");
            }
        }

        public static Feature ControllingPlayback   
        {
            get
            {
                return new Story("Controlling playback")
                    .InOrderTo("avoid listening to the (parts of the) song I don't like")
                        .And("adjust the playback properties such as volume suiting my preferences and environment")
                        .And("initiate / stop the playback, etc.")
                    .AsA(listener)
                    .IWant("to have a control over the playback by doing fast-forward, rewind, play/stop/pause, increase/decrease volume...");
            }
        }

        public static Feature ContinuousAudioPlayback
        {
            get
            {
                return new Story("Maintaining continuous audio playback")
                    .InOrderTo("avoid manually switching to the next track")
                        .And("to enjoy music with the least amount of interaction")
                    .AsA(listener)
                    .IWant("the system to automaticallly start playing the next track, once a track is over")
                        .And("to play a similar mix, once a mix ends");

            }
        }

        public static Feature AuthenticatingAnUser
        {
            get
            {
                return new Story("Authenticating an user")
                    .InOrderTo("keep my settings and preferences protected and synchronized with the 8tracks service")
                    .AsA(listener)
                    .IWant("to use the application with my user credentials");
            }
        }

        public static Feature BrowsingAvailableMixes
        {
            get
            {
                return new Story("Browsing available mixes")
                    .InOrderTo("select the mix I want to listen to")
                    .AsA(listener)
                    .IWant("the system to provide me with an overview of mixes in several categories");
            }
        }

        public static Feature FavoritingItemsInTheMediaLibrary
        {
            get
            {
                return new Story("Favoriting items in the media library")
                    .InOrderTo("find items in the media library easily")
                    .AsA(listener)
                    .IWant("to be able to star/unstar items in the media library");
            }
        }

        public static Feature DownloadingMediaContent
        {
            get
            {
                return new Story("Downloading media content")
                    .InOrderTo("enjoy the music offline")
                    .AsA(listener)
                    .IWant("I want to save the songs or a whole playlist to external memory");
            }
        }
    }
}