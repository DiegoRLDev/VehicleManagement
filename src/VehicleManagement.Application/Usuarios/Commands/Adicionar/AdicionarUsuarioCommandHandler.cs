using MediatR;
using VehicleManagement.Application.DTOs;
using VehicleManagement.Application.Inputs;
using VehicleManagement.Application.Services;

namespace VehicleManagement.Application.Usuarios.Commands.Adicionar;

public class AdicionarUsuarioCommandHandler : IRequestHandler<AdicionarUsuarioCommand, UsuarioDto>
{
    private readonly IUsuarioService _usuarioService;

    public AdicionarUsuarioCommandHandler(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public async Task<UsuarioDto> Handle(AdicionarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var input = new AdicionarUsuarioInput(request.Nome, request.Login, request.Senha);
        return await _usuarioService.AdicionarAsync(input, cancellationToken);
    }
}
