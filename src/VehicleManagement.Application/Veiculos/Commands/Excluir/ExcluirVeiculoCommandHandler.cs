using MediatR;
using VehicleManagement.Application.Services;

namespace VehicleManagement.Application.Veiculos.Commands.Excluir;

public class ExcluirVeiculoCommandHandler : IRequestHandler<ExcluirVeiculoCommand>
{
    private readonly IVeiculoService _veiculoService;

    public ExcluirVeiculoCommandHandler(IVeiculoService veiculoService)
    {
        _veiculoService = veiculoService;
    }

    public async Task Handle(ExcluirVeiculoCommand request, CancellationToken cancellationToken)
    {
        await _veiculoService.ExcluirAsync(request.Id, cancellationToken);
    }
}
