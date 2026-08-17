namespace Business.Application.Validators.Maestros;

using FluentValidation;
using Business.Application.DTOs.Maestros;

public class CreateClienteValidator : AbstractValidator<CreateClienteDto>
{
    public CreateClienteValidator()
    {
        RuleFor(x => x.RazonSocial).NotEmpty().MaximumLength(200);
        RuleFor(x => x.RUC).NotEmpty().Length(11).WithMessage("El RUC debe tener 11 dígitos");
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.LimiteCredito).GreaterThanOrEqualTo(0);
    }
}
