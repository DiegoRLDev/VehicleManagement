using MediatR;
using VehicleManagement.Application.DTOs;

namespace VehicleManagement.Application.Usuarios.Queries.ObterPorId;

public record ObterUsuarioPorIdQuery(Guid Id) : IRequest<UsuarioDto>;
