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

        /// <summary>
        /// Try to get number of pages
        /// </summary>
        /// <param name="cssSelector"></param>
        /// <returns></returns>
        public static int GetNbPages(string cssSelector)
        {
            try
            {
                var foundItems = _driver.FindElement(By.CssSelector(cssSelector)).GetAttribute("innerText");
                var nbItems = Convert.ToDouble(Regex.Match(foundItems, @"\d+").Value);
                if (nbItems >= 72)
                {
                    return 72 / 24;
                }

                var nbPages = (int)Math.Ceiling(nbItems / 24);
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
        public static void GetNextPage(By valueLimit, By valueNextPage, string attribute)
        {

            try
            {
                var isLimit = _driver.FindElement(valueLimit);
                if (isLimit.Displayed)
                {
                    return;
                }
            }
            catch
            {
                var nextPage = _driver.FindElement(valueNextPage).GetAttribute(attribute);
                NavigateToUrl(nextPage);
                WaitForLoad();
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
