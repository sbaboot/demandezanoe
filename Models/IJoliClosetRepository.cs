using demandezanoe.Models.Entities;
using System.Collections.Generic;

namespace demandezanoe.Models
{
    public interface IJoliClosetRepository
    {
        List<JoliCloset> GetProductList(string catalog = "0", string brand = "0", string modele = "0");
    }
}
