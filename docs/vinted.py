from bs4 import BeautifulSoup as bs
from selenium import webdriver
from pyvirtualdisplay import Display
from math import ceil

class Vinted:
    def __init__(self, base_url):
        self.base_url = base_url
        self.pages_limit = 1

    def __enter__(self):
        self.display = Display(visible=0, size=(1366, 768))
        self.display.start()
        self.driver = webdriver.Chrome()
        #self.driver.set_page_load_timeout(30)
        return self

    def __exit__(self, exc_type, exc_val, exc_tb):
        self.driver.quit()
        self.display.stop()
        del self.driver
        del self.display


    def parseCriteria(self, criteria):
        request_criteria = ""
        catalog = 'catalog[]=' + str(criteria["catalog_id"]) if (str(criteria["catalog_id"]) != "") else ""
        brand = 'brand_id[]=' + str(criteria["brand_id"]) if (str(criteria["brand_id"]) is not "") else ""
        color = 'color_id[]=' + str(criteria["color_id"]) if (str(criteria["color_id"]) is not "") else ""
        status = 'status[]=' + str(criteria["status_id"]) if (str(criteria["status_id"]) is not "") else ""
        size = 'size_id[]=' + str(criteria["size_id"]) if (str(criteria["size_id"]) is not "") else ""
        price = 'price_to=' + str(criteria["price_max"]) if (str(criteria["price_max"]) is not "") else ""
        options = [catalog, brand, color, status, size, price]

        for id, el in enumerate(options):
            if (len(el) > 0):
                request_criteria += el
                if (id != len(options) - 1):
                    request_criteria += "&"

        return request_criteria

    def getNbPages(self, criteria):
        self.driver.get(self.base_url + criteria)
        source = bs(self.driver.page_source, 'html.parser')
        try:
            founded_items = source.find('div', class_='js-catalog-body').find_all('div')[0].find_all('span')[0].text.split()[0][:3]
            pages = ceil(int(founded_items) / 24)

            return {"error": 0, "pages": pages, "nbResults":int(founded_items)}
        except:
            return {"error": 1, "message": "Page can't be scraped"}

    def get(self, criteria, limit=0):

        if(limit):
            limit = self.pages_limit
        self.driver.get(self.base_url + criteria)
        source = bs(self.driver.page_source, 'html.parser')
        try:
            founded_items = source.find('div', class_='js-catalog-body').find_all('div')[0].find_all('span')[0].text.split()[0]
            pages = ceil(int(founded_items) / 24)
            print("{pages} pages for a total of {founded_items} results".format(pages=pages, founded_items=founded_items))

            if (int(founded_items) > 150):
                return {"error": 1, "message": "Too many results"}

        except:
            return {"error": 1, "message": "Page can't be scraped"}

        items = source.find_all('div', class_="feed-grid__item")

        response = {"items": [], "error": 0}
        for element in items:
            try:
                response["items"].append({
                    "owner": element.find('div', class_='c-box__owner').find('span').string,
                    "image": element.find('img', class_='c-image__content')['src'],
                    "price": element.find('div', class_='c-box__title').find('span').string,
                    "link": "https://www.vinted.fr" + element.find('a', class_='c-box__overlay')['href']
                })
            except:
                pass

        try:
            if ("is-disabled") not in source.find('a', class_='c-pagination__next')['class']:
                pagination = source.find('a', class_='c-pagination__next')['href'][11:]
            else:
                pagination = None
        except:
            pagination = None

        if (pagination is not None):
            next_results = self.get(pagination)["items"]
            for result in next_results:
                response["items"].append(result)


        del items
        del source

        return response