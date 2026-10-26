using VehicleManagement.Application.DTOs;
using VehicleManagement.Application.Inputs;

namespace VehicleManagement.Application.Services;

public interface IUsuarioService
{
    Task<UsuarioDto> AdicionarAsync(AdicionarUsuarioInput input, CancellationToken cancellationToken);

    Task<UsuarioDto> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
}
