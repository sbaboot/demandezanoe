using demandezanoe.Models.Entities;
using demandezanoe.Services;
using Flurl;
using OpenQA.Selenium;
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
        protected static IWebDriver driver;

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
                baseUrl = GenericMethods.GetBaseUrlVestaire(baseUrl, parameters, queryStrings);
                
                // create driver and navigate to url defined just before
                driver = SeleniumDriver.Setup();
                SeleniumDriver.NavigateToUrl(baseUrl);

                // get newest articles first
                SeleniumDriver.NewestFirstVestiaire();

                // check how many results to get number of pages. If page 2 available, pages = 2
                By cssClass = By.ClassName("filters_checkbox_label_count");
                var pages = SeleniumDriver.GetNbPages("vestiaire", cssClass);

                // Posibility to display 120 results. If two pages, Click.
                if (pages == 2) { SeleniumDriver.GetNextPage("vestiaire"); };

                int counter = 1;
                var nodes = driver.FindElements(By.ClassName("catalog__product"));
                foreach (var node in nodes)
                {
                    prodList.Add(new VestaireCollective()
                    {
                        Id = counter++,
                        Picture = node.FindElement(By.ClassName("image")).GetAttribute("src"),
                        Link = node.FindElement(By.CssSelector("[rel='noopener'][itemprop='url']")).GetAttribute("href"),
                        Brand = node.FindElement(By.ClassName("productSnippet__text--brand")).GetAttribute("innerText"),
                        Modele = modele,
                        Price = node.FindElement(By.ClassName("productSnippet__price")).Text.Replace(" €", "")
                    });
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
