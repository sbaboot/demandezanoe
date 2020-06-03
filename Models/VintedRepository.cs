using System;
using System.Collections.Generic;
using demandezanoe.Services;
using OpenQA.Selenium;

namespace demandezanoe.Models
{
    public class VintedRepository : IVintedRepository
    {
        string baseUrl = "https://www.vinted.fr/vetements?";
        protected static IWebDriver driver;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="brandId"></param>
        /// <param name="colorId"></param>
        /// <param name="status"></param>
        /// <param name="priceFrom"></param>
        /// <param name="priceTo"></param>
        /// <param name="textarea"></param>
        /// <returns></returns>
        public List<Vinted> GetProductList(string catalogId = null, string brandId = "0", string colorId = "0", 
            string status = "0", string priceFrom = "0", string priceTo = "0", string textarea = "0")
        {
            List<Vinted> prodList = new List<Vinted>();

            try
            {
                // Parameters et query strings used to build our url base
                string[] parameters = { catalogId, brandId, colorId, status, priceFrom, priceTo, textarea, "order" };
                string[] queryStrings = { "catalog[]", "brand_id[]", "color_id[]", "status[]", "price_from", "price_to", "search_text", "newest_first" };
                baseUrl = GenericMethods.GetBaseUrl(baseUrl, parameters, queryStrings);
                
                // create driver and navigate to url defined just before
                driver = SeleniumDriver.Setup();
                SeleniumDriver.NavigateToUrl(baseUrl);

                string cssClass = "[class='c-text c-text--subtitle c-text--left c-text--content']";
                var pages = SeleniumDriver.GetNbPages(cssClass);

                int counter = 1;
                for (int i = 1; i <= pages; i++)
                {
                    var nodes = driver.FindElements(By.ClassName("feed-grid__item"));
                     foreach (var node in nodes)
                     {
                        //var picture = node.FindElement(By.CssSelector("[class='c-badge c-badge--muted']")).FindElement(By.TagName("span")).Displayed ? "No picture" : node.FindElement(By.ClassName("c-box__image")).FindElement(By.TagName("img")).GetAttribute("src");
                        var picture = node.FindElement(By.ClassName("c-box__image")).FindElement(By.TagName("img")).GetAttribute("src");
                        var link = node.FindElement(By.ClassName("c-box__overlay")).GetAttribute("href");
                        var brand = node.FindElement(By.ClassName("c-box__subtitle")).GetAttribute("innerText");
                        var price = node.FindElement(By.ClassName("c-box__title")).Text.Replace(" €", "");
                        
                        prodList.Add(new Vinted()
                        {
                            Id = counter++,
                            Picture = picture,
                            Link = link,
                            Brand = brand,
                            Modele = textarea,
                            Price = price
                        });
                     }

                    if (i == pages + 1) { break; }
                    By isLimit = By.CssSelector("[class='c-pagination__next is-disabled']");
                    By nextPage = By.ClassName("c-pagination__next");
                    SeleniumDriver.GetNextPage(isLimit, nextPage, "href");

                }

                SeleniumDriver.CleanUp();
                return prodList;

            }
            catch (Exception ex)
            {
                SeleniumDriver.CleanUp();
                throw new Exception("message: " + ex);
            }

        }

       
    }
}


