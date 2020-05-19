using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Text.RegularExpressions;

namespace demandezanoe.Utils
{
    public static class Selenium
    {
        public static IWebDriver Setup(IWebDriver driver)
        {
            if (driver == null)
            {
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                var options = new ChromeOptions();
                options.AddArgument("--window-position=-32000,-32000");

                driver = new ChromeDriver(service);
                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                return driver;
            }
            else
                return driver;

        }

        public static void NavigateToUrl(string baseUrl, IWebDriver driver)
        {
            driver.Navigate().GoToUrl(baseUrl);
        }

        public static void StopSelenium(IWebDriver driver)
        {
            driver.Close();
            driver.Dispose();
            driver.Quit();
        }

        public static int GetNbPages(IWebDriver driver)
        {

            try
            {
                var foundItems = driver.FindElement(By.CssSelector("[class='c-text c-text--subtitle c-text--left c-text--content']")).GetAttribute("innerText");
                var nbItems = Convert.ToDouble(Regex.Match(foundItems, @"\d+").Value);
                if (nbItems > 168)
                {
                    nbItems = 168;
                    return 168 / 24;
                }

                var nbPages = (int)Math.Ceiling(nbItems / 24);
                return nbPages;
            }
            catch (Exception)
            {
                StopSelenium(driver);
                throw new Exception("Les critères indiqués ne permettent d'effectuer une recherche");
            }
        }
    }
}
