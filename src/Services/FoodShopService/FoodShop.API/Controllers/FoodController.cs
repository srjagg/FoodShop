using FoodShop.Core.CoreInterface;
using FoodShop.Model.Models;
using FoodShop.Model.ModelsDto;
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
        public async Task<IActionResult> AddFoodAsync([FromBody] FoodDto foodDto)
        {
            var result = await _foodCore.AddFoodAsync(foodDto);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateFoodAsync/{foodId}")]
        public async Task<IActionResult> UpdateFoodAsync(int foodId, [FromBody] FoodDto foodDto)
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
        public async Task<IActionResult> GetAllFoodAsync()
        {
            var result = await _foodCore.GetAllFoodAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("GetAvailableFoods")]
        public async Task<IActionResult> GetAvailableFoods()
        {
            var result = await _foodCore.GetAvailableFoods();
            return Ok(result);
        }
    }
}
