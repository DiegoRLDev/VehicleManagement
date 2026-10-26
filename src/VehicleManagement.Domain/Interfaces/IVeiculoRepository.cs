using VehicleManagement.Domain.Entities;

namespace VehicleManagement.Domain.Interfaces;

public interface IVeiculoRepository
{
    Task AdicionarAsync(Veiculo veiculo, CancellationToken cancellationToken);

    Task AtualizarAsync(Veiculo veiculo, CancellationToken cancellationToken);

    Task ExcluirAsync(Veiculo veiculo, CancellationToken cancellationToken);

    Task<Veiculo?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Veiculo>> ListarAsync(CancellationToken cancellationToken);
}
