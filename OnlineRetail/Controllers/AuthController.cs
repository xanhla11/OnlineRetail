using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineRetail.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        SiteProvider provider;
        public AuthController(SiteProvider provider)
        {
            this.provider = provider;
        }

        [HttpPost("login")]
        public object Login(LoginModel obj)
        {
            Member member = provider.Member.Login(obj);
            if(member != null)
            {
                return new
                {
                    Member = member
                };
            }
            return null;
        }


        [HttpPost("register")]
        public async Task<int> Register(RegisterModel obj)
        {
            if (string.IsNullOrEmpty(obj.memberId))
            {
                obj.memberId = Guid.NewGuid().ToString(); // Generating a random string for memberId
            }
            return await provider.Member.Add(obj);
        }
    }
}

