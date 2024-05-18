using Microsoft.AspNetCore.Mvc;
using OnlineRetail.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        SiteProvider provider;
        public CategoryController(SiteProvider provider)
        {
            this.provider = provider;
        }
        [HttpGet]
        public async Task<List<Category>> GetCategories()
        {
            return await provider.Category.GetCategoriesAsync();
        }
        [HttpGet("{id}")]
        public async Task<List<Category>> GetCategoriesById(int id)
        {
            return await provider.Category.GetCategoriesById(id);
        }

    }
}

