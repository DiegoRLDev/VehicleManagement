using FluentValidation;

namespace VehicleManagement.Application.Usuarios.Commands.Adicionar;

public class AdicionarUsuarioCommandValidator : AbstractValidator<AdicionarUsuarioCommand>
{
    public AdicionarUsuarioCommandValidator()
    {
        RuleFor(command => command.Nome)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(command => command.Login)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(command => command.Senha)
            .NotEmpty()
            .MinimumLength(6);
    }
}
