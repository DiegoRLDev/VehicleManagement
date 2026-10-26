using VehicleManagement.Domain.Enums;

namespace VehicleManagement.WebApi.Requests;

public record AtualizarVeiculoRequest(
    string Descricao,
    Marca Marca,
    string Modelo,
    string? Opcionais,
    decimal? Valor);
