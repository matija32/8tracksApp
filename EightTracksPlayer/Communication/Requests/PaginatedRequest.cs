namespace EightTracksPlayer.Communication.Requests
{
    public abstract class PaginatedRequest : RequestBase
    {
        protected PaginatedRequest(string baseUri, string apiKey, int pageNumber, int pageSize) : base(baseUri, apiKey)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }

        public abstract PaginatedRequest GetNextPageRequest();
    }
}