using FoodShop.Core.CoreInterface;
using FoodShop.Core.Util;
using FoodShop.Model.Models;
using FoodShop.Model.ModelsDto;
using FoodShop.UnitOfWork;

namespace FoodShop.Core.CoreImplement
{
    public class OrderDetailCore : IOrderDetailCore
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailCore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PetitionResponse<int>> AddOrderDetailAsync(OrderDetailDto orderDetailDto)
        {
            try
            {
                var food = await _unitOfWork.FoodRepository.GetFoodByIdAsync(orderDetailDto.FoodId);
                if (food == null)
                {
                    return new PetitionResponse<int>
                    {
                        Success = false,
                        Message = "El alimento no existe en el catálogo",
                        Module = "OrderDetailCore",
                        Result = 0
                    };
                }

                var orderDetail = new OrderDetail
                {
                    OrderId = orderDetailDto.OrderId,
                    FoodId = orderDetailDto.FoodId,
                    Quantity = orderDetailDto.Quantity,
                    UnitPrice = food.Price
                };

                await _unitOfWork.OrderDetailRepository.AddOrderDetailAsync(orderDetail);

                return new PetitionResponse<int>
                {
                    Success = true,
                    Message = "Detalle de orden agregado exitosamente",
                    Module = "OrderDetailCore",
                    Result = orderDetail.OrderDetailId
                };
            }
            catch (Exception ex)
            {
                return new PetitionResponse<int>
                {
                    Success = false,
                    Message = $"Error al agregar el detalle de orden: {ex.Message}",
                    Module = "OrderDetailCore",
                    Result = 0
                };
            }
        }
    }
}
