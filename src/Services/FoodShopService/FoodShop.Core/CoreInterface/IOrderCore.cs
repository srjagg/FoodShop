using FoodShop.Core.Util;
using FoodShop.Model.ModelsDto;

namespace FoodShop.Core.CoreInterface
{
    public interface IOrderCore
    {
        Task<PetitionResponse<int>> PlaceOrderAsync(OrderDto orderDto);
        Task<PetitionResponse<List<OrderResponseDto>>> GetOrdersByUserEmailAsync(string userEmail);
    }
}
