using FoodShop.Core.CoreInterface;
using FoodShop.Core.FluentValidation;
using FoodShop.Core.Util;
using FoodShop.Model.Models;
using FoodShop.UnitOfWork;

namespace FoodShop.Core.CoreImplement
{
    public class FoodCore : IFoodCore
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FoodValidator _foodValidator;

        private string module = "FoodCore";

        public FoodCore(IUnitOfWork unitOfWork, FoodValidator foodValidator)
        {
            _unitOfWork = unitOfWork;
            _foodValidator = foodValidator;
        }

        public async Task<PetitionResponse<int>> AddFoodAsync(Food food)
        {
            string urlApi = "/Food/AddFoodAsync";
            try
            {
                var validationResult = await _foodValidator.ValidateAsync(food);
                if (!validationResult.IsValid)
                {
                    return new PetitionResponse<int>
                    {
                        Success = false,
                        Message = $"Error de validación: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}",
                        Module = module,
                        URL = urlApi,
                        Result = 1
                    };
                }

                await _unitOfWork.FoodRepository.AddFoodAsync(food);

                return new PetitionResponse<int>
                {
                    Success = true,
                    Message = "Alimento agregado exitosamente",
                    Module = module,
                    URL = urlApi,
                    Result = 0
                };
            }
            catch (Exception ex)
            {
                return new PetitionResponse<int>
                {
                    Success = false,
                    Message = $"Error al agregar el Alimento: {ex.Message}",
                    Module = module,
                    URL = urlApi,
                    Result = 1
                };
            }
        }


        public async Task<PetitionResponse<bool>> UpdateFoodAsync(int foodId, Food food)
        {
            string urlApi = "/Food/UpdateFoodAsync";

            try
            {
                var existingFood = await _unitOfWork.FoodRepository.GetByIdAsync(foodId);
                if (existingFood == null)
                {
                    return new PetitionResponse<bool>
                    {
                        Success = false,
                        Message = "No se encontró el alimento especificado",
                        Module = module,
                        URL = urlApi
                    };
                }

                var validationResult = await _foodValidator.ValidateAsync(food);
                if (!validationResult.IsValid)
                {
                    return new PetitionResponse<bool>
                    {
                        Success = false,
                        Message = $"Error de validación: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}",
                        Module = module
                    };
                }

                existingFood.Name = food.Name;
                existingFood.Description = food.Description;
                existingFood.Price = food.Price;
                existingFood.AvailableQuantity = food.AvailableQuantity;

                await _unitOfWork.FoodRepository.UpdateFoodAsync(existingFood);

                return new PetitionResponse<bool>
                {
                    Success = true,
                    Message = "Alimento actualizado exitosamente",
                    Module = module,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new PetitionResponse<bool>
                {
                    Success = false,
                    Message = $"Error al actualizar el Alimento: {ex.Message}",
                    Module = module,
                    URL = urlApi,
                    Result = false
                };
            }           
        }

        public async Task<PetitionResponse<bool>> DeleteFoodAsync(int foodId)
        {
            string urlApi = "/Food/DeleteFoodAsync";
            try
            {
                var existingFood = await _unitOfWork.FoodRepository.GetByIdAsync(foodId);
                if (existingFood == null)
                {
                    return new PetitionResponse<bool>
                    {
                        Success = false,
                        Message = "No se encontró el alimento especificado",
                        Module = module
                    };
                }

                await _unitOfWork.FoodRepository.DeleteFoodAsync(existingFood);

                return new PetitionResponse<bool>
                {
                    Success = true,
                    Message = "Alimento eliminado exitosamente",
                    Module = module,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new PetitionResponse<bool>
                {
                    Success = false,
                    Message = $"Error al eliminar el Alimento: {ex.Message}",
                    Module = module,
                    URL = urlApi,
                    Result = false
                };
            }
        }

        public async Task<PetitionResponse<IEnumerable<Food>>> GetAllFoodAsync()
        {
            string urlApi = "/Food/GetAllFoodAsync";
            try
            {
                var allFood = await _unitOfWork.FoodRepository.GetAllAsync();

                return new PetitionResponse<IEnumerable<Food>>
                {
                    Success = true,
                    Message = "Alimentos obtenidos exitosamente",
                    Module = "FoodService",
                    Result = allFood
                };
            }
            catch (Exception ex)
            {
                return new PetitionResponse<IEnumerable<Food>>
                {
                    Success = false,
                    Message = $"Error al consultar los alimentos: {ex.Message}",
                    Module = module,
                    URL = urlApi,
                    Result = null
                };
            }
        }
    }
}
