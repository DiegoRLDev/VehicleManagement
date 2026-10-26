using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleManagement.Application.Auth.Commands.Login;
using VehicleManagement.Application.DTOs;
using VehicleManagement.WebApi.Requests;

namespace VehicleManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponseDto>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var token = await _mediator.Send(
            new LoginCommand(request.Login, request.Senha),
            cancellationToken);

        return Ok(token);
    }
}
