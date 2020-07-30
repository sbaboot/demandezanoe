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
    public class JoliClosetController : ControllerBase
    {
        private readonly IJoliClosetRepository _repository;
        public JoliClosetController(IJoliClosetRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<JoliCloset> Get(string catalog, string brand, string modele)
        {
            return _repository.GetProductList(catalog, brand, modele);
        }
    }
}