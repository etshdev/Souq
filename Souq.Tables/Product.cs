using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Souq.Tables
{
   public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public string productImgUrl { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual IdentityUser AppUser { get; set; }
        [ForeignKey("IdentityUser")]
        public string AppUserId { get; set; }


    }
}
