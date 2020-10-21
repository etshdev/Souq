using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Souq.Tables
{
   public class Cart
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual IdentityUser AppUser { get; set; }
        [ForeignKey("IdentityUser")]
        public string AppUserId { get; set; }

        public virtual ICollection<CartDetailes> CartDetailes { get; set; }

    }
}
