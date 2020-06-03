using Flurl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demandezanoe.Services
{
    public class GenericMethods
    {
        /// <summary>
        /// Get base url
        /// Replace all "?" by "&" except the first one.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="parameters"></param>
        /// <param name="queryStrings"></param>
        /// <returns></returns>
        public static string GetBaseUrl(string baseUrl, string[] parameters, string[] queryStrings)
        {
            string optionalUrl = "";
            string addQueryStrings = "";
            var i = 0;
            if (parameters.Length == 0) { return baseUrl; }
            if (parameters.Length > 1)
            {
                foreach (var parameter in parameters)
                {
                    optionalUrl += addQueryStrings
                        .SetQueryParam(!string.IsNullOrEmpty(parameter) && parameter != "0" ? queryStrings[i] : "", new[] { parameter == "0" ? null : parameter });
                    i++;
                }
                optionalUrl = ReplaceExceptFirst(optionalUrl, "?", "&");
                optionalUrl = optionalUrl.Replace("?", "");
                baseUrl = baseUrl + optionalUrl;

            }

            return baseUrl;
        }

        /// <summary>
        /// Replace all characters except the first one founded
        /// </summary>
        /// <param name="text"></param>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        private static string ReplaceExceptFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }

            int strlen = pos + search.Length;
            return text.Substring(0, strlen) + (text.Substring(strlen)).Replace(search, replace);
        }
                
    }
}




//baseUrl = baseUrl
//   .SetQueryParam(!string.IsNullOrEmpty(catalogId) && catalogId != "0" ? "catalog[]" : "", new[] { catalogId == "0" ? null : catalogId })
//   .SetQueryParam(!string.IsNullOrEmpty(brandId) && brandId != "0" ? "brand_id[]" : "", new[] { brandId == "0" ? null : brandId })
//   .SetQueryParam(!string.IsNullOrEmpty(colorId) && colorId != "0" ? "color_id[]" : "", new[] { colorId == "0" ? null : colorId })
//   .SetQueryParam(!string.IsNullOrEmpty(status) && status != "0" ? "status[]" : "", new[] { status == "0" ? null : status })
//   .SetQueryParam(!string.IsNullOrEmpty(priceFrom) && priceFrom != "0" ? "price_from" : "", new[] { priceFrom == "0" ? null : priceFrom })
//   .SetQueryParam(!string.IsNullOrEmpty(priceTo) && priceTo != "0" ? "price_to" : "", new[] { priceTo == "0" ? null : priceTo })
//   .SetQueryParam(!string.IsNullOrEmpty(textarea) && textarea != "0" ? "search_text" : "", new[] { textarea == "0" ? null : textarea })
//   .SetQueryParam("order", new[] { "newest_first" });