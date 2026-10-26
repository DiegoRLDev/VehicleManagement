using VehicleManagement.Application.Exceptions;
using VehicleManagement.Application.Inputs;
using VehicleManagement.Application.Services;
using VehicleManagement.Application.Tests.Fakes;
using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Application.Tests.Services;

public class VeiculoServiceTests
{
    [Fact]
    public async Task AdicionarAsync_Cadastra_Veiculo()
    {
        var service = new VeiculoService(new FakeVeiculoRepository());

        var resultado = await service.AdicionarAsync(
            new AdicionarVeiculoInput("Corolla XEi", Marca.Toyota, "Corolla", "Ar", 145000m),
            CancellationToken.None);

        Assert.NotEqual(Guid.Empty, resultado.Id);
        Assert.Equal("Corolla XEi", resultado.Descricao);
        Assert.Equal(Marca.Toyota, resultado.Marca);
        Assert.Equal("Corolla", resultado.Modelo);
        Assert.Equal("Ar", resultado.Opcionais);
        Assert.Equal(145000m, resultado.Valor);
    }

    [Fact]
    public async Task AtualizarAsync_Altera_Veiculo_Existente()
    {
        var service = new VeiculoService(new FakeVeiculoRepository());
        var criado = await service.AdicionarAsync(
            new AdicionarVeiculoInput("Uno", Marca.Fiat, "Uno", null, null),
            CancellationToken.None);

        var atualizado = await service.AtualizarAsync(
            criado.Id,
            new AtualizarVeiculoInput("Corolla XEi", Marca.Toyota, "Corolla", "Couro", 150000m),
            CancellationToken.None);

        Assert.Equal(criado.Id, atualizado.Id);
        Assert.Equal("Corolla XEi", atualizado.Descricao);
        Assert.Equal(Marca.Toyota, atualizado.Marca);
        Assert.Equal("Corolla", atualizado.Modelo);
        Assert.Equal("Couro", atualizado.Opcionais);
        Assert.Equal(150000m, atualizado.Valor);
    }

    [Fact]
    public async Task ExcluirAsync_Remove_Veiculo()
    {
        var service = new VeiculoService(new FakeVeiculoRepository());
        var criado = await service.AdicionarAsync(
            new AdicionarVeiculoInput("Civic", Marca.Honda, "Civic", null, null),
            CancellationToken.None);

        await service.ExcluirAsync(criado.Id, CancellationToken.None);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.ObterPorIdAsync(criado.Id, CancellationToken.None));
    }

    [Fact]
    public async Task ObterPorIdAsync_Retorna_Veiculo_Existente()
    {
        var service = new VeiculoService(new FakeVeiculoRepository());
        var criado = await service.AdicionarAsync(
            new AdicionarVeiculoInput("Ka", Marca.Ford, "Ka", null, 40000m),
            CancellationToken.None);

        var obtido = await service.ObterPorIdAsync(criado.Id, CancellationToken.None);

        Assert.Equal(criado.Id, obtido.Id);
        Assert.Equal("Ka", obtido.Descricao);
    }

    [Fact]
    public async Task ListarAsync_Retorna_Todos_Veiculos()
    {
        var service = new VeiculoService(new FakeVeiculoRepository());
        await service.AdicionarAsync(new AdicionarVeiculoInput("Uno", Marca.Fiat, "Uno", null, null), CancellationToken.None);
        await service.AdicionarAsync(new AdicionarVeiculoInput("Ka", Marca.Ford, "Ka", null, null), CancellationToken.None);

        var lista = await service.ListarAsync(CancellationToken.None);

        Assert.Equal(2, lista.Count);
    }

    [Fact]
    public async Task Operacoes_Com_Veiculo_Inexistente_Lancam_NotFoundException()
    {
        var service = new VeiculoService(new FakeVeiculoRepository());
        var id = Guid.NewGuid();

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.ObterPorIdAsync(id, CancellationToken.None));

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.AtualizarAsync(
                id,
                new AtualizarVeiculoInput("X", Marca.Fiat, "Y", null, null),
                CancellationToken.None));

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.ExcluirAsync(id, CancellationToken.None));
    }
}
