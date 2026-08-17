namespace Business.Application.Validators.Auth;

using FluentValidation;
using Business.Application.DTOs.Auth;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("El usuario es requerido");
        RuleFor(x => x.Password).NotEmpty().WithMessage("La contraseña es requerida");
    }
}
