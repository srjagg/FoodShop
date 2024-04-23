using FoodShop.Core.CoreInterface;
using FoodShop.Model.ModelsDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderCore _orderCore;

        public OrderController(IOrderCore orderCore)
        {
            _orderCore = orderCore;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost("PlaceOrderAsync")]
        public async Task<IActionResult> PlaceOrderAsync(OrderDto orderDto)
        {
            var result = await _orderCore.PlaceOrderAsync(orderDto);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
