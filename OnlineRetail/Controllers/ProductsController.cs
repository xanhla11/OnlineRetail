
using Microsoft.AspNetCore.Mvc;
using OnlineRetail.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineRetail.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        int size = 6;
        SiteProvider provider;
        public ProductsController(SiteProvider provider)
        {
            this.provider = provider;
        }
        [HttpGet("{p?}")]
        public async Task<IEnumerable<object>> GetProducts(int p =1)
        {
            return await provider.Products.GetProductsWithImage(p, size);
        }

        [HttpPost]
        public async Task<int> AddProduct(Products obj)
        {
            return await provider.Products.AddProduct(obj);
        }
        [HttpPut("{id}")]
        public async Task<int> UpdateProduct([FromBody] Products obj, int id)
        {
            return await provider.Products.UpdateProduct(obj,id);
        }
        [HttpDelete("{id}")]
        public async Task<int> DeleteProduct(int id)
        {
            return await provider.Products.DeleteProduct(id);
        }
        [HttpGet("search")]
        public async Task<IEnumerable<object>> SearchProducts(int page = 1, string name = "")
        {
            // Set a default value for size or retrieve it from your configuration
            //int size = 10;

            // Ensure page and size are valid
            if (page < 1 || size < 1)
            {
                throw new ArgumentException("Page and size must be greater than 0.");
            }

            // Call the search method from your provider
            return await provider.Products.SearchProducts(name, page, size);
        }
        [HttpGet("sort")]
        public async Task<IEnumerable<object>> SortProducts(decimal maxPrice, string comparison)
        {
            return await provider.Products.SortProducts(maxPrice, comparison);
        }
    }
}

