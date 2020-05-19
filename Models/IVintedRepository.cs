using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demandezanoe.Models
{
    public interface IVintedRepository
    {
        List<Vinted> GetProductList(string brandId = null, string catalogId = null, string colorId = null,
            string status = null, string sizeId = null, string priceTo = null);
    }
}
