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
    public class UrlComputedField : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;
            if (item == null)
                return null;
            // setting the context to website(sorry for hardcoding), to enable proper links
            using (new SiteContextSwitcher(Sitecore.Sites.SiteContext.GetSite("website")))
            {
                return LinkManager.GetItemUrl(item);
            }
            return null;
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}
