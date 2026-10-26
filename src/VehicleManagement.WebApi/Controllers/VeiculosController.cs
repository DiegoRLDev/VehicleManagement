using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleManagement.Application.DTOs;
using VehicleManagement.Application.Veiculos.Commands.Adicionar;
using VehicleManagement.Application.Veiculos.Commands.Atualizar;
using VehicleManagement.Application.Veiculos.Commands.Excluir;
using VehicleManagement.Application.Veiculos.Queries.Listar;
using VehicleManagement.Application.Veiculos.Queries.ObterPorId;
using VehicleManagement.WebApi.Requests;

namespace VehicleManagement.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly IMediator _mediator;

    public VeiculosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(VeiculoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VeiculoDto>> Adicionar(
        [FromBody] AdicionarVeiculoRequest request,
        CancellationToken cancellationToken)
    {
        var veiculo = await _mediator.Send(
            new AdicionarVeiculoCommand(
                request.Descricao,
                request.Marca,
                request.Modelo,
                request.Opcionais,
                request.Valor),
            cancellationToken);

        return CreatedAtAction(nameof(ObterPorId), new { id = veiculo.Id }, veiculo);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<VeiculoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyCollection<VeiculoDto>>> Listar(CancellationToken cancellationToken)
    {
        var veiculos = await _mediator.Send(new ListarVeiculosQuery(), cancellationToken);
        return Ok(veiculos);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VeiculoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VeiculoDto>> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var veiculo = await _mediator.Send(new ObterVeiculoPorIdQuery(id), cancellationToken);
        return Ok(veiculo);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(
        Guid id,
        [FromBody] AtualizarVeiculoRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new AtualizarVeiculoCommand(
                id,
                request.Descricao,
                request.Marca,
                request.Modelo,
                request.Opcionais,
                request.Valor),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ExcluirVeiculoCommand(id), cancellationToken);
        return NoContent();
    }
}
