using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Souq.Tables
{
   public class CartDetailes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int productcount { get; set; }
        public virtual Product Product { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Cart Cart { get; set; }
        [ForeignKey("Cart")]
        public int CartId { get; set; }

    }
}
