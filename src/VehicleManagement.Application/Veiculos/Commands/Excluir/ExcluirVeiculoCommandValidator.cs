using FluentValidation;

namespace VehicleManagement.Application.Veiculos.Commands.Excluir;

public class ExcluirVeiculoCommandValidator : AbstractValidator<ExcluirVeiculoCommand>
{
    public ExcluirVeiculoCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}
