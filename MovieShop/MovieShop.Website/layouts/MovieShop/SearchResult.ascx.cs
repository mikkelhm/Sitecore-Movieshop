using System;
using MovieShop.BusinessLayer.Search;

namespace MovieShop.Website.layouts.MovieShop
{

    public partial class SearchResult : System.Web.UI.UserControl
    {
        private void Page_Load(object sender, EventArgs e)
        {
            var result = SearchHelper.SearchMovies();
            lvMovies.DataSource = result;
            lvMovies.DataBind();
        }
    }
}