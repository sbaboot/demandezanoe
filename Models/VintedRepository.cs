using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using demandezanoe.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace demandezanoe.Models
{
    public class VintedRepository : IVintedRepository
    {
        string baseUrl = "https://www.vinted.fr/vetements?";
        public static IWebDriver driver;
        
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
                ChromeOptions options = new ChromeOptions();
                driver = new ChromeDriver(options);
                driver.Navigate().GoToUrl(baseUrl);

                var foundItems = driver.FindElement(By.CssSelector("[class='Text_text__QBn4- Text_subtitle__1I9iB Text_left__3s3CR']")).GetAttribute("innerText");
                var nbItems = Convert.ToDouble(Regex.Match(foundItems, @"\d+").Value);
                var pages = (int)Math.Ceiling(nbItems / 24);
                if (nbItems >= 72) { pages = 72 / 24; };

                int counter = 1;
                for (int i = 1; i <= pages; i++)
                {
                     var nodes = driver.FindElements(By.ClassName("feed-grid__item"));
                     foreach (var node in nodes)
                     {
                        var picture = "";
                        try
                        {
                           picture = node.FindElement(By.ClassName("c-box__image")).FindElement(By.TagName("img")).Displayed ? node.FindElement(By.ClassName("c-box__image")).FindElement(By.TagName("img")).GetAttribute("src") : "";

                        }
                        catch (NoSuchElementException) { picture = ""; }
                        prodList.Add(new Vinted()
                        {
                            TotalResults = nodes.Count.ToString(),
                            Id = counter++,
                            Picture = picture,
                            Link = node.FindElement(By.ClassName("c-box__overlay")).GetAttribute("href"),
                            Brand = node.FindElement(By.ClassName("c-box__subtitle")).GetAttribute("innerText"),
                            Modele = modele,
                            Price = node.FindElement(By.ClassName("c-box__title")).Text.Replace(" €", "")
                        });
                     }

                    // Click or not in the next page
                    if (i == pages) { break; }
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
                        var nextPageVinted = driver.FindElement(By.ClassName("c-pagination__next")).GetAttribute("href");
                        driver.Navigate().GoToUrl(nextPageVinted);
                    }
                }

                if (driver != null)
                {
                    driver.Close();
                    driver.Quit();
                    driver.Dispose();
                    driver = null;
                }
                return prodList;

            }
            catch (Exception ex)
            {
                if (driver != null)
                {
                    driver.Close();
                    driver.Quit();
                    driver.Dispose();
                    driver = null;
                }
                throw new Exception("Message: " + ex);
            }

        }

       
    }
}


