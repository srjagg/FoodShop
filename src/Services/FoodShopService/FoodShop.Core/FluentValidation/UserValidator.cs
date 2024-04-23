using FluentValidation;
using FoodShop.Model.ModelsDto;

namespace FoodShop.Core.FluentValidation
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("El nombre del usuario no puede estar vacío");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("El correo electrónico del usuario no puede estar vacío")
                .EmailAddress().WithMessage("El correo electrónico no es válido");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("La contraseña del usuario no puede estar vacía");
        }
    }
}
