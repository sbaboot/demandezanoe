using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Text.RegularExpressions;

namespace demandezanoe.Services
{
    public class SeleniumDriver
    {
        protected static IWebDriver _driver;
        public SeleniumDriver(IWebDriver driver)
        {
            _driver = driver;
        }
        /// <summary>
        /// Setup chromedriver
        /// </summary>
        /// <returns></returns>
        public static IWebDriver Setup()
        {
            if (_driver == null)
            {
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                
                var options = new ChromeOptions();
                options.AddArgument("--window-position=-32000,-32000");

                _driver = new ChromeDriver(service);
                _driver.Manage().Window.Maximize();
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                return _driver;
            }
            else
                return _driver;

        }

        /// <summary>
        /// Navigate to Url
        /// </summary>
        /// <param name="baseUrl"></param>
        public static void NavigateToUrl(string baseUrl)
        {
            _driver.Navigate().GoToUrl(baseUrl);
        }

        /// <summary>
        /// Close & quit the driver 
        /// </summary>
        public static void CleanUp()
        {
            if (_driver != null)
            {
                _driver.Close();
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }

        public static void NewestFirstVestiaire()
        {
            var privacyPolicy = _driver.FindElement(By.XPath("/html/body/app-root/div/vc-privacy-policy/vc-privacy-policy-banner/div/div[2]/button"));
            privacyPolicy.Click();

            var sortBy = _driver.FindElement(By.CssSelector("[class='catalogSort__button d-none d-md-inline-block']"));
            sortBy.Click();

            var newestFirst = _driver.FindElement(By.XPath("//*[@id=\"mat-radio-5\"]/label/div[2]"));
            newestFirst.Click();
        }

        /// <summary>
        /// Try to get number of pages
        /// </summary>
        /// <param name="cssSelector"></param>
        /// <returns></returns>
        public static int GetNbPages(string site, By cssSelector)
        {
            try
            {
                var foundItems = _driver.FindElement(cssSelector).GetAttribute("innerText");
                var nbItems = Convert.ToDouble(Regex.Match(foundItems, @"\d+").Value);
                var nbPages = 0;

                switch (site)
                {
                    case "vinted":
                        nbPages = (int)Math.Ceiling(nbItems / 24);
                        if (nbItems >= 72) { return 72 / 24; };
                        break;
                    case "vestiaire":
                        try
                        {
                            var nextPages = _driver.FindElement(By.XPath("/html/body/app-root/div/main/catalog-page/vc-catalog/div/div/ais-instantsearch/div/div[2]/div[1]/vc-catalog-widget-pagination/div/button[2]"));
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
        /// Click on next page if it's enable
        /// </summary>
        /// <param name="valueLimit"></param>
        /// <param name="valueNextPage"></param>
        /// <param name="attribute"></param>
        public static void GetNextPage(string site)
        {
            switch (site)
            {
                case "vinted":
                    try
                    {
                        var isLimit = _driver.FindElement(By.CssSelector("[class='c-pagination__next is-disabled']"));
                        if (isLimit.Displayed)
                        {
                            return;
                        }
                    }
                    catch
                    {
                        var nextPageVinted = _driver.FindElement(By.ClassName("c-pagination__next")).GetAttribute("href");
                        NavigateToUrl(nextPageVinted);
                        WaitForLoad();
                    }
                    break;
                case "vestiaire":
                    var nextPageVestaire = _driver.FindElement(By.XPath("/html/body/app-root/div/main/catalog-page/vc-catalog/div/div/ais-instantsearch/div/div[2]/div[3]/vc-catalog-widget-hits-per-page/div/button[2]"));
                    nextPageVestaire.Click();
                    break;
                default: break;
            }
            

        }

        /// <summary>
        /// Wait for the page to be loaded
        /// </summary>
        public static void WaitForLoad()
        {
            var javaScriptExecutor = _driver as IJavaScriptExecutor;
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            bool readyCondition(IWebDriver webDriver) =>
                (bool)javaScriptExecutor.ExecuteScript("return (document.readyState == 'complete' && jQuery.active == 0)") == true;

            wait.Until(readyCondition);
        }

       
    }
}
