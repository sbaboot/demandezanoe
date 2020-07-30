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
    public class JoliClosetRepository : IJoliClosetRepository
    {
        string baseUrl = "https://www.jolicloset.com/fr/mode-femme";
        protected static ISeleniumRepository _seleniumRepository;
        protected static IWebDriver _driver;

        public JoliClosetRepository(ISeleniumRepository seleniumRepository)
        {
            _seleniumRepository = seleniumRepository;
        }

        public List<JoliCloset> GetProductList(string catalog = "0", string brand = "0", string modele = "0")
        {
            List<JoliCloset> prodList = new List<JoliCloset>();

            try
            {
                // Parameters et query strings used to build our url base
                string queryString ="?query=";                
                baseUrl = GenericMethods.GetBaseUrlJoliCloset(baseUrl, catalog, brand, modele, queryString);

                //create driver and navigate to url defined just before
                _driver = _seleniumRepository.SetupJoliCloset();
                _seleniumRepository.NavigateToJoliCloset(baseUrl);

                // if no results stop scraping
                bool hasResults = _seleniumRepository.HasResults("joliCloset");
                if (!hasResults) { _seleniumRepository.CloseJoliCloset(); return prodList = null; };

                // Give the number of pages for the results
                var pages = _seleniumRepository.GetNbPages("joliCloset");
                int counter = 1;
                for (int i = 1; i <= pages; i++)
                {
                    var nodes = _driver.FindElements(By.ClassName("standard"));
                    foreach (var node in nodes)
                    {
                        var picture = "";
                        var link = "";
                        try
                        {
                            picture = node.FindElement(By.CssSelector("[class='primary lazyautosizes lazyloaded']")).Displayed ? node.FindElement(By.CssSelector("[class='primary lazyautosizes lazyloaded']")).GetAttribute("src") : "";
                        }
                        catch { picture = ""; }

                        try
                        {
                            link = node.FindElement(By.CssSelector("[class='item-tile jcSLink']")).Displayed ? "https://jolicloset.com" + node.FindElement(By.CssSelector("[class='item-tile jcSLink']")).GetAttribute("data-joli") : "";
                        }
                        catch { link = ""; }

                        prodList.Add(new JoliCloset()
                        {
                            Id = counter++,
                            Picture = picture,
                            Link = link,
                            Brand = node.FindElement(By.ClassName("tile-snip")).GetAttribute("innerText"),
                            Modele = modele,
                            Price = node.FindElement(By.ClassName("prices")).Text.Replace(" €", "")
                        });
                    }

                    // Click or not in the next page
                    if (i == pages) { break; }
                    _seleniumRepository.GetNextPage("joliCloset");
                }

                _seleniumRepository.CloseJoliCloset();
                return prodList;

            }
            catch (Exception ex)
            {
                _seleniumRepository.CloseJoliCloset();                
                throw new Exception("Message: " + ex);
            }
        }      
    }
}


