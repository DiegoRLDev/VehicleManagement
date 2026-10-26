using VehicleManagement.Application.Veiculos.Commands.Adicionar;
using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Application.Tests.Validators;

public class AdicionarVeiculoCommandValidatorTests
{
    private readonly AdicionarVeiculoCommandValidator _validator = new();

    [Fact]
    public void Cadastro_Valido_Passa()
    {
        var result = _validator.Validate(
            new AdicionarVeiculoCommand("Corolla XEi", Marca.Toyota, "Corolla", "Ar", 145000m));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Descricao_Vazia_Falha()
    {
        var result = _validator.Validate(
            new AdicionarVeiculoCommand("", Marca.Toyota, "Corolla", null, null));

        Assert.Contains(result.Errors, e => e.PropertyName == nameof(AdicionarVeiculoCommand.Descricao));
    }

    [Fact]
    public void Descricao_Acima_De_100_Caracteres_Falha()
    {
        var result = _validator.Validate(
            new AdicionarVeiculoCommand(new string('a', 101), Marca.Toyota, "Corolla", null, null));

        Assert.Contains(result.Errors, e => e.PropertyName == nameof(AdicionarVeiculoCommand.Descricao));
    }

    [Fact]
    public void Marca_Invalida_Falha()
    {
        var result = _validator.Validate(
            new AdicionarVeiculoCommand("Corolla", (Marca)999, "Corolla", null, null));

        Assert.Contains(result.Errors, e => e.PropertyName == nameof(AdicionarVeiculoCommand.Marca));
    }

    [Fact]
    public void Modelo_Vazio_Falha()
    {
        var result = _validator.Validate(
            new AdicionarVeiculoCommand("Corolla", Marca.Toyota, "", null, null));

        Assert.Contains(result.Errors, e => e.PropertyName == nameof(AdicionarVeiculoCommand.Modelo));
    }

    [Fact]
    public void Modelo_Acima_De_30_Caracteres_Falha()
    {
        var result = _validator.Validate(
            new AdicionarVeiculoCommand("Corolla", Marca.Toyota, new string('m', 31), null, null));

        Assert.Contains(result.Errors, e => e.PropertyName == nameof(AdicionarVeiculoCommand.Modelo));
    }

    [Fact]
    public void Opcionais_Nulos_Sao_Permitidos()
    {
        var result = _validator.Validate(
            new AdicionarVeiculoCommand("Corolla", Marca.Toyota, "Corolla", null, 1000m));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Valor_Nulo_E_Permitido()
    {
        var result = _validator.Validate(
            new AdicionarVeiculoCommand("Corolla", Marca.Toyota, "Corolla", "Ar", null));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Valor_Negativo_Falha()
    {
        var result = _validator.Validate(
            new AdicionarVeiculoCommand("Corolla", Marca.Toyota, "Corolla", null, -1m));

        Assert.Contains(result.Errors, e => e.PropertyName == nameof(AdicionarVeiculoCommand.Valor));
    }
}
