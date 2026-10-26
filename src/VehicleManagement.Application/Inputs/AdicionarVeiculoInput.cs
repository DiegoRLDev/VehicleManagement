using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Application.Inputs;

public record AdicionarVeiculoInput(
    string Descricao,
    Marca Marca,
    string Modelo,
    string? Opcionais,
    decimal? Valor);
