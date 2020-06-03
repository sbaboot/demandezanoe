﻿using System.Collections.Generic;

namespace demandezanoe.Models
{
    public interface IVintedRepository
    {
        List<Vinted> GetProductList(string catalogId = "0", string brandId = "0", string colorId = "0",
            string status = "0", string priceFrom = "0", string priceTo = "0", string textarea = "0");
    }
}
