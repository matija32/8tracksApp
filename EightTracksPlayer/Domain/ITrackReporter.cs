using EightTracksPlayer.Domain.Entities;

namespace EightTracksPlayer.Domain
{
    public interface ITrackReporter
    {
        void Report(Track track, int mixId);
    }
}