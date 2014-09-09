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
    public class DecadeComputedField : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;
            if (item == null)
                return null;
            if (!String.IsNullOrEmpty(item["Release date"]))
            {
                DateField dateField = item.Fields["Release date"];
                if (dateField.DateTime != DateTime.MinValue)
                {
                    return ((int)Math.Floor(dateField.DateTime.Year / 10.0)) * 10;
                }
            }
            return null;
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}
