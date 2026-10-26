using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Application.DTOs;

public record VeiculoDto(
    Guid Id,
    string Descricao,
    Marca Marca,
    string Modelo,
    string? Opcionais,
    decimal? Valor);
