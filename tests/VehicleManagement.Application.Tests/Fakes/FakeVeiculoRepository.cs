using VehicleManagement.Domain.Entities;
using VehicleManagement.Domain.Interfaces;

namespace VehicleManagement.Application.Tests.Fakes;

public class FakeVeiculoRepository : IVeiculoRepository
{
    private readonly List<Veiculo> _veiculos = [];

    public Task AdicionarAsync(Veiculo veiculo, CancellationToken cancellationToken)
    {
        _veiculos.Add(veiculo);
        return Task.CompletedTask;
    }

    public Task AtualizarAsync(Veiculo veiculo, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task ExcluirAsync(Veiculo veiculo, CancellationToken cancellationToken)
    {
        _veiculos.RemoveAll(item => item.Id == veiculo.Id);
        return Task.CompletedTask;
    }

    public Task<Veiculo?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_veiculos.FirstOrDefault(veiculo => veiculo.Id == id));
    }

    public Task<IReadOnlyCollection<Veiculo>> ListarAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyCollection<Veiculo>>(_veiculos.ToList());
    }
}
