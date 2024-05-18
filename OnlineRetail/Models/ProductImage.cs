using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineRetail.Models
{
	[Table("productImage")]
	public class ProductImage
	{
        [Key, ForeignKey("Products")]
        public int productId { get; set; }

		public string? imageUrl { get; set; }

		public Products? Products { get; set; }
	}
}

