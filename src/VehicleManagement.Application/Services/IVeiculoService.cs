using VehicleManagement.Application.DTOs;
using VehicleManagement.Application.Inputs;

namespace VehicleManagement.Application.Services;

public interface IVeiculoService
{
    Task<VeiculoDto> AdicionarAsync(AdicionarVeiculoInput input, CancellationToken cancellationToken);

    Task<VeiculoDto> AtualizarAsync(Guid id, AtualizarVeiculoInput input, CancellationToken cancellationToken);

    Task ExcluirAsync(Guid id, CancellationToken cancellationToken);

    Task<VeiculoDto> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<VeiculoDto>> ListarAsync(CancellationToken cancellationToken);
}
