using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _12864MVCApplication.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Category ProductCategory { get; set; }
    }
}