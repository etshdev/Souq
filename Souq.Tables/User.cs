using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Souq.Tables
{
   public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public double balance { get; set; }
        public virtual IdentityUser AppUser { get; set; }
        [ForeignKey("IdentityUser")]
        public string AppUserId { get; set; }

    }
}
