using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleManagement.Application.DTOs;
using VehicleManagement.Application.Usuarios.Commands.Adicionar;
using VehicleManagement.Application.Usuarios.Queries.ObterPorId;
using VehicleManagement.WebApi.Requests;

namespace VehicleManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuariosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UsuarioDto>> Adicionar(
        [FromBody] AdicionarUsuarioRequest request,
        CancellationToken cancellationToken)
    {
        var usuario = await _mediator.Send(
            new AdicionarUsuarioCommand(request.Nome, request.Login, request.Senha),
            cancellationToken);

        return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, usuario);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDto>> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var usuario = await _mediator.Send(new ObterUsuarioPorIdQuery(id), cancellationToken);
        return Ok(usuario);
    }
}
