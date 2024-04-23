using FluentValidation;
using FoodShop.Model.ModelsDto;

namespace FoodShop.Core.FluentValidation
{
    public class FoodValidator : AbstractValidator<FoodDto>
    {
        public FoodValidator()
        {
            RuleFor(food => food.Name).NotEmpty().WithMessage("El nombre del alimento es requerido.");
            RuleFor(food => food.Description).NotEmpty().WithMessage("La descripción del alimento es requerida.");
            RuleFor(food => food.Price).GreaterThan(0).WithMessage("El precio del alimento debe ser mayor que cero.");
            RuleFor(food => food.AvailableQuantity).GreaterThanOrEqualTo(0).WithMessage("La cantidad disponible del alimento debe ser mayor o igual a cero.");
        }
    }
}
