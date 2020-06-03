using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using demandezanoe.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace demandezanoe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VintedController : ControllerBase
    {
        private readonly IVintedRepository _repository;
        public VintedController(IVintedRepository repository)
        {
            _repository = repository;
        }

        [Route("{catalogId}/{brandId}/{colorId}/{status}/{priceFrom}/{priceTo}/{textarea}")]
        [HttpGet]
        public IEnumerable<Vinted> Get(string catalogId, string brandId, string colorId,
           string status, string priceFrom, string priceTo, string textarea)
        {
            return _repository.GetProductList(catalogId, brandId, colorId, status, priceFrom, priceTo, textarea);
        }
    }
}
