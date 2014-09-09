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
    public class MovieLengthComputedField : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;
            if (item == null)
                return null;
            if (!String.IsNullOrEmpty(item["Runtime"]))
            {
                int length = 0;
                if (Int32.TryParse(item["Runtime"], out length))
                {
                    if (length < 60)
                        return "0-59";
                    if (length <= 60 && length < 120)
                        return "60-119";
                    if (length <= 120 && length < 180)
                        return "120-178";
                    if (length <= 180 && length < 240)
                        return "180-239";
                    return "240+";
                }
                else
                {
                    return "";
                }
            }
            return null;
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}
