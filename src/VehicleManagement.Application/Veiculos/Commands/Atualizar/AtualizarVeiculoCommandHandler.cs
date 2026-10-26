using MediatR;
using VehicleManagement.Application.Inputs;
using VehicleManagement.Application.Services;

namespace VehicleManagement.Application.Veiculos.Commands.Atualizar;

public class AtualizarVeiculoCommandHandler : IRequestHandler<AtualizarVeiculoCommand>
{
    private readonly IVeiculoService _veiculoService;

    public AtualizarVeiculoCommandHandler(IVeiculoService veiculoService)
    {
        _veiculoService = veiculoService;
    }

    public async Task Handle(AtualizarVeiculoCommand request, CancellationToken cancellationToken)
    {
        var input = new AtualizarVeiculoInput(
            request.Descricao,
            request.Marca,
            request.Modelo,
            request.Opcionais,
            request.Valor);

        await _veiculoService.AtualizarAsync(request.Id, input, cancellationToken);
    }
}
