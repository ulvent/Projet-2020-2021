using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace bladelinkv2.Models
{
    public class Client
    {
        public string Name { get; set; }
        public string Fname { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string password { get; set; }
        [Key]
        public int ID_cli { get; set; }
    }
}