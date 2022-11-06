using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _12864MVCApplication.Models
{
    public class Category
    {
        [Display(Name = "Category Id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}