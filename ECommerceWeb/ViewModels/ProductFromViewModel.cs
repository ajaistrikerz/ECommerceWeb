using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace ECommerceWeb.ViewModels
{
    public class ProductFromViewModel
    {
        public Tble_Product Product { get; set; }
        public IEnumerable<Tble_Category> Categories { get; set; }
    }
}