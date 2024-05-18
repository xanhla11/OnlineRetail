
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace OnlineRetail.Models
{
    public class ProductRepository : Repository
    {
        public ProductRepository(OnlineRetailContext context) : base(context)
        {
        }
        public async Task<List<Products>> GetProductsAsync()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<IEnumerable<object>> SortProducts(decimal maxPrice, string comparison)
        {
            string sql = @"
            SELECT *
            FROM products p
            LEFT JOIN productImage pi ON p.id = pi.productId 
             WHERE
            ";
            switch (comparison)
            {
                case "<":
                    sql += "price < @MaxPrice";
                break;
                case ">":
                    sql += "price > @MaxPrice";
                    break;
                default:
                    throw new ArgumentException("Invalid comparison operator");
            }
            var products = await context.Products.FromSqlRaw(sql,
                new SqliteParameter("@MaxPrice", maxPrice))
                .Include(p => p.ProductImages)
                .ToListAsync();
            var result = products.Select(p => new
            {
                id = p.id,
                name = p.name,
                price = p.price,
                categoryId = p.categoryId,
                imageUrl = p.ProductImages.FirstOrDefault()?.imageUrl
            }).ToList();

            return result;
        }
           
    public async Task<IEnumerable<object>> SearchProducts(string name, int page, int size)
        {
            var sql = @"
        SELECT p.*, pi.imageUrl 
        FROM products p
        LEFT JOIN productImage pi ON p.id = pi.productId 
        WHERE p.name LIKE @Name
        LIMIT @Size OFFSET @Offset";

            var products = await context.Products.FromSqlRaw(sql,
                new SqliteParameter("@Name", $"%{name}%"),
                new SqliteParameter("@Size", size),
                new SqliteParameter("@Offset", (page - 1) * size))
                .Include(p => p.ProductImages)
                .ToListAsync();

            var result = products.Select(p => new 
            {
                id = p.id,
                name = p.name,
                price = p.price,
                categoryId = p.categoryId,
                imageUrl = p.ProductImages.FirstOrDefault()?.imageUrl
            }).ToList();

            return result;
        }

        public async Task<int> DeleteProduct(int id)
        {
            return await context.Database.ExecuteSqlRawAsync("DELETE FROM products WHERE id=@id", new SqliteParameter("@id", id));
        }

        public async Task<int> UpdateProduct(Products obj, int id)
        {
            // Ensure that the provided product object has at least one image
            if (obj.ProductImages == null || !obj.ProductImages.Any())
            {
                throw new ArgumentException("Product must have at least one image.");
            }
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Perform database operations within the transaction scope

                    // Insert data into the products table
                    await context.Database.ExecuteSqlRawAsync(
                        "UPDATE products SET name = @name, categoryId = @categoryId , price = @price WHERE id=@id",
                        new SqliteParameter("@id", id),
                        new SqliteParameter("@name", obj.name),
                        new SqliteParameter("@categoryId", obj.categoryId),
                        new SqliteParameter("@price", obj.price)
                    );

                    // Retrieve the generated primary key (id) of the inserted product
                    var productId = await context.Products
                        .OrderByDescending(p => p.id)
                        .Select(p => p.id)
                        .FirstOrDefaultAsync();

                    // Insert data into the productImage table
                    await context.Database.ExecuteSqlRawAsync(
                        "UPDATE productImage SET imageUrl=@imageUrl WHERE productId = @productId",
                        new SqliteParameter("@productId", productId),
                        new SqliteParameter("@imageUrl", obj.ProductImages.FirstOrDefault().imageUrl)
                    );

                    // Commit the transaction
                    await transaction.CommitAsync();

                    return 1; // Return 1 to indicate success
                }
                catch (Exception)
                {
                    // Rollback the transaction if an exception occurs
                    await transaction.RollbackAsync();
                    throw; // Re-throw the exception
                }
            }

        }

        public async Task<int> AddProduct(Products obj)
        {
            // Ensure that the provided product object has at least one image
            if (obj.ProductImages == null || !obj.ProductImages.Any())
            {
                throw new ArgumentException("Product must have at least one image.");
            }

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Perform database operations within the transaction scope

                    // Insert data into the products table
                    await context.Database.ExecuteSqlRawAsync(
                        "INSERT INTO products (name, categoryId, price) VALUES (@name, @categoryId, @price)",
                        new SqliteParameter("@name", obj.name),
                        new SqliteParameter("@categoryId", obj.categoryId),
                        new SqliteParameter("@price", obj.price)
                    );

                    // Retrieve the generated primary key (id) of the inserted product
                    var productId = await context.Products
                        .OrderByDescending(p => p.id)
                        .Select(p => p.id)
                        .FirstOrDefaultAsync();

                    // Insert data into the productImage table
                    await context.Database.ExecuteSqlRawAsync(
                        "INSERT INTO productImage (productId, imageUrl) VALUES (@productId, @imageUrl)",
                        new SqliteParameter("@productId", productId),
                        new SqliteParameter("@imageUrl", obj.ProductImages.FirstOrDefault().imageUrl)
                    );

                    // Commit the transaction
                    await transaction.CommitAsync();

                    return 1; // Return 1 to indicate success
                }
                catch (Exception)
                {
                    // Rollback the transaction if an exception occurs
                    await transaction.RollbackAsync();
                    throw; // Re-throw the exception
                }
            }
        }


        public async Task<IEnumerable<object>> GetProductsWithImage(int page, int size)
		{
			return await context.Products.Include(p => p.ProductImages).Select(p => new
			{
				p.id,
				p.name,
				p.price,
				p.categoryId,
				imageUrl = p.ProductImages.FirstOrDefault().imageUrl
			}).Skip((page - 1) * size).Take(size).ToListAsync();
		}
	}
}

