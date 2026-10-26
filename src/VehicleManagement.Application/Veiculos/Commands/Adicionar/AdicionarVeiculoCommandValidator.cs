using FluentValidation;

namespace VehicleManagement.Application.Veiculos.Commands.Adicionar;

public class AdicionarVeiculoCommandValidator : AbstractValidator<AdicionarVeiculoCommand>
{
    public AdicionarVeiculoCommandValidator()
    {
        RuleFor(command => command.Descricao)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.Marca)
            .IsInEnum();

        RuleFor(command => command.Modelo)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(command => command.Valor)
            .GreaterThanOrEqualTo(0)
            .When(command => command.Valor.HasValue);
    }
}
