using System;
using System.Collections.Generic;
using demandezanoe.Utils;
using Flurl;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace demandezanoe.Models
{
    public class VintedRepository : IVintedRepository
    {
        string baseUrl = "https://www.vinted.fr/vetements?";
        protected static IWebDriver driver;

        public List<Vinted> GetProductList(string catalogId = null, string brandId = "0", string colorId = "0", 
            string status = "0", string priceFrom = "0", string priceTo = "0", string textarea = "0")
        {
            List<Vinted> prodList = new List<Vinted>();

            try
            {
                baseUrl = baseUrl
                   .SetQueryParam(!string.IsNullOrEmpty(catalogId) && catalogId != "0" ? "catalog[]" : "", new[] { catalogId == "0" ? null : catalogId })
                   .SetQueryParam(!string.IsNullOrEmpty(brandId) && brandId != "0" ? "brand_id[]" : "", new[] { brandId == "0" ? null : brandId })
                   .SetQueryParam(!string.IsNullOrEmpty(colorId) && colorId != "0" ? "color_id[]" : "", new[] { colorId == "0" ? null : colorId })
                   .SetQueryParam(!string.IsNullOrEmpty(status) && status != "0" ? "status[]" : "", new[] { status == "0" ? null : status })
                   .SetQueryParam(!string.IsNullOrEmpty(priceFrom) && priceFrom != "0" ? "price_from" : "", new[] { priceFrom == "0" ? null : priceFrom })
                   .SetQueryParam(!string.IsNullOrEmpty(priceTo) && priceTo != "0" ? "price_to" : "", new[] { priceTo == "0" ? null : priceTo })
                   .SetQueryParam(!string.IsNullOrEmpty(textarea) && textarea != "0" ? "search_text" : "", new[] { textarea == "0" ? null : textarea })
                   .SetQueryParam("order", new[] { "newest_first" });

                driver = SeleniumDriver.Setup();
                SeleniumDriver.NavigateToUrl(baseUrl);
                var pages = SeleniumDriver.GetNbPages();

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
                    
                    try
                    {
                        if (i == pages) { break; }
                        var isLimit = driver.FindElement(By.CssSelector("[class='c-pagination__next is-disabled']"));
                        if (isLimit.Displayed)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        var nextPage = driver.FindElement(By.ClassName("c-pagination__next")).GetAttribute("href");
                        SeleniumDriver.NavigateToUrl(nextPage);
                        SeleniumDriver.WaitForLoad();
                    }
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


