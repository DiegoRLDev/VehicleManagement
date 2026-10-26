using MediatR;
using VehicleManagement.Application.DTOs;

namespace VehicleManagement.Application.Veiculos.Queries.Listar;

public record ListarVeiculosQuery : IRequest<IReadOnlyCollection<VeiculoDto>>;
