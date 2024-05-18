using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineRetail.Models
{
	[Table("categories")]
	public class Category
	{
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

		public string? categoryName { get; set; }

		public List<Products> Products { get; set; }
	}
}

