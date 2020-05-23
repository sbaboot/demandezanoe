using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demandezanoe.Models
{
    public class Vinted
    {
        public int Id { get; set; }
        
        public string Catalog { get; set; }

        public string Picture { get; set; }

        public string Link { get; set; }

        public string Brand { get; set; }

        public string Color { get; set; }

        public string ModelBag { get; set; }

        public Conditions Condition { get; set; }

        public string Price { get; set; }
    }
}

public enum Conditions
{
    NewWithLabel = 6,
    New = 1,
    VeryGoodState = 2,
    GoodState = 3,
    Satisfactory = 4
}