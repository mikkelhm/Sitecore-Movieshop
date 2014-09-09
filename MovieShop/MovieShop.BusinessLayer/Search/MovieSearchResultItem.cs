using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace MovieShop.BusinessLayer.Search
{
    public class MovieSearchResultItem:SearchResultItem
    {
        [IndexField("original title")]
        public string OriginalTitle { get; set; }

        [IndexField("title")]
        public string Title{ get; set; }

        [IndexField("tagline")]
        public string Tagline { get; set; }

        [IndexField("body")]
        public string Body { get; set; }

        [IndexField("vote average")]
        public double VoteAverage { get; set; }

        [IndexField("vote count")]
        public int VoteCount { get; set; }

        [IndexField("genres")]
        public List<ID> Genres { get; set; }

        [IndexField("tmdb id")]
        public int TmdbId { get; set; }

        [IndexField("imdb id")]
        public string ImdbId { get; set; }

        [IndexField("runtime")]
        public int Runtime { get; set; }

        [IndexField("status")]
        public string Status{ get; set; }

        [IndexField("release date")]
        public DateTime ReleaseDate { get; set; }

        [IndexField("releaseyear")]
        public int ReleaseYear { get; set; }

        [IndexField("releasedecade")]
        public int ReleaseDecade { get; set; }

        [IndexField("production companies")]
        public List<ID> ProductionCompanies { get; set; }

        [IndexField("production contries")]
        public List<ID> ProductionContries { get; set; }

        [IndexField("imageurl")]
        public string ImageUrl { get; set; }

        [IndexField("url")]
        public string Url { get; set; }
    }
}
