using FoodShop.Core.CoreInterface;
using FoodShop.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginCore _loginCore;

        public AccountController(IConfiguration configuration, ILoginCore loginCore)
        {
            _configuration = configuration;
            _loginCore = loginCore;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var tokenString = await _loginCore.IsLoggedIn(model);

            if (tokenString != null)
            {
                return Ok(new { token = tokenString });
            }

            return Unauthorized();
        }
    }
}
