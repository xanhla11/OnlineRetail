
using Microsoft.EntityFrameworkCore;

namespace OnlineRetail.Models
{
    public class CategoryRepository : Repository
    {
        public CategoryRepository(OnlineRetailContext context) : base(context)
        {
        }
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesById(int id)
        {
            return await context.Categories.Include(p => p.Products).Where(p => p.id == id).ToListAsync();
        }
    }
}

