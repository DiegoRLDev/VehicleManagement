using MediatR;
using VehicleManagement.Application.DTOs;
using VehicleManagement.Application.Services;

namespace VehicleManagement.Application.Veiculos.Queries.ObterPorId;

public class ObterVeiculoPorIdQueryHandler : IRequestHandler<ObterVeiculoPorIdQuery, VeiculoDto>
{
    private readonly IVeiculoService _veiculoService;

    public ObterVeiculoPorIdQueryHandler(IVeiculoService veiculoService)
    {
        _veiculoService = veiculoService;
    }

    public async Task<VeiculoDto> Handle(ObterVeiculoPorIdQuery request, CancellationToken cancellationToken)
    {
        return await _veiculoService.ObterPorIdAsync(request.Id, cancellationToken);
    }
}
