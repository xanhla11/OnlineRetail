
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineRetail.Models
{
	[Table("products")]
	public class Products
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

		public string? name { get; set; }

        
       public int categoryId { get; set; }

		public float price { get; set; }

        //[ForeignKey("categoryId")]
        //public Category Category { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
    }
}

