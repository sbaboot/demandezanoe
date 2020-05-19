using System;
using System.Collections.Generic;
using demandezanoe.Utils;
using Flurl;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace demandezanoe.Models
{
    public class VintedRepository : IVintedRepository
    {
        string baseUrl = "https://www.vinted.fr/vetements?";
        public IWebDriver driver;    
        public List<Vinted> GetProductList(string catalogId = null, string brandId = null, string colorId = null, 
            string status = null, string sizeId = null, string priceTo = null)
        {
            try
            {
                baseUrl = baseUrl
               .SetQueryParam(!string.IsNullOrEmpty(catalogId) && catalogId != "0" ? "catalog[]" : "", new[] { catalogId == "0" ? null : catalogId })
               .SetQueryParam(!string.IsNullOrEmpty(brandId) && brandId != "0" ? "brand_id[]" : "", new[] { brandId == "0" ? null : brandId })
               .SetQueryParam(!string.IsNullOrEmpty(colorId) && colorId != "0" ? "color_id[]" : "", new[] { colorId == "0" ? null : colorId })
               .SetQueryParam(!string.IsNullOrEmpty(status) && status != "0" ? "status[]" : "", new[] { status == "0" ? null : status })
               .SetQueryParam(!string.IsNullOrEmpty(sizeId) && sizeId != "0" ? "size_id[]" : "", new[] { sizeId == "0" ? null : sizeId })
               .SetQueryParam(!string.IsNullOrEmpty(priceTo) && priceTo != "0" ? "price_to" : "", new[] { priceTo == "0" ? null : priceTo });

                driver = Selenium.Setup(driver);
                Selenium.NavigateToUrl(baseUrl, driver);             

                var pages = Selenium.GetNbPages(driver);
                List<Vinted> prodList = new List<Vinted>();

                int counter = 1;
                for (int i = 1; i <= pages; i++)
                {
                    var nodes = driver.FindElements(By.ClassName("feed-grid__item"));
                     foreach (var node in nodes)
                    {
                        prodList.Add(new Vinted()
                        {
                            Id = counter++,
                            Owner = node.FindElement(By.ClassName("c-box__owner")).Text,
                            Picture = node.FindElement(By.ClassName("c-box__image")).FindElement(By.TagName("img")).GetAttribute("src"),
                            Link = node.FindElement(By.ClassName("c-box__overlay")).GetAttribute("href"),
                            Brand = "Zara",
                            Color = "Beige",
                            ModelBag = "test",
                            Condition = Conditions.New,
                            Price = Convert.ToDouble(node.FindElement(By.ClassName("c-box__title")).Text.Replace(" €", ""))
                        });
                    }

                    try
                    {
                        var isLimit = driver.FindElement(By.CssSelector("[class='c-pagination__next is-disabled']"));
                        if (isLimit.Displayed)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        var nextPage = driver.FindElement(By.ClassName("c-pagination__next")).GetAttribute("href");
                        driver.Navigate().GoToUrl(nextPage);
                        System.Threading.Thread.Sleep(500);
                    }
                }

                Selenium.StopSelenium(driver);
                return prodList;

            }
            catch (Exception ex)
            {
               Selenium.StopSelenium(driver);
               throw new Exception("message: " + ex);
            }

        }

       
    }
}


