using OpenQA.Selenium;

namespace demandezanoe.Models
{
    public interface ISeleniumRepository
    {
        public IWebDriver SetupVinted();

        public IWebDriver SetupVestiaire();

        public void NavigateToVestiaire(string baseUrl);
        public void NavigateToVinted(string baseUrl);

        public void CloseVestiaire();
        public void CloseVinted();

        public void NewestFirstVestiaire();

        public int GetNbPages(string site);

        public void GetNextPage(string site);

        public void WaitForLoad();


    }
}
