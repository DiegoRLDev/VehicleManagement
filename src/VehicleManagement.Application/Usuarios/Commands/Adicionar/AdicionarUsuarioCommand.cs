using MediatR;
using VehicleManagement.Application.DTOs;

namespace VehicleManagement.Application.Usuarios.Commands.Adicionar;

public record AdicionarUsuarioCommand(string Nome, string Login, string Senha) : IRequest<UsuarioDto>;
