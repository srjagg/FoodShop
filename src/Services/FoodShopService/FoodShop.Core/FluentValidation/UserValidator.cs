using FluentValidation;
using FoodShop.Model.ModelsDto;
using FoodShop.Repository.RepositoryInterface;

namespace FoodShop.Core.FluentValidation
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        private readonly IUserRepository _userRepository;
        public UserValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("El nombre del usuario no puede estar vacío");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("El correo electrónico del usuario no puede estar vacío")
                .EmailAddress().WithMessage("El correo electrónico no es válido")
                .MustAsync(async (email, CancellationToken) => await _userRepository.IsEmailUnique(email, CancellationToken))
                .WithMessage(user => $"El correo electrónico: '{user.Email}' ya está registrado");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("La contraseña del usuario no puede estar vacía");
        }
    }
}
