using System;
using System.Collections.Generic;
using demandezanoe.Controllers;
using demandezanoe.Models.Entities;
using demandezanoe.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace demandezanoe.Models
{
    public class VintedRepository : IVintedRepository
    {
        string baseUrl = "https://www.vinted.fr/vetements?";
        protected static ISeleniumRepository _seleniumRepository;
        protected static IWebDriver _driver;

        public VintedRepository(ISeleniumRepository seleniumRepository)
        {
            _seleniumRepository = seleniumRepository;
        }

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
                _driver = _seleniumRepository.SetupVinted();
                _seleniumRepository.NavigateToVinted(baseUrl);

                // if no results stop scraping
                bool hasResults = _seleniumRepository.HasResults("vinted"); 
                if (!hasResults) { _seleniumRepository.CloseVinted(); return prodList = null; };

                // Give the number of pages for the results
                var pages = _seleniumRepository.GetNbPages("vinted");
                int counter = 1;
                for (int i = 1; i <= pages; i++)
                {
                     var nodes = _driver.FindElements(By.ClassName("feed-grid__item"));
                     foreach (var node in nodes)
                     {
                        var picture = "";
                        try
                        {
                           picture = node.FindElement(By.ClassName("c-box__image")).FindElement(By.TagName("img")).Displayed ? node.FindElement(By.ClassName("c-box__image")).FindElement(By.TagName("img")).GetAttribute("src") : "";

                        }
                        catch { picture = ""; }
                        prodList.Add(new Vinted()
                        {
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
                    _seleniumRepository.GetNextPage("vinted");
                }

                _seleniumRepository.CloseVinted();
                return prodList;

            }
            catch (Exception ex)
            {
                _seleniumRepository.CloseVinted();
                throw new Exception("Message: " + ex);
            }

        }

       
    }
}


