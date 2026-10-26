using MediatR;
using VehicleManagement.Application.DTOs;
using VehicleManagement.Application.Services;

namespace VehicleManagement.Application.Veiculos.Queries.Listar;

public class ListarVeiculosQueryHandler : IRequestHandler<ListarVeiculosQuery, IReadOnlyCollection<VeiculoDto>>
{
    private readonly IVeiculoService _veiculoService;

    public ListarVeiculosQueryHandler(IVeiculoService veiculoService)
    {
        _veiculoService = veiculoService;
    }

    public async Task<IReadOnlyCollection<VeiculoDto>> Handle(
        ListarVeiculosQuery request,
        CancellationToken cancellationToken)
    {
        return await _veiculoService.ListarAsync(cancellationToken);
    }
}
