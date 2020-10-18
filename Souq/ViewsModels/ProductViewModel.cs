using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Souq.ViewsModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        public String productName { get; set; }
        [Required]
        public Category category { get; set; }
        public enum Category
        {
            Laptop,
            Mobile
        };
        [Required]
        public string productBrand { get; set; }
        [Required]
        public double productPrice { get; set; }
        [Required]
        public string ProductDiskUsage { get; set; }
        [Required]
        public string ProductRamUsage { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Proudct Picture")]
        public IFormFile productImg { get; set; }

    }
}
