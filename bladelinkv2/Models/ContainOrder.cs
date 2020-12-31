using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace bladelinkv2.Models
{
    public class ContainOrder
    {
        [Key]
        public int ID_Cont { get; set; }
        public virtual int ID_Product { get; set; }
        public virtual int ID_Comm { get; set; }
        public Product p { get;set; }
    }
}