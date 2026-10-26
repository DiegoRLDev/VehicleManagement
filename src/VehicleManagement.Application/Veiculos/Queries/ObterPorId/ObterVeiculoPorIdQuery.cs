using MediatR;
using VehicleManagement.Application.DTOs;

namespace VehicleManagement.Application.Veiculos.Queries.ObterPorId;

public record ObterVeiculoPorIdQuery(Guid Id) : IRequest<VeiculoDto>;
