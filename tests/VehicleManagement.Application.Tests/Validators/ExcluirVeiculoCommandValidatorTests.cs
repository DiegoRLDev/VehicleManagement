using VehicleManagement.Application.Veiculos.Commands.Excluir;

namespace VehicleManagement.Application.Tests.Validators;

public class ExcluirVeiculoCommandValidatorTests
{
    private readonly ExcluirVeiculoCommandValidator _validator = new();

    [Fact]
    public void Id_Empty_Falha()
    {
        var result = _validator.Validate(new ExcluirVeiculoCommand(Guid.Empty));

        Assert.Contains(result.Errors, e => e.PropertyName == nameof(ExcluirVeiculoCommand.Id));
    }

    [Fact]
    public void Id_Valido_Passa()
    {
        var result = _validator.Validate(new ExcluirVeiculoCommand(Guid.NewGuid()));

        Assert.True(result.IsValid);
    }
}
