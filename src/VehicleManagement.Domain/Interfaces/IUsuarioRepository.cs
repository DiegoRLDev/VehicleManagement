using VehicleManagement.Domain.Entities;

namespace VehicleManagement.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken);

    Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Usuario?> ObterPorLoginAsync(string login, CancellationToken cancellationToken);

    Task<bool> ExisteLoginAsync(string login, CancellationToken cancellationToken);
}
