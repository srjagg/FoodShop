using FluentValidation;
using FoodShop.Core.CoreInterface;
using FoodShop.Core.FluentValidation;
using FoodShop.Core.Util;
using FoodShop.Model.Models;
using FoodShop.Model.ModelsDto;
using FoodShop.Repository.RepositoryInterface;

namespace FoodShop.Core.CoreImplement
{
    public class UserCore : IUserCore
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly UserValidator _userValidator;

        private string urlApi = "/User/AddUserAsync";
        private string module = "UserRepository";

        public UserCore(IUserRepository userRepository, IPasswordHasher passwordHasher, UserValidator userValidator)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _userValidator = userValidator;
        }
        public async Task<PetitionResponse<int>> AddUserAsync(UserDto userModel)
        {
            try
            {
                var validationResult = await _userValidator.ValidateAsync(userModel);
                if (!validationResult.IsValid)
                {
                    var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return new PetitionResponse<int>
                    {
                        Success = false,
                        Message = $"Error de validación: {errorMessages}",
                        Module = module,
                        URL = urlApi,
                        Result = 1
                    };
                }

                userModel.Password = _passwordHasher.HashPassword(userModel.Password);

                var user = new User
                {
                    Name = userModel.Name,
                    Email = userModel.Email,
                    Password = userModel.Password,
                    IsAdmin = userModel.IsAdmin
                };

                await _userRepository.AddUserAsync(user);

                return new PetitionResponse<int>
                {
                    Success = true,
                    Message = "Usuario agregado exitosamente",
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
                    Message = $"Error al agregar el usuario: {ex.Message}",
                    Module = module,
                    URL = urlApi,
                    Result = 1
                };
            }
        }
    }
}
