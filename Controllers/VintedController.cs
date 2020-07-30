using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using demandezanoe.Models;
using demandezanoe.Models.Entities;
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

        [HttpGet]
        public IEnumerable<Vinted> Get(string catalog, string brand, string color,
           string condition, string priceFrom, string priceTo, string modele)
        {
            return _repository.GetProductList(catalog, brand, color, condition, priceFrom, priceTo, modele);
        }
    }
}
