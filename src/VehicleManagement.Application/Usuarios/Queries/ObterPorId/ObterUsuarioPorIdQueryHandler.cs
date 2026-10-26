using MediatR;
using VehicleManagement.Application.DTOs;
using VehicleManagement.Application.Services;

namespace VehicleManagement.Application.Usuarios.Queries.ObterPorId;

public class ObterUsuarioPorIdQueryHandler : IRequestHandler<ObterUsuarioPorIdQuery, UsuarioDto>
{
    private readonly IUsuarioService _usuarioService;

    public ObterUsuarioPorIdQueryHandler(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public async Task<UsuarioDto> Handle(ObterUsuarioPorIdQuery request, CancellationToken cancellationToken)
    {
        return await _usuarioService.ObterPorIdAsync(request.Id, cancellationToken);
    }
}
