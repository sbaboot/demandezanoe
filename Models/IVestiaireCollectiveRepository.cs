using demandezanoe.Models.Entities;
using System.Collections.Generic;

namespace demandezanoe.Models
{
    public interface IVestiaireCollectiveRepository
    {
        List<VestaireCollective> GetProductList(string catalog = "0", string brand = "0", string color = "0",
            string condition = "0", string priceFrom = "0", string priceTo = "0", string modele = "0");
    }
}