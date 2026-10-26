using VehicleManagement.Domain.Entities;
using VehicleManagement.Domain.Enums;
using VehicleManagement.Infra.Repositories;

namespace VehicleManagement.Application.Tests.Infra;

public class VeiculoRepositoryTests
{
    [Fact]
    public async Task AdicionarAsync_Persiste_Veiculo()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var repository = new VeiculoRepository(context);
        var veiculo = new Veiculo("Corolla XEi", Marca.Toyota, "Corolla", "Ar", 145000m);

        await repository.AdicionarAsync(veiculo, CancellationToken.None);

        var persistido = await repository.ObterPorIdAsync(veiculo.Id, CancellationToken.None);

        Assert.NotNull(persistido);
        Assert.Equal(veiculo.Id, persistido.Id);
        Assert.Equal("Corolla XEi", persistido.Descricao);
        Assert.Equal(Marca.Toyota, persistido.Marca);
        Assert.Equal("Corolla", persistido.Modelo);
        Assert.Equal("Ar", persistido.Opcionais);
        Assert.Equal(145000m, persistido.Valor);
    }

    [Fact]
    public async Task ObterPorIdAsync_Retorna_Null_Quando_Nao_Existe()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var repository = new VeiculoRepository(context);

        var obtido = await repository.ObterPorIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(obtido);
    }

    [Fact]
    public async Task ListarAsync_Retorna_Todos_Veiculos()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var repository = new VeiculoRepository(context);
        await repository.AdicionarAsync(new Veiculo("Uno", Marca.Fiat, "Uno"), CancellationToken.None);
        await repository.AdicionarAsync(new Veiculo("Ka", Marca.Ford, "Ka"), CancellationToken.None);

        var lista = await repository.ListarAsync(CancellationToken.None);

        Assert.Equal(2, lista.Count);
    }

    [Fact]
    public async Task AtualizarAsync_Persiste_Alteracoes_Da_Entidade()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var repository = new VeiculoRepository(context);
        var veiculo = new Veiculo("Descricao antiga", Marca.Fiat, "Uno");
        await repository.AdicionarAsync(veiculo, CancellationToken.None);

        var paraAtualizar = await repository.ObterPorIdAsync(veiculo.Id, CancellationToken.None);
        Assert.NotNull(paraAtualizar);

        paraAtualizar.Atualizar("Corolla XEi", Marca.Toyota, "Corolla", "Bancos de couro", 150000m);
        await repository.AtualizarAsync(paraAtualizar, CancellationToken.None);

        var atualizado = await repository.ObterPorIdAsync(veiculo.Id, CancellationToken.None);

        Assert.NotNull(atualizado);
        Assert.Equal(veiculo.Id, atualizado.Id);
        Assert.Equal("Corolla XEi", atualizado.Descricao);
        Assert.Equal(Marca.Toyota, atualizado.Marca);
        Assert.Equal("Corolla", atualizado.Modelo);
        Assert.Equal("Bancos de couro", atualizado.Opcionais);
        Assert.Equal(150000m, atualizado.Valor);
    }

    [Fact]
    public async Task ExcluirAsync_Remove_Veiculo()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var repository = new VeiculoRepository(context);
        var veiculo = new Veiculo("Civic", Marca.Honda, "Civic");
        await repository.AdicionarAsync(veiculo, CancellationToken.None);

        var paraExcluir = await repository.ObterPorIdAsync(veiculo.Id, CancellationToken.None);
        Assert.NotNull(paraExcluir);

        await repository.ExcluirAsync(paraExcluir, CancellationToken.None);

        var excluido = await repository.ObterPorIdAsync(veiculo.Id, CancellationToken.None);
        Assert.Null(excluido);
    }
}
