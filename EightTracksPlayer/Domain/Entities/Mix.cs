using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using EightTracksPlayer.Communication.Responses.SupportingElements;
using EightTracksPlayer.Utils;
using ReactiveUI;

namespace EightTracksPlayer.Domain.Entities
{
    public class Mix : ReactiveValidatedObject
    {
        public static Mix NoMixAvailable = new Mix()
                                               {
                                                   Author = "No mix author available",
                                                   Name = "No mix name available",
                                                   CoverUri = @"..\..\Resources\NoImageAvailable.jpg",
                                                   TagListCache = new List<string>(),
                                                   Description = "No mix description available",
                                                   LikesCount = 0,
                                                   PlaysCount = 0,
                                                   TotalNumberOfTracks = 0,
                                                   TotalDuration = TimeSpan.Zero,
                                                   LikedByCurrentUser = false,
                                                   RestfulUri = "http://8tracks.com/"
                                               };

        private int currentTrackIndex = -1;
        private ReplaySubject<int> currentTrackIndexObservable = new ReplaySubject<int>(1);
        private bool likedByCurrentUser;
        private ReplaySubject<bool> likedByCurrentUserObservable = new ReplaySubject<bool>(1);
        private DispatchedReactiveCollection<Track> tracks = new DispatchedReactiveCollection<Track>();
        //private ObservableAsPropertyHelper<int> _TotalNumberOfTracks;

        private Mix()
        {
        }

        public Mix(MixElement mixElement, long setId)
        {
            Author = mixElement.UserElement.Slug;
            FirstPublishedAt = mixElement.FirstPublishedAt;
            Name = mixElement.Name;
            MixId = mixElement.Id;
            CoverUri = mixElement.CoverUrls.sq250;
            LikedByCurrentUser = mixElement.LikedByCurrentUser;
            LikesCount = mixElement.LikesCount;
            PlaysCount = mixElement.PlaysCount;
            TagListCache = ExtractMixTags(mixElement.TagListCache);
            SetId = setId;

            // removing empty new lines
            Description = Regex.Replace(mixElement.Description, "(\\r|\\n)+", Environment.NewLine);
            RestfulUri = mixElement.RestfulUrl;

//            _TotalNumberOfTracks = tracks
//                .CollectionCountChanged
//                .ToProperty(this, x => x.TotalNumberOfTracks);
            TotalNumberOfTracks = mixElement.TracksCount;
            TotalDuration = TimeSpan.FromSeconds(mixElement.Duration);
        }

        public TimeSpan TotalDuration { get; set; }
        public IObservable<int> CurrentTrackIndexObservable
        {
            get { return currentTrackIndexObservable; }
        }

        public IObservable<bool> LikedByCurrentUserObservable
        {
            get { return likedByCurrentUserObservable; }
        }

        [Required]
        public ReactiveCollection<Track> Tracks
        {
            get { return tracks; }
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public int MixId { get; set; }

        [Required]
        public long SetId { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public DateTime FirstPublishedAt { get; set; }

        [Required]
        public string CoverUri { get; set; }

        [Required]
        public int PlaysCount { get; set; }

        [Required]
        public int LikesCount { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string RestfulUri { get; set; }

        [Required]
        public List<string> TagListCache { get; set; }

        [Required]
        public int CurrentTrackIndex
        {
            get { return currentTrackIndex; }
            set
            {
                currentTrackIndex = value;
                currentTrackIndexObservable.OnNext(currentTrackIndex);
            }
        }

        [Required]
        public bool LikedByCurrentUser
        {
            get { return likedByCurrentUser; }
            set
            {
                likedByCurrentUser = value;
                likedByCurrentUserObservable.OnNext(likedByCurrentUser);
            }
        }

        [Required]
        public int TotalNumberOfTracks
        {
            //get { return _TotalNumberOfTracks.Value; }
            get; set;
        }

        private List<string> ExtractMixTags(string tagListCache)
        {
            if (String.IsNullOrEmpty(tagListCache))
            {
                return new List<string>();
            }

            string[] tags = tagListCache.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
            List<string> decodedTagString = tags.Select(HttpUtility.HtmlDecode).ToList();
            return decodedTagString;
        }

        public void AddTrack(Track track)
        {
            tracks.Add(track);
        }

        public Track GetTrack(int trackIndex)
        {
            return tracks[trackIndex];
        }

        public Track GetCurrentTrack()
        {
            return GetTrack(CurrentTrackIndex);
        }

        public void ResetTrackIndex()
        {
            CurrentTrackIndex = 0;
        }

        public void MoveToNextTrack()
        {
            CurrentTrackIndex++;
        }

        public void GoToTrack(int trackIndex)
        {
            CurrentTrackIndex = trackIndex;
        }

        public void GoToTrack(Track track)
        {
            CurrentTrackIndex = tracks.IndexOf(track);
        }
    }
}