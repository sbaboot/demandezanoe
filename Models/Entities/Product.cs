﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demandezanoe.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Picture { get; set; }

        public string Link { get; set; }

        public string Brand { get; set; }

        public string Modele { get; set; }

        public string Price { get; set; }
    }
}

