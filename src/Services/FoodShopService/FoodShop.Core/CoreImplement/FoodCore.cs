﻿using FoodShop.Core.CoreInterface;
using FoodShop.Core.FluentValidation;
using FoodShop.Core.Util;
using FoodShop.Model.Models;
using FoodShop.Model.ModelsDto;
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

        public async Task<PetitionResponse<int>> AddFoodAsync(FoodDto foodDto)
        {
            string urlApi = "/Food/AddFoodAsync";
            try
            {
                var validationResult = await _foodValidator.ValidateAsync(foodDto);
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
                var food = new Food
                {
                    Name = foodDto.Name,
                    Description = foodDto.Description,
                    Price = foodDto.Price,
                    AvailableQuantity = foodDto.AvailableQuantity,
                };

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


        public async Task<PetitionResponse<bool>> UpdateFoodAsync(int foodId, FoodDto foodDto)
        {
            string urlApi = "/Food/UpdateFoodAsync";

            try
            {
                var existingFood = await _unitOfWork.FoodRepository.GetFoodByIdAsync(foodId);
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

                var validationResult = await _foodValidator.ValidateAsync(foodDto);
                if (!validationResult.IsValid)
                {
                    return new PetitionResponse<bool>
                    {
                        Success = false,
                        Message = $"Error de validación: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}",
                        Module = module
                    };
                }

                existingFood.Name = foodDto.Name;
                existingFood.Description = foodDto.Description;
                existingFood.Price = foodDto.Price;
                existingFood.AvailableQuantity = foodDto.AvailableQuantity;

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
                var existingFood = await _unitOfWork.FoodRepository.GetFoodByIdAsync(foodId);
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

        public async Task<PetitionResponse<IEnumerable<FoodDto>>> GetAllFoodAsync()
        {
            string urlApi = "/Food/GetAllFoodAsync";
            try
            {
                var allFood = await _unitOfWork.FoodRepository.GetAllAsync();

                var foodDtos = allFood.Select(food => new FoodDto
                {
                    FoodId = food.FoodId,
                    Name = food.Name,
                    Description = food.Description,
                    Price = food.Price,
                    AvailableQuantity = food.AvailableQuantity
                });

                return new PetitionResponse<IEnumerable<FoodDto>>
                {
                    Success = true,
                    Message = "Alimentos obtenidos exitosamente",
                    Module = module,
                    Result = foodDtos
                };
            }
            catch (Exception ex)
            {
                return new PetitionResponse<IEnumerable<FoodDto>>
                {
                    Success = false,
                    Message = $"Error al consultar los alimentos: {ex.Message}",
                    Module = module,
                    URL = urlApi,
                    Result = null
                };
            }
        }

        public async Task<PetitionResponse<List<FoodDto>>> GetAvailableFoods()
        {
            string urlApi = "/Food/GetAvailableFoods";
            try
            {
                var availableFoods = await _unitOfWork.FoodRepository.GetAvailableFoods();

                var foodDtos = availableFoods.Select(food => new FoodDto
                {
                    FoodId = food.FoodId,
                    Name = food.Name,
                    Description = food.Description,
                    Price = food.Price,
                    AvailableQuantity = food.AvailableQuantity
                }).ToList();

                return new PetitionResponse<List<FoodDto>>
                {
                    Success = true,
                    Message = "Alimentos obtenidos exitosamente",
                    Module = module,
                    Result = foodDtos
                }; 
            }
            catch (Exception ex)
            {
                return new PetitionResponse<List<FoodDto>>
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
