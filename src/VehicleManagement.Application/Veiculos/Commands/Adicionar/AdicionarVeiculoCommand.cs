using MediatR;
using VehicleManagement.Application.DTOs;
using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Application.Veiculos.Commands.Adicionar;

public record AdicionarVeiculoCommand(
    string Descricao,
    Marca Marca,
    string Modelo,
    string? Opcionais,
    decimal? Valor) : IRequest<VeiculoDto>;
