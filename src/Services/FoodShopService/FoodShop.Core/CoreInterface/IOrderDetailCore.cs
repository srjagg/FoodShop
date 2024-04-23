using FoodShop.Core.Util;
using FoodShop.Model.ModelsDto;

namespace FoodShop.Core.CoreInterface
{
    public interface IOrderDetailCore
    {
        Task<PetitionResponse<int>> AddOrderDetailAsync(OrderDetailDto orderDetailDto);
    }
}
