using demandezanoe.Models.Entities;
using demandezanoe.Services;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
namespace demandezanoe.Models
{
    public class VestiaireCollectiveRepository : IVestiaireCollectiveRepository
    {
        string baseUrl = "https://fr.vestiairecollective.com/femme/";
        protected static IWebDriver _driver;
        protected static ISeleniumRepository _seleniumRepository;

        public VestiaireCollectiveRepository(ISeleniumRepository seleniumRepository)
        {
            _seleniumRepository = seleniumRepository;
        }

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

                // Create _driver and navigate to url defined just before
                _driver = _seleniumRepository.SetupVestiaire();
                _seleniumRepository.NavigateToVestiaire(baseUrl);

                // if no results stop scraping
                bool hasResults = _seleniumRepository.HasResults("vestiaire");
                if (!hasResults) { _seleniumRepository.CloseVestiaire(); return prodList = null; };

                // Filter with the latest articles
                _seleniumRepository.NewestFirstVestiaire();

                // Give the number of pages for the results
                var pages = _seleniumRepository.GetNbPages("vestiaire");

                // Posibility to display 120 results. If two pages, Click.
                if (pages == 2)
                {
                    _seleniumRepository.GetNextPage("vestiaire");
                }

                var nodes = _driver.FindElements(By.ClassName("catalog__product"));
                int counter = 1;
                foreach (var node in nodes)
                {
                    var link = "";
                    try
                    {
                        link = node.FindElement(By.ClassName("productSnippet")).FindElement(By.TagName("vc-ref")).FindElement(By.TagName("a")).Displayed ? node.FindElement(By.ClassName("productSnippet")).FindElement(By.TagName("vc-ref")).FindElement(By.TagName("a")).GetAttribute("href") : "";
                    }
                    catch { link = ""; }

                    prodList.Add(new VestaireCollective()
                    {
                        Id = counter++,
                        Picture = node.FindElement(By.ClassName("image")).GetAttribute("src"),
                        Link = link,
                        Brand = node.FindElement(By.ClassName("productSnippet__text--brand")).GetAttribute("innerText"),
                        Modele = modele,
                        Price = node.FindElement(By.ClassName("productSnippet__price")).Text.Replace(" €", "")
                    });
                }

                _seleniumRepository.CloseVestiaire();
                return prodList;

            }
            catch (Exception ex)
            {
                _seleniumRepository.CloseVestiaire();
                throw new Exception("Message: " + ex);
            }
        }

    }
}
