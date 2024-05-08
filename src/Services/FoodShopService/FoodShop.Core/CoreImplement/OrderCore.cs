using FoodShop.Core.CoreInterface;
using FoodShop.Core.Util;
using FoodShop.Model.Models;
using FoodShop.Model.ModelsDto;
using FoodShop.UnitOfWork;
using System.Text;

namespace FoodShop.Core.CoreImplement
{
    public class OrderCore : IOrderCore
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailCore _emailCore;

        public OrderCore(IUnitOfWork unitOfWork, EmailCore emailCore)
        {
            _unitOfWork = unitOfWork;
            _emailCore = emailCore;
        }
        public async Task<PetitionResponse<int>> PlaceOrderAsync(OrderDto orderDto)
        {
            try
            {
                var user = await GetUserOrReturnError(orderDto.UserId);
                if (user == null)
                    return UserNotFoundError();

                if (!HasOrderDetails(orderDto))
                    return NoOrderDetailsError();

                var unavailableFood = await GetUnavailableFood(orderDto.OrderDetails);
                if (unavailableFood.Any())
                    return FoodAvailabilityError();

                var order = CreateOrder(orderDto);
                var orderId = await AddOrderToDatabase(order);

                await UpdateFoodAvailability(orderDto.OrderDetails);

                await SendOrderConfirmationEmail(user, orderDto);

                return SuccessResponse(order.OrderId);
            }
            catch (Exception ex)
            {
                return GenericError(ex.Message);
            }
        }

        public async Task<PetitionResponse<List<OrderResponseDto>>> GetOrdersByUserEmailAsync(string userEmail)
        {
            try
            {
                var orders = await _unitOfWork.OrderRepository.GetOrdersByUserEmailAsync(userEmail);

                var orderResponseDtos = orders.Select(o => new OrderResponseDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    Total = o.Total,
                    UserName = o.User.Name,
                    UserEmail = o.User.Email,
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailResponseDto
                    {
                        FoodId = od.FoodId,
                        FoodName = od.Food.Name,
                        FoodPrice = od.Food.Price,
                        Quantity = od.Quantity
                    }).ToList()
                }).ToList();

                return new PetitionResponse<List<OrderResponseDto>>
                {
                    Success = true,
                    Message = "Órdenes obtenidas exitosamente",
                    Module = "OrderCore",
                    Result = orderResponseDtos
                };
            }
            catch (Exception ex)
            {
                return new PetitionResponse<List<OrderResponseDto>>
                {
                    Success = false,
                    Message = $"Error al obtener las órdenes: {ex.Message}",
                    Module = "OrderCore",
                    Result = null
                };
            }
        }

        //------------------------------------ MÉTODOS PRIVADOS -------------------------------------------------//

        private async Task<User?> GetUserOrReturnError(int userId)
        {
            return await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
        }

        private PetitionResponse<int> UserNotFoundError()
        {
            return new PetitionResponse<int>
            {
                Success = false,
                Message = "Usuario no encontrado",
                Module = "OrderCore",
                Result = 0
            };
        }

        private PetitionResponse<int> NoOrderDetailsError()
        {
            return new PetitionResponse<int>
            {
                Success = false,
                Message = "No se proporcionaron detalles de pedido",
                Module = "OrderCore",
                Result = 0
            };
        }

        private PetitionResponse<int> FoodAvailabilityError()
        {
            return new PetitionResponse<int>
            {
                Success = false,
                Message = "Alimento no disponible en el catálogo o cantidad insuficiente",
                Module = "OrderCore",
                Result = 0
            };
        }

        private PetitionResponse<int> OrderDetailSaveError(string errorMessage)
        {
            return new PetitionResponse<int>
            {
                Success = false,
                Message = errorMessage,
                Module = "OrderCore",
                Result = 0
            };
        }

        private PetitionResponse<int> SuccessResponse(int orderId)
        {
            return new PetitionResponse<int>
            {
                Success = true,
                Message = "Pedido realizado exitosamente",
                Module = "OrderCore",
                Result = orderId
            };
        }

        private PetitionResponse<int> GenericError(string errorMessage)
        {
            return new PetitionResponse<int>
            {
                Success = false,
                Message = $"Error al generar el pedido: {errorMessage}",
                Module = "OrderCore",
                Result = 0
            };
        }

        private bool HasOrderDetails(OrderDto orderDto)
        {
            return orderDto.OrderDetails != null && orderDto.OrderDetails.Any();
        }

        private async Task<List<int>> GetUnavailableFood(List<OrderDetailDto> orderDetails)
        {
            var unavailableFoodIds = new List<int>();
            foreach (var detail in orderDetails)
            {
                var food = await _unitOfWork.FoodRepository.GetFoodByIdAsync(detail.FoodId);
                if (food == null || food.AvailableQuantity < detail.Quantity)
                    unavailableFoodIds.Add(detail.FoodId);
            }
            return unavailableFoodIds;
        }
        private Order CreateOrder(OrderDto orderDto)
        {
            return new Order
            {
                UserId = orderDto.UserId,
                OrderDate = DateTime.Now,
                Total = orderDto.OrderDetails.Sum(d => d.Quantity * _unitOfWork.FoodRepository.GetFoodByIdAsync(d.FoodId).Result.Price),
                OrderDetails = orderDto.OrderDetails.Select(d =>
                {
                    var food = _unitOfWork.FoodRepository.GetFoodByIdAsync(d.FoodId).Result;
                    return new OrderDetail
                    {
                        FoodId = d.FoodId,
                        Quantity = d.Quantity,
                        UnitPrice = food.Price
                    };
                }).ToList()
            };
        }
        private async Task<int> AddOrderToDatabase(Order order)
        {
            return await _unitOfWork.OrderRepository.AddOrderAsync(order);
        }

        private async Task UpdateFoodAvailability(List<OrderDetailDto> orderDetails)
        {
            foreach (var orderItem in orderDetails)
            {
                var food = await _unitOfWork.FoodRepository.GetFoodByIdAsync(orderItem.FoodId);
                if (food != null)
                {
                    food.AvailableQuantity -= orderItem.Quantity;
                    await _unitOfWork.FoodRepository.UpdateFoodAsync(food);
                }
            }
        }

        private async Task SendOrderConfirmationEmail(User user, OrderDto orderDto)
        {
            var orderDetails = GetOrderDetails(orderDto);
            await _emailCore.SendOrderConfirmationEmailAsync(user.Email, orderDetails, "Confirmación de Pedido");
        }

        private string GetOrderDetails(OrderDto orderDto)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Detalles del pedido:");

            foreach (var orderDetail in orderDto.OrderDetails)
            {
                var food = _unitOfWork.FoodRepository.GetFoodByIdAsync(orderDetail.FoodId).Result;
                sb.AppendLine($"- {orderDetail.Quantity} x {food?.Name}: ${orderDetail.Quantity * food?.Price}");
            }

            sb.AppendLine($"Total: ${orderDto.OrderDetails.Sum(d => d.Quantity * _unitOfWork?.FoodRepository?.GetFoodByIdAsync(d.FoodId)?.Result?.Price)}");

            return sb.ToString();
        }

    }
}
