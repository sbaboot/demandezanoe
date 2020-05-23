using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demandezanoe.Models
{
    public interface IVintedRepository
    {
        List<Vinted> GetProductList(string catalogId = null, string brandId = null, string colorId = null,
            string status = null, string priceFrom = null, string priceTo = null, string textarea = null);
    }
}
