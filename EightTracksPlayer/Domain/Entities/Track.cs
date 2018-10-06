using System;
using System.Collections.Generic;
using EightTracksPlayer.Communication.Responses.SupportingElements;
using ReactiveUI;

namespace EightTracksPlayer.Domain.Entities
{
    public class Track : ICloneable
    {
        private double duration;
        private ReplaySubject<double> durationObservable = new ReplaySubject<double>(1);

        public Track(TrackElement trackElement, bool isLast, bool isSkipAllowed)
        {
            Performer = trackElement.Performer;
            Name = trackElement.Name;
            Uri = trackElement.Url;
            Duration = 0;
            IsLast = isLast;
            Id = trackElement.Id;
            IsSkipAllowed = isSkipAllowed;
        }

        

        private Track()
        {
        }

        public string Name { get; set; }
        public string Performer { get; set; }
        public string Uri { get; set; }
        public bool IsLast { get; set; }
        public long Id { get; set; }

        public double Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                durationObservable.OnNext(duration);
            }
        }

        public IObservable<double> DurationObservable
        {
            get { return durationObservable; }
        }

        public bool IsReported { get; set; }

        public bool IsSkipAllowed { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            return new Track()
                       {
                           Name = this.Name,
                           Performer = this.Performer,
                           Uri = this.Uri,
                           IsLast = this.IsLast
                       };
        }

        #endregion
    }
}