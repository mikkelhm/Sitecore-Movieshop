using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Indexing;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.Sites;

namespace MovieShop.BusinessLayer.Search.ComputedFields
{
    public class ImageUrlComputedField : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;
            if (item == null)
                return null;
            if (item["Image"] == null)
                return null;
            ImageField imageField = item.Fields["Image"];
            if (imageField == null)
                return null;
            var mediaItem = imageField.MediaItem;
            if (mediaItem == null)
                return null;

            using (new SiteContextSwitcher(Sitecore.Sites.SiteContext.GetSite("website")))
            {
                return MediaManager.GetMediaUrl(mediaItem);
            }
            return null;
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}
