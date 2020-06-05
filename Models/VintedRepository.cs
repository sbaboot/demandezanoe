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
        /// <param name="catalog"></param>
        /// <param name="brand"></param>
        /// <param name="color"></param>
        /// <param name="condition"></param>
        /// <param name="priceFrom"></param>
        /// <param name="priceTo"></param>
        /// <param name="modele"></param>
        /// <returns></returns>
        public List<Vinted> GetProductList(string catalog = "0", string brand = "0", string color = "0", 
            string condition = "0", string priceFrom = "0", string priceTo = "0", string modele = "0")
        {
            List<Vinted> prodList = new List<Vinted>();

            try
            {
                // Parameters et query strings used to build our url base
                string[] parameters = { catalog, brand, color, condition, priceFrom, priceTo, modele, "newest_first" };
                string[] queryStrings = { "catalog[]", "brand_id[]", "color_id[]", "status[]", "price_from", "price_to", "search_text", "order" };
                baseUrl = GenericMethods.GetBaseUrlVinted(baseUrl, parameters, queryStrings);
                
                // create driver and navigate to url defined just before
                driver = SeleniumDriver.Setup();
                SeleniumDriver.NavigateToUrl(baseUrl);

                By cssClass = By.CssSelector("[class='c-text c-text--subtitle c-text--left c-text--content']");
                var pages = SeleniumDriver.GetNbPages("vinted", cssClass);

                int counter = 1;
                for (int i = 1; i <= pages; i++)
                {
                     var nodes = driver.FindElements(By.ClassName("feed-grid__item"));
                     foreach (var node in nodes)
                     {                      
                        prodList.Add(new Vinted()
                        {
                            Id = counter++,
                            Picture = node.FindElement(By.ClassName("c-box__image")).FindElement(By.TagName("img")).GetAttribute("src"),
                            Link = node.FindElement(By.ClassName("c-box__overlay")).GetAttribute("href"),
                            Brand = node.FindElement(By.ClassName("c-box__subtitle")).GetAttribute("innerText"),
                            Modele = modele,
                            Price = node.FindElement(By.ClassName("c-box__title")).Text.Replace(" €", "")
                    });
                     }

                    // Click or not in the next page
                    if (i == pages) { break; }
                    SeleniumDriver.GetNextPage("vinted");

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


