using Microsoft.EntityFrameworkCore;
using VehicleManagement.Domain.Entities;
using VehicleManagement.Domain.Interfaces;
using VehicleManagement.Infra.Context;

namespace VehicleManagement.Infra.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly VehicleManagementDbContext _context;

    public UsuarioRepository(VehicleManagementDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);
    }

    public async Task<Usuario?> ObterPorLoginAsync(string login, CancellationToken cancellationToken)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(usuario => usuario.Login == login, cancellationToken);
    }

    public async Task<bool> ExisteLoginAsync(string login, CancellationToken cancellationToken)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .AnyAsync(usuario => usuario.Login == login, cancellationToken);
    }
}
