using Flurl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

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
        public static string GetBaseUrlVinted(string baseUrl, string[] parameters, string[] queryStrings)
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
                baseUrl += optionalUrl;

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


        public static string GetBaseUrlVestaire(string baseUrl, string[] parameters, string[] queryStrings)
        {
            string optionalUrl = "";
            string addQueryStrings = "";
            var i = 0;
            if (parameters.Length == 0) { return baseUrl; }
           
            for (var j = 0; j < parameters.Length; j++)
            {
                parameters[j] = parameters[j].Replace("#", "%23");
            } 

            if (parameters.Length > 1)
            {
                foreach (var parameter in parameters)
                {
                    optionalUrl += addQueryStrings
                        .AppendPathSegments(!string.IsNullOrEmpty(parameter) && parameter != "0" ? queryStrings[i] + parameter : "");
                    i++;
                }
                optionalUrl = ReplaceExceptFirst(optionalUrl, "#", "_");
                optionalUrl = optionalUrl.Replace("%3F", "?");
                if (optionalUrl.Contains("search/?q="))
                {
                    baseUrl = "https://fr.vestiairecollective.com/";
                }

                    baseUrl += optionalUrl;

            }

            return baseUrl;
        }
                
    }
}
//%23categoryParent%3dSacs%235%23brand%3dChanel%2350%23color%3dNoir%2314