using VehicleManagement.Domain.Entities;
using VehicleManagement.Domain.Interfaces;

namespace VehicleManagement.Application.Tests.Fakes;

public class FakeUsuarioRepository : IUsuarioRepository
{
    private readonly List<Usuario> _usuarios = [];

    public Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken)
    {
        _usuarios.Add(usuario);
        return Task.CompletedTask;
    }

    public Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_usuarios.FirstOrDefault(usuario => usuario.Id == id));
    }

    public Task<Usuario?> ObterPorLoginAsync(string login, CancellationToken cancellationToken)
    {
        return Task.FromResult(_usuarios.FirstOrDefault(usuario => usuario.Login == login));
    }

    public Task<bool> ExisteLoginAsync(string login, CancellationToken cancellationToken)
    {
        return Task.FromResult(_usuarios.Any(usuario => usuario.Login == login));
    }

    public IReadOnlyCollection<Usuario> Items => _usuarios;
}
