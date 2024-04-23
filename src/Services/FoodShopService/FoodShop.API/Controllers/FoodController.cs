using FoodShop.Core.CoreInterface;
using FoodShop.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodCore _foodCore;

        public FoodController(IFoodCore foodCore)
        {
            _foodCore = foodCore;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddFoodAsync")]
        public async Task<IActionResult> AddFoodAsync([FromBody] Food food)
        {
            var result = await _foodCore.AddFoodAsync(food);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateFoodAsync/{foodId}")]
        public async Task<IActionResult> UpdateFoodAsync(int foodId, [FromBody] Food foodDto)
        {
            var result = await _foodCore.UpdateFoodAsync(foodId, foodDto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteFoodAsync/{foodId}")]
        public async Task<IActionResult> DeleteFoodAsync(int foodId)
        {
            var result = await _foodCore.DeleteFoodAsync(foodId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("GetAllFoodAsync")]
        public async Task<IActionResult> GetAllFood()
        {
            var result = await _foodCore.GetAllFoodAsync();
            return Ok(result);
        }
    }
}
