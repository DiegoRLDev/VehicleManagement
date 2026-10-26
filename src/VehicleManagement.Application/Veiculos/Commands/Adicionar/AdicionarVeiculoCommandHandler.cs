using MediatR;
using VehicleManagement.Application.DTOs;
using VehicleManagement.Application.Inputs;
using VehicleManagement.Application.Services;

namespace VehicleManagement.Application.Veiculos.Commands.Adicionar;

public class AdicionarVeiculoCommandHandler : IRequestHandler<AdicionarVeiculoCommand, VeiculoDto>
{
    private readonly IVeiculoService _veiculoService;

    public AdicionarVeiculoCommandHandler(IVeiculoService veiculoService)
    {
        _veiculoService = veiculoService;
    }

    public async Task<VeiculoDto> Handle(AdicionarVeiculoCommand request, CancellationToken cancellationToken)
    {
        var input = new AdicionarVeiculoInput(
            request.Descricao,
            request.Marca,
            request.Modelo,
            request.Opcionais,
            request.Valor);

        return await _veiculoService.AdicionarAsync(input, cancellationToken);
    }
}
