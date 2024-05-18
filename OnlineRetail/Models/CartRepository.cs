using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace OnlineRetail.Models
{
    public class CartRepository : Repository
    {
        public CartRepository(OnlineRetailContext context) : base(context)
        {
        }
        public async Task<int> AddCart(Cart obj)
        {
            const int MaxRetryCount = 5;
            const int DelayMilliseconds = 100;
            int affectedRows = 0;

            for (int retry = 0; retry < MaxRetryCount; retry++)
            {
                try
                {
                    // Begin a transaction
                    using (var transaction = await context.Database.BeginTransactionAsync())
                    {
                        // Insert the cart data without the total price
                        string insertSql = @"
                    INSERT INTO carts (amount, productId)
                    VALUES (@amount, @productId);
                ";

                        var insertParameters = new List<SqliteParameter>
                {
                    new SqliteParameter("@amount", obj.amount),
                    new SqliteParameter("@productId", obj.productId)
                };

                        // Execute the initial insert command
                        await context.Database.ExecuteSqlRawAsync(insertSql, insertParameters);

                        // Update the total price based on the inserted row
                        string updateSql = @"
                    UPDATE carts
                   
                SET totalPrice = (
                SELECT SUM(p.price * c.amount)
                FROM products p
                JOIN carts c ON p.id = c.productId
                WHERE c.productId = @productId AND c.totalPrice IS NULL
            )
                    WHERE productId = @productId AND totalPrice IS NULL;
                ";

                        var updateParameters = new List<SqliteParameter>
                {
                    new SqliteParameter("@amount", obj.amount),
                    new SqliteParameter("@productId", obj.productId)
                };

                        // Execute the update command
                        affectedRows = await context.Database.ExecuteSqlRawAsync(updateSql, updateParameters);

                        // Commit the transaction
                        await transaction.CommitAsync();

                        // Break the loop if the transaction was successful
                        break;
                    }
                }
                catch (SqliteException ex) when (ex.SqliteErrorCode == 5) // SQLITE_BUSY
                {
                    if (retry == MaxRetryCount - 1)
                        throw;

                    // Wait before retrying
                    await Task.Delay(DelayMilliseconds);
                }
            }

            return affectedRows;
        }
    }
}
