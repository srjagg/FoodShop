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
        private readonly IOrderDetailCore _orderDetailCore;
        private readonly EmailCore _emailCore;

        public OrderCore(IUnitOfWork unitOfWork, IOrderDetailCore orderDetailCore, EmailCore emailCore)
        {
            _unitOfWork = unitOfWork;
            _orderDetailCore = orderDetailCore;
            _emailCore = emailCore;
        }
        public async Task<PetitionResponse<int>> PlaceOrderAsync(OrderDto orderDto)
        {
            try
            {
                // Verificar si el usuario existe
                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(orderDto.UserId);
                if (user == null)
                {
                    return new PetitionResponse<int>
                    {
                        Success = false,
                        Message = "Usuario no encontrado",
                        Module = "OrderCore",
                        Result = 0
                    };
                }

                // Verificar si hay detalles de pedido
                if (orderDto.OrderDetails == null || !orderDto.OrderDetails.Any())
                {
                    return new PetitionResponse<int>
                    {
                        Success = false,
                        Message = "No se proporcionaron detalles de pedido",
                        Module = "OrderCore",
                        Result = 0
                    };
                }

                // Verificar la disponibilidad de los alimentos en el catálogo
                foreach (var detail in orderDto.OrderDetails)
                {
                    var food = await _unitOfWork.FoodRepository.GetFoodByIdAsync(detail.FoodId);
                    if (food == null || food.AvailableQuantity < detail.Quantity)
                    {
                        return new PetitionResponse<int>
                        {
                            Success = false,
                            Message = "Alimento no disponible en el catálogo o cantidad insuficiente",
                            Module = "OrderCore",
                            Result = 0
                        };
                    }
                }

                // Crear la orden
                var order = new Order
                {
                    UserId = orderDto.UserId,
                    OrderDate = DateTime.Now,
                    Total = orderDto.OrderDetails.Sum(d => d.Quantity * d.UnitPrice),
                    OrderDetails = orderDto.OrderDetails.Select(d => new OrderDetail
                    {
                        FoodId = d.FoodId,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice
                    }).ToList()
                };

                // Agregar la orden a la base de datos
                var orderId = await _unitOfWork.OrderRepository.AddOrderAsync(order);

                // Actualizar la disponibilidad de alimentos en el catálogo
                foreach (var orderItem in orderDto.OrderDetails)
                {
                    var food = await _unitOfWork.FoodRepository.GetFoodByIdAsync(orderItem.FoodId);
                    if (food != null)
                    {
                        food.AvailableQuantity -= orderItem.Quantity;
                        await _unitOfWork.FoodRepository.UpdateFoodAsync(food);
                    }

                }

                var orderDetails = GetOrderDetails(orderDto); 
                await _emailCore.SendOrderConfirmationEmailAsync(user.Email, orderDetails, "Confirmación de Pedido");

                foreach (var orderDetailDto in orderDto.OrderDetails)
                {

                    orderDetailDto.OrderId = order.OrderId;
                    var response = await _orderDetailCore.AddOrderDetailAsync(orderDetailDto);
                    if (!response.Success)
                    {
                        // Manejar el error si falla al guardar el detalle del pedido
                        return new PetitionResponse<int>
                        {
                            Success = false,
                            Message = $"Error al guardar el detalle del pedido: {response.Message}",
                            Module = "OrderCore",
                            Result = 0
                        };
                    }
                }


                return new PetitionResponse<int>
                {
                    Success = true,
                    Message = "Pedido realizado exitosamente",
                    Module = "OrderCore",
                    Result = order.OrderId
                };
            }
            catch (Exception ex)
            {
                return new PetitionResponse<int>
                {
                    Success = false,
                    Message = $"Error al generar el pedido: {ex.Message}",
                    Module = "OrderCore",
                    Result = 0
                };
            }
            
        }

        private string GetOrderDetails(OrderDto orderDto)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Detalles del pedido:");

            foreach (var orderDetail in orderDto.OrderDetails)
            {
                sb.AppendLine($"- {orderDetail.Quantity} x {orderDetail.FoodName}: ${orderDetail.Quantity * orderDetail.UnitPrice}");
            }

            sb.AppendLine($"Total: ${orderDto.OrderDetails.Sum(d => d.Quantity * d.UnitPrice)}");

            return sb.ToString();
        }

    }
}
