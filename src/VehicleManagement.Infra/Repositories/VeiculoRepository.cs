using Microsoft.EntityFrameworkCore;
using VehicleManagement.Domain.Entities;
using VehicleManagement.Domain.Interfaces;
using VehicleManagement.Infra.Context;

namespace VehicleManagement.Infra.Repositories;

public class VeiculoRepository : IVeiculoRepository
{
    private readonly VehicleManagementDbContext _context;

    public VeiculoRepository(VehicleManagementDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Veiculo veiculo, CancellationToken cancellationToken)
    {
        _context.Veiculos.Add(veiculo);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(Veiculo veiculo, CancellationToken cancellationToken)
    {
        var existente = await ObterRastreadoAsync(veiculo.Id, cancellationToken);
        if (existente is null)
        {
            _context.Veiculos.Update(veiculo);
        }
        else
        {
            _context.Entry(existente).CurrentValues.SetValues(veiculo);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ExcluirAsync(Veiculo veiculo, CancellationToken cancellationToken)
    {
        var existente = await ObterRastreadoAsync(veiculo.Id, cancellationToken);
        if (existente is null)
        {
            _context.Veiculos.Remove(veiculo);
        }
        else
        {
            _context.Veiculos.Remove(existente);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Veiculo?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Veiculos
            .AsNoTracking()
            .FirstOrDefaultAsync(veiculo => veiculo.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Veiculo>> ListarAsync(CancellationToken cancellationToken)
    {
        return await _context.Veiculos
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    private async Task<Veiculo?> ObterRastreadoAsync(Guid id, CancellationToken cancellationToken)
    {
        var local = _context.Veiculos.Local.FirstOrDefault(veiculo => veiculo.Id == id);
        if (local is not null)
        {
            return local;
        }

        return await _context.Veiculos
            .FirstOrDefaultAsync(veiculo => veiculo.Id == id, cancellationToken);
    }
}
