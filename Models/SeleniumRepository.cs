using demandezanoe.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Text.RegularExpressions;

namespace demandezanoe.Services
{
    public class SeleniumRepository : ISeleniumRepository
    {
        private static IWebDriver driverVinted;
        private static IWebDriver driverVestiaire;

        /// <summary>
        /// Setup chrome driver for Vinted
        /// </summary>
        /// <returns></returns>
        public IWebDriver SetupVinted()
        {
            if (driverVinted == null)
            {
                ChromeOptions options = new ChromeOptions();
                driverVinted = new ChromeDriver(options);
                return driverVinted;
            }
            else
                return driverVinted;
        }

        /// <summary>
        /// Setup chrome driver for Vestiaire Collective
        /// </summary>
        /// <returns></returns>
        public IWebDriver SetupVestiaire()
        {
            if (driverVestiaire == null)
            {
                ChromeOptions options = new ChromeOptions();
                driverVestiaire = new ChromeDriver(options);
                return driverVestiaire;
            }
            else
                return driverVestiaire;
        }

        /// <summary>
        /// Navigate to Vinted Url with vinted driver
        /// </summary>
        /// <param name="baseUrl"></param>
        public void NavigateToVinted(string baseUrl)
        {
            driverVinted.Navigate().GoToUrl(baseUrl);
        }

        /// <summary>
        /// Navigate to Vestiaire Collective Url with Vestiaire Collective driver
        /// </summary>
        /// <param name="baseUrl"></param>
        public void NavigateToVestiaire(string baseUrl)
        {
            driverVestiaire.Navigate().GoToUrl(baseUrl);
        }

        /// <summary>
        /// Close Vinted driver
        /// </summary>
        public void CloseVinted()
        {
            if (driverVinted != null)
            {
                driverVinted.Close();
                driverVinted.Quit();
                driverVinted.Dispose();
                driverVinted = null;
            }
        }

        /// <summary>
        /// Close Vestiaire Collective driver
        /// </summary>
        public void CloseVestiaire()
        {
            if (driverVestiaire != null)
            {
                driverVestiaire.Close();
                driverVestiaire.Quit();
                driverVestiaire.Dispose();
                driverVestiaire = null;
            }
        }

        /// <summary>
        /// Get the newest articles
        /// </summary>
        public void NewestFirstVestiaire()
        {
            WebDriverWait wait = new WebDriverWait(driverVestiaire, TimeSpan.FromSeconds(3));

            var privacyPolicy = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/app-root/div/vc-privacy-policy/vc-privacy-policy-banner/div/div[2]/button")));
            privacyPolicy.Click();
            
            var sortBy = driverVestiaire.FindElement(By.CssSelector("[class='catalogSort__button d-none d-md-inline-block']"));
            sortBy.Click();

            var newestFirst = driverVestiaire.FindElement(By.XPath("//*[@id=\"mat-radio-5\"]/label/div[2]"));
            newestFirst.Click();
        }

        /// <summary>
        /// Get number of pages result from multiple websites
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public int GetNbPages(string site)
        {
            try
            {
                var nbPages = 0;
                switch (site)
                {
                    case "vinted":
                        var foundItems = driverVinted.FindElement(By.CssSelector("[class='Text_text__QBn4- Text_subtitle__1I9iB Text_left__3s3CR']")).GetAttribute("innerText");
                        var nbItems = Convert.ToDouble(Regex.Match(foundItems, @"\d+").Value);
                        nbPages = (int)Math.Ceiling(nbItems / 24);
                        if (nbItems >= 72) { return 72 / 24; };
                        break;
                    case "vestiaire":
                        try
                        {
                            var nextPages = driverVestiaire.FindElement(By.XPath("/html/body/app-root/div/main/catalog-page/vc-catalog/div/div/ais-instantsearch/div/div[2]/div[1]/vc-catalog-widget-pagination/div/button[2]"));
                            if (nextPages.Displayed) { nbPages = 2; };
                            break;

                        }
                        catch (Exception)
                        {
                            nbPages = 1;
                            break;
                        }
                    default: break;
                }

                return nbPages;
            }
            catch (Exception)
            {   
                throw new Exception("Pas de résulats");
            }

        }

        /// <summary>
        /// Click on the next page to get next results
        /// </summary>
        /// <param name="site"></param>
        public void GetNextPage(string site)
        {
            switch (site)
            {
                case "vinted":
                    try
                    {
                        var isLimit = driverVinted.FindElement(By.CssSelector("[class='c-pagination__next is-disabled']"));
                        if (isLimit.Displayed)
                        {
                            return;
                        }
                    }
                    catch
                    {
                        var nextPageVinted = driverVinted.FindElement(By.ClassName("c-pagination__next")).GetAttribute("href");
                        NavigateToVinted(nextPageVinted);
                        WaitForLoad();
                    }
                    break;
                case "vestiaire":
                    var nextPageVestaire = driverVestiaire.FindElement(By.XPath("/html/body/app-root/div/main/catalog-page/vc-catalog/div/div/ais-instantsearch/div/div[2]/div[3]/vc-catalog-widget-hits-per-page/div/button[2]"));
                    nextPageVestaire.Click();
                    break;
                default: break;
            }
            

        }

        public void WaitForLoad()
        {
            var javaScriptExecutor = driverVinted as IJavaScriptExecutor;
            var wait = new WebDriverWait(driverVinted, TimeSpan.FromSeconds(10));

            bool readyCondition(IWebDriver webDriver) =>
                (bool)javaScriptExecutor.ExecuteScript("return (document.readyState == 'complete' && jQuery.active == 0)") == true;

            wait.Until(readyCondition);
        }

       
    }
}
