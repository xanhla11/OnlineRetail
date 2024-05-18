using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineRetail.Models
{
	[Table("carts")]
	public class Cart
	{
        [Key]
        [Column("cardId")]
		public int Id { get; set; }

		public int amount { get; set; }

		public decimal totalPrice { get; set; }

		[ForeignKey("Products")]
		public int productId { get; set; }

        public Products? Product { get; set; }

    }
}

