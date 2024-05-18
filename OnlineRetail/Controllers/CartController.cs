using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineRetail.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        SiteProvider provider;
        public CartController(SiteProvider provider)
        {
            this.provider = provider;
        }

        [HttpPost]
        public async Task<int> AddCart(Cart obj)
        {
            return await provider.Carts.AddCart(obj);
        }
    }
}

