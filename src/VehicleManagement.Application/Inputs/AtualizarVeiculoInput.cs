using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Application.Inputs;

public record AtualizarVeiculoInput(
    string Descricao,
    Marca Marca,
    string Modelo,
    string? Opcionais,
    decimal? Valor);
