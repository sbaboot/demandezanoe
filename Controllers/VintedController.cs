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

        // GET: api/Vinted/
        [Route("{brandId}/{catalogId}/{colorId}/{status}/{sizeId}/{priceTo}")]
        [HttpGet]
        public IEnumerable<Vinted> Get(string brandId, string catalogId, string colorId,
            string status, string sizeId , string priceTo)
        {
            return _repository.GetProductList(brandId, catalogId, colorId, status, sizeId, priceTo);
        }
    }
}
