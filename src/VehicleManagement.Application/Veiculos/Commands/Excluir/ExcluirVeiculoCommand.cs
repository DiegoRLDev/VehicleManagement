using MediatR;

namespace VehicleManagement.Application.Veiculos.Commands.Excluir;

public record ExcluirVeiculoCommand(Guid Id) : IRequest;
