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

        public string Owner { get; set; }

        public string Picture { get; set; }

        public string Link { get; set; }

        public string Brand { get; set; }

        public string Color { get; set; }

        public string ModelBag { get; set; }

        public Conditions Condition { get; set; }

        public double Price { get; set; }
    }
}

public enum Conditions
{
    NewWithLabel = 1,
    New = 2, 
    VeryGoodState = 3,
    GoodState = 4,
    Satisfactory = 5
}