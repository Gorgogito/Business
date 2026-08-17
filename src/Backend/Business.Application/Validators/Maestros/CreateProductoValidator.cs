namespace Business.Application.Validators.Maestros;

using FluentValidation;
using Business.Application.DTOs.Maestros;

public class CreateProductoValidator : AbstractValidator<CreateProductoDto>
{
    public CreateProductoValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(200);
        RuleFor(x => x.PrecioCompra).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PrecioVenta).GreaterThan(0);
        RuleFor(x => x.CategoriaId).GreaterThan(0);
    }
}
