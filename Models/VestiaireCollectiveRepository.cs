using demandezanoe.Models.Entities;
using demandezanoe.Services;
using Flurl;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace demandezanoe.Models
{
    public class VestiaireCollectiveRepository : IVestiaireCollectiveRepository
    {
        string baseUrl = "https://fr.vestiairecollective.com/femme/";
        public static IWebDriver driver;

        public List<VestaireCollective> GetProductList(string catalog = "0", string brand = "0", string color = "0",
            string condition = "0", string priceFrom = "0", string priceTo = "0", string modele = "0")
        {
            List<VestaireCollective> prodList = new List<VestaireCollective>();

            if (priceTo != "0") { priceTo += "00"; };
            if (priceFrom != "0") { priceFrom += "00"; };
            try
            {
                string[] parameters = { modele, catalog, brand, color, condition, priceFrom, priceTo };
                string[] queryStrings = { "search/?q=", "#categoryParent=", "#brand=", "#color=", "#condition=", "#priceMin=", "#priceMax=" };
                baseUrl = GenericMethods.GetBaseUrlVestiaire(baseUrl, parameters, queryStrings);

                // create driver and navigate to url defined just before
                //driver = SeleniumDriver.SetupVestiaire();
                //SeleniumDriver.NavigateToUrl(baseUrl);
                //SeleniumDriver.NewestFirstVestiaire();
                //var pages = SeleniumDriver.GetNbPages("vestiaire");

                ChromeOptions options = new ChromeOptions();
                //driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), options);
                driver = new ChromeDriver(options);
                driver.Navigate().GoToUrl(baseUrl);


                var privacyPolicy = driver.FindElement(By.XPath("/html/body/app-root/div/vc-privacy-policy/vc-privacy-policy-banner/div/div[2]/button"));
                privacyPolicy.Click();

                var sortBy = driver.FindElement(By.CssSelector("[class='catalogSort__button d-none d-md-inline-block']"));
                sortBy.Click();

                var newestFirst = driver.FindElement(By.XPath("//*[@id=\"mat-radio-5\"]/label/div[2]"));
                newestFirst.Click();
                // get newest articles first
                Thread.Sleep(1500);

                // check how many results to get number of pages. If page 2 available, pages = 2
                var pages = 0;
                try
                {
                    var nextPages = driver.FindElement(By.XPath("/html/body/app-root/div/main/catalog-page/vc-catalog/div/div/ais-instantsearch/div/div[2]/div[1]/vc-catalog-widget-pagination/div/button[2]"));
                    if (nextPages.Displayed) { pages = 2; };

                }
                catch (Exception)
                {
                    pages = 1;
                }

                // Posibility to display 120 results. If two pages, Click.
                //if (pages == 2) { SeleniumDriver.GetNextPage("vestiaire"); };

                if (pages == 2)
                {
                    var nextPageVestaire = driver.FindElement(By.XPath("/html/body/app-root/div/main/catalog-page/vc-catalog/div/div/ais-instantsearch/div/div[2]/div[3]/vc-catalog-widget-hits-per-page/div/button[2]"));
                    nextPageVestaire.Click();
                }
                    

                var nodes = driver.FindElements(By.ClassName("catalog__product"));
                
                int counter = 1;
                foreach (var node in nodes)
                {
                    var link = "";
                    try
                    {
                        link = node.FindElement(By.ClassName("productSnippet")).FindElement(By.TagName("vc-ref")).FindElement(By.TagName("a")).Displayed ? node.FindElement(By.ClassName("productSnippet")).FindElement(By.TagName("vc-ref")).FindElement(By.TagName("a")).GetAttribute("href") : "";

                    }
                    catch (NoSuchElementException) { link = ""; }

                    prodList.Add(new VestaireCollective()
                    {
                        TotalResults = nodes.Count.ToString(),
                        Id = counter++,
                        Picture = node.FindElement(By.ClassName("image")).GetAttribute("src"),
                        Link = link,
                        Brand = node.FindElement(By.ClassName("productSnippet__text--brand")).GetAttribute("innerText"),
                        Modele = modele,
                        Price = node.FindElement(By.ClassName("productSnippet__price")).Text.Replace(" €", "")
                    });
                }                

                //SeleniumDriver.CloseDriver();

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
                //SeleniumDriver.CloseDriver();

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
