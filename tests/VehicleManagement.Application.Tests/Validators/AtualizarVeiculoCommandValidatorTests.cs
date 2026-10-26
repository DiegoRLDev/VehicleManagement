using VehicleManagement.Application.Veiculos.Commands.Atualizar;
using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Application.Tests.Validators;

public class AtualizarVeiculoCommandValidatorTests
{
    private readonly AtualizarVeiculoCommandValidator _validator = new();

    [Fact]
    public void Atualizacao_Valida_Passa()
    {
        var result = _validator.Validate(
            new AtualizarVeiculoCommand(Guid.NewGuid(), "Corolla", Marca.Toyota, "Corolla", null, 10m));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Id_Empty_Falha()
    {
        var result = _validator.Validate(
            new AtualizarVeiculoCommand(Guid.Empty, "Corolla", Marca.Toyota, "Corolla", null, null));

        Assert.Contains(result.Errors, e => e.PropertyName == nameof(AtualizarVeiculoCommand.Id));
    }

    [Fact]
    public void Valor_Negativo_Falha()
    {
        var result = _validator.Validate(
            new AtualizarVeiculoCommand(Guid.NewGuid(), "Corolla", Marca.Toyota, "Corolla", null, -5m));

        Assert.Contains(result.Errors, e => e.PropertyName == nameof(AtualizarVeiculoCommand.Valor));
    }
}
