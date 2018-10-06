using System.Collections.Generic;
using EightTracksPlayer.Domain.Entities;

namespace EightTracksPlayer.Domain
{
    /// <summary>
    /// Media Library Browser obtains the information about the mixes based on various criteria (category, tags etc).
    /// </summary>
    public interface IMediaLibraryBrowser
    {
        string UserToken { get; set; }
        int PageSize { get; set; }
        BrowsingSessionData SessionData { get; }
        List<Mix> GetRecentlyMadeMixes(MixFilter mixFilter);
        List<Mix> GetHotMixes(MixFilter mixFilter);
        List<Mix> GetPopularMixes(MixFilter mixFilter);

        List<Mix> GetMoreMixes();

        void ToggleLike(Mix mix);

        List<Mix> GetLikedMixes(MixFilter mixFilter, string userSlug);
        List<Mix> GetFeedMixes(MixFilter mixFilter, string userSlug);
        List<Mix> GetHistoryMixes(MixFilter mixFilter, string userSlug);
        List<Mix> GetRecommendedMixes(MixFilter mixFilter, string userSlug);
    }
}