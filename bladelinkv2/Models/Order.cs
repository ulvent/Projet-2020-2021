using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace bladelinkv2.Models
{
    public class Order
    {
        public virtual int Id_cli { get; set; }
        [Key]
        public int ID_comm1 { get; set; }
        public virtual List<ContainOrder> lp { get; set; }
    }
}