using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerceWeb.Models
{
    public class Item
    {
        public Tble_Product Product { get; set; }
        public int Quantity { get; set; }
    }
}