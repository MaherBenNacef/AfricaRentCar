using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfricaRentCar.Models
{
    public class facture
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public ApplicationUser user { get; set; }
        public float somme { get; set; }
    }
}