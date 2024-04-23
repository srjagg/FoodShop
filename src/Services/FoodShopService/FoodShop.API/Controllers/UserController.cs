using FoodShop.Core.CoreInterface;
using FoodShop.Model.ModelsDto;
using Microsoft.AspNetCore.Mvc;

namespace FoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserCore _userCore;

        public UserController(IUserCore userCore) 
        { 
            _userCore = userCore;
        }

        [HttpPost("AddUserAsync")]
        public async Task<IActionResult> AddUserAsync(UserDto userModel)
        {
            var result = await _userCore.AddUserAsync(userModel);

            return Ok(result);
        }
    }
}
