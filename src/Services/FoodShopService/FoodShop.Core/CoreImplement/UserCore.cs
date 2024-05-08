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
     
        private string module = "UserCore";

        public UserCore(IUserRepository userRepository, IPasswordHasher passwordHasher, UserValidator userValidator)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _userValidator = userValidator;
        }
        public async Task<PetitionResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            string urlApi = "/User/GetAllUsersAsync";
            try
            {
                var allUsers = await _userRepository.GetAllUsersAsync();

                var userDto = allUsers.Select(user => new UserDto
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    IsAdmin = user.IsAdmin,
                });

                return new PetitionResponse<IEnumerable<UserDto>>
                {
                    Success = true,
                    Message = "Usuarios obtenidos exitosamente",
                    Module = module,
                    Result = userDto
                };
            }
            catch (Exception ex)
            {
                return new PetitionResponse<IEnumerable<UserDto>>
                {
                    Success = false,
                    Message = $"Error al consultar los usuarios: {ex.Message}",
                    Module = module,
                    URL = urlApi,
                    Result = null
                };
            }
        }


        public async Task<PetitionResponse<int>> AddUserAsync(UserDto userModel)
        {
            string urlApi = "/User/AddUserAsync";
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
                        Result = 0
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

                var userId = await _userRepository.AddUserAsync(user);

                return new PetitionResponse<int>
                {
                    Success = true,
                    Message = "Usuario agregado exitosamente",
                    Module = module,
                    URL = urlApi,
                    Result = userId
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
                    Result = 0
                };
            }
        }
    }
}
