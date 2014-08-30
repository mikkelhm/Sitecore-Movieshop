using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;

namespace MovieShop.BusinessLayer.Search
{
    public class SearchHelper
    {
        public static List<MovieSearchResultItem> SearchMovies()
        {
            var indexName = "movieshop_master";
            using (var context = ContentSearchManager.GetIndex(indexName).CreateSearchContext())
            {
                var query = context.GetQueryable<MovieSearchResultItem>()
                    .OrderByDescending(x => x.VoteAverage)
                    .Take(10);
                return query.ToList();
            }

        } 
    }
}
