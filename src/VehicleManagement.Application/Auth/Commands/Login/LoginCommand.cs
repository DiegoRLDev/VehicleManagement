using MediatR;
using VehicleManagement.Application.DTOs;

namespace VehicleManagement.Application.Auth.Commands.Login;

public record LoginCommand(string Login, string Senha) : IRequest<TokenResponseDto>;
