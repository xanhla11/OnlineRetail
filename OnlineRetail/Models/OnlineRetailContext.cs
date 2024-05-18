
using Microsoft.EntityFrameworkCore;

namespace OnlineRetail.Models
{
	public class OnlineRetailContext :DbContext
	{
        IConfiguration configuration;
        public OnlineRetailContext(IConfiguration configuration)
		{
            this.configuration = configuration;
        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Products> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite(configuration.GetConnectionString("OnlineRetail"));

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductImage>().HasKey(p => new { p.productId, p.imageUrl });
        }
    }
}

