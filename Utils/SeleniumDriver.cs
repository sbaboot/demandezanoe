using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace demandezanoe.Utils
{
    public class SeleniumDriver
    {
        protected static IWebDriver _driver;
        public SeleniumDriver(IWebDriver driver)
        {
            _driver = driver;
        }

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

        public static void NavigateToUrl(string baseUrl)
        {
            _driver.Navigate().GoToUrl(baseUrl);
        }

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

        public static int GetNbPages()
        {
            var foundItems = _driver.FindElement(By.CssSelector("[class='c-text c-text--subtitle c-text--left c-text--content']")).GetAttribute("innerText");
            var nbItems = Convert.ToDouble(Regex.Match(foundItems, @"\d+").Value);
            if (nbItems >= 72)
            {
                return 72 / 24;
            }

            var nbPages = (int)Math.Ceiling(nbItems / 24);
            return nbPages;
        }

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
