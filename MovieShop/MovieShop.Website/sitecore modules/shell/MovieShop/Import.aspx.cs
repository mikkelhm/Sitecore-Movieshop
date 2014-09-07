using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;

namespace Website.sitecore_modules.Shell.Services
{
    public partial class Import : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (new SecurityDisabler())
            {
                Sitecore.Configuration.Settings.Indexing.Enabled = false;
                IndexCustodian.PauseIndexing();
                TMDbClient client = new TMDbClient("a7e29a282bb192663ce78f3574cf7a24");
                client.GetConfig();

                var master = Factory.GetDatabase("master");
                var movieContainer = master.GetItem(new ID("{46294884-8157-4828-9FD1-83B6E8EAFEA0}"));
                //movieContainer.DeleteChildren();
                var folderItem = master.GetItem(new ID("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}"));
                var movieTemplateItem = master.GetItem(new ID("{7D2F4318-029A-461B-854E-FD7CFB4C4F6A}"));
                var genresContainer = master.GetItem(new ID("{1A3786F4-68C0-47A5-A9D5-462765FE7C3E}"));
                var listItem = master.GetItem(new ID("{EB8C2C7A-FE2B-4E9A-8859-D4EBD9CD94D1}"));
                var productionCompaniesContainer = master.GetItem(new ID("{7A968F6F-7A21-4977-815C-581A9DC3EF5A}"));
                var productionCountriesContainer = master.GetItem(new ID("{2E421735-1C21-4648-8CC9-118B68B150C8}"));
                var pageID = 0;
                var totalPages = client.GetMovieList(MovieListType.TopRated, pageID).TotalPages;
                while (pageID <= totalPages)
                {
                    var movies = client.GetMovieList(MovieListType.TopRated, pageID);
                    pageID++;
                    foreach (var movie in movies.Results)
                    {
                        litMessage.Text += String.Format("{0} -> {1}<br/>", movie.OriginalTitle,
                            !movie.Title.Equals(movie.OriginalTitle) ? movie.Title : "");
                        var movieExt = client.GetMovie(movie.Id, MovieMethods.Credits);
                        
                        var sitecoreTitle = ItemUtil.ProposeValidItemName(movieExt.Title.Trim().ToLower());
                        var firstLetter = ItemUtil.ProposeValidItemName(sitecoreTitle.Substring(0, 1));

                        Item subFolder = movieContainer.Children.FirstOrDefault(x => x.Name == firstLetter);
                        if (subFolder == null)
                            subFolder = movieContainer.Add(firstLetter, new TemplateID(folderItem.ID));
                        if (subFolder.Children.FirstOrDefault(x => x.Name == sitecoreTitle) != null)
                            continue;
                        var movieItem = subFolder.Add(sitecoreTitle, new TemplateID(movieTemplateItem.ID));
                        movieItem.Editing.BeginEdit();
                        movieItem["Original title"] = movieExt.OriginalTitle;
                        movieItem["Title"] = movieExt.Title;
                        movieItem["Tagline"] = movieExt.Tagline;
                        movieItem["Body"] = movieExt.Overview;
                        movieItem["Vote average"] = movieExt.VoteAverage.ToString("00.00");
                        movieItem["Vote count"] = movieExt.VoteCount.ToString();
                        var genresField = (MultilistField) movieItem.Fields["Genres"];

                        foreach (var genre in movieExt.Genres)
                        {
                            var genreSitecoreName = ItemUtil.ProposeValidItemName(genre.Name.Trim().ToLower());
                            var genreItem = genresContainer.Children.FirstOrDefault(x => x.Name == genreSitecoreName);
                            if (genreItem == null)
                            {
                                genreItem = genresContainer.Add(genreSitecoreName, new TemplateID(listItem.ID));
                                genreItem.Editing.BeginEdit();
                                genreItem["Id"] = genre.Id.ToString();
                                genreItem["Name"] = genre.Name;
                                genreItem.Editing.EndEdit();
                            }
                            genresField.Add(genreItem.ID.ToString());
                        }
                        movieItem["Tmdb Id"] = movieExt.Id.ToString();
                        movieItem["Imdb Id"] = movieExt.ImdbId;
                        movieItem["Runtime"] = movieExt.Runtime.GetValueOrDefault().ToString();
                        movieItem["Status"] = movieExt.Status;
                        movieItem["Release date"] = DateUtil.ToIsoDate(movieExt.ReleaseDate.GetValueOrDefault());

                        var productionCompaniesField = (MultilistField) movieItem.Fields["Production companies"];

                        foreach (var productionCompany in movieExt.ProductionCompanies)
                        {
                            var productionCompanySitecoreName =
                                ItemUtil.ProposeValidItemName(productionCompany.Name.Trim().ToLower());
                            var productionCompanyItem =
                                productionCompaniesContainer.Children.FirstOrDefault(
                                    x => x.Name == productionCompanySitecoreName);
                            if (productionCompanyItem == null)
                            {
                                productionCompanyItem = productionCompaniesContainer.Add(productionCompanySitecoreName,
                                    new TemplateID(listItem.ID));
                                productionCompanyItem.Editing.BeginEdit();
                                productionCompanyItem["Id"] = productionCompany.Id.ToString();
                                productionCompanyItem["Name"] = productionCompany.Name;
                                productionCompanyItem.Editing.EndEdit();
                            }
                            productionCompaniesField.Add(productionCompanyItem.ID.ToString());
                        }
                        var productionCountriesField = (MultilistField) movieItem.Fields["Production countries"];

                        foreach (var productionCountry in movieExt.ProductionCountries)
                        {
                            var productionCountrySitecoreName =
                                ItemUtil.ProposeValidItemName(productionCountry.Name.Trim().ToLower());
                            var productionCountryItem =
                                productionCountriesContainer.Children.FirstOrDefault(
                                    x => x.Name == productionCountrySitecoreName);
                            if (productionCountryItem == null)
                            {
                                productionCountryItem = productionCountriesContainer.Add(productionCountrySitecoreName,
                                    new TemplateID(listItem.ID));
                                productionCountryItem.Editing.BeginEdit();
                                productionCountryItem["Id"] = productionCountry.Iso_3166_1;
                                productionCountryItem["Name"] = productionCountry.Name;
                                productionCountryItem.Editing.EndEdit();
                            }
                            productionCountriesField.Add(productionCountryItem.ID.ToString());
                        }
                        var posterUri = client.GetImageUrl("original", movie.PosterPath);
                        if (!String.IsNullOrEmpty(movie.PosterPath))
                        {
                            var mediaItem = AddFile(posterUri, sitecoreTitle, movie.PosterPath.Replace("/", ""));
                            if (mediaItem != null)
                            {
                                Sitecore.Data.Fields.ImageField imageField = movieItem.Fields["Image"];
                                imageField.MediaID = mediaItem.ID;
                            }
                        }
                        movieItem["Menu title"] = movie.Title;
                        movieItem["SEO title"] = movie.Title;
                        movieItem.Editing.EndEdit();
                    }
                }
                IndexCustodian.ResumeIndexing();
                Sitecore.Configuration.Settings.Indexing.Enabled = true;
                IndexCustodian.RebuildAll();
            }}

        public MediaItem AddFile(Uri url, string fileName, string originalFileName)
        {
            var mediaFilesContainer =
                Factory.GetDatabase("master").GetItem(new ID("{D05418BD-FDF8-4226-865F-E00FEFA7EDA9}"));
            var mediaFolderTemplate =
                Factory.GetDatabase("master").GetItem(new ID("{FE5DD826-48C6-436D-B87A-7C4210C7413B}"));
            using (var client = new WebClient())
            {
                // Remember to insert the appropriate try..catch..finally blocks.
                // I have refrained from using them to compress the example.
                var sitecoreName = Path.GetFileNameWithoutExtension(fileName);
                sitecoreName = ItemUtil.ProposeValidItemName(sitecoreName.ToLower().Trim());
                var firstLetter = sitecoreName.Substring(0, 1);
                var subFolder = mediaFilesContainer.Children.FirstOrDefault(x => x.Name == firstLetter);
                if (subFolder == null)
                    subFolder = mediaFilesContainer.Add(firstLetter, new TemplateID(mediaFolderTemplate.ID));
                var options = new Sitecore.Resources.Media.MediaCreatorOptions
                {
                    FileBased = false,
                    IncludeExtensionInItemName = false,
                    KeepExisting = false,
                    Versioned = false,
                    Destination = subFolder.Paths.FullPath + "/" + sitecoreName,
                    Database = Factory.GetDatabase("master")
                };
                if (Factory.GetDatabase("master").GetItem(options.Destination) != null)
                    return Factory.GetDatabase("master").GetItem(options.Destination);
                var image = client.DownloadData(url);

                using (var ms = new MemoryStream(image))
                {
                    Item mediaItem =
                        Sitecore.Resources.Media.MediaManager.Creator.CreateFromStream(ms, originalFileName,
                            options);
                    return mediaItem;
                }
                
            }

            return null;
        }
    }
}