using System.Collections.Generic;

namespace EightTracksPlayer.Domain.Entities
{
    public class MixFilter
    {
        private string query = string.Empty;
        private List<string> tags = new List<string>();

        public string Query
        {
            get { return query; }
            set { query = value; }
        }

        public List<string> Tags
        {
            get { return tags; }
            set { tags = value; }
        }
    }
}