using System;
using System.Linq;
using EightTracksPlayer.Communication;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Communication.Responses;
using EightTracksPlayer.Domain;
using EightTracksPlayer.Domain.Entities;
using log4net;
using ReactiveUI.Xaml;

namespace EightTracksPlayer
{
    public class TrackReporter : ITrackReporter
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private RequestFactory requestFactory = RequestFactory.Instance;
        private RequestExecutionAdapter requestExecutor;
        private double reportingMark = 30; //seconds
        private Mix currentMix = Mix.NoMixAvailable;

        public TrackReporter(IObservable<Mix> currentMixObservable, IObservable<double> currentPositionObservable)
            : this(currentMixObservable, currentPositionObservable, new HttpRequestExecutor())
        {
        }

        public double ReportingMark
        {
            get { return reportingMark; }
        }

        public TrackReporter(IObservable<Mix> currentMixObservable, IObservable<double> currentPositionObservable, IRequestExecutor requestExecutor)
        {
            this.requestExecutor = new RequestExecutionAdapter(requestExecutor);
            IObservable<bool> reportingMarkReached = currentPositionObservable
                .BufferWithCount(2, 1)
                .Select(positions => (positions[0] < reportingMark && positions[1] > reportingMark));

            currentMixObservable.Subscribe(mix =>
                                               {
                                                   currentMix = mix;
                                               });

            ReactiveAsyncCommand reportTrack = new ReactiveAsyncCommand();
            reportTrack.Subscribe(_ =>
            {
                int mixId = currentMix.MixId;
                Track track = currentMix.GetCurrentTrack();
                Report(track, mixId);
            });

            reportingMarkReached
                .Where(x => x) // we crossed the mark
                .Where(_ => !currentMix.GetCurrentTrack().IsReported) //it's not previously reported
                .Subscribe(_ => reportTrack.Execute(null));
        }

        public void Report(Track track, int mixId)
        {
            IRequest request = requestFactory.CreateReportMixRequest(mixId, track.Id);
            ResponseBase response = requestExecutor.ExecuteReportMixRequest(request);
            track.IsReported = response.Status == ResponseStatusEnum.Ok;

            if (!track.IsReported)
            {
                log.Error(String.Format("Unable to report track {0} - {1} to SoundExachange", track.Performer, track.Name));
            }
        }
    }
}