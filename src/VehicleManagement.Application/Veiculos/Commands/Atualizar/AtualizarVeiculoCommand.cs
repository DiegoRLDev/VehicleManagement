using MediatR;
using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Application.Veiculos.Commands.Atualizar;

public record AtualizarVeiculoCommand(
    Guid Id,
    string Descricao,
    Marca Marca,
    string Modelo,
    string? Opcionais,
    decimal? Valor) : IRequest;
