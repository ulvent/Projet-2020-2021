using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace bladelinkv2.Models
{
    public class Product
    {
        public string Name_prod { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public String type { get; set; }
        [Key]
        public int ID_prod { get; set; }
    }
}