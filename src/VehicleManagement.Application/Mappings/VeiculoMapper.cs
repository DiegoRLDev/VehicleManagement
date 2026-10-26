using VehicleManagement.Application.DTOs;
using VehicleManagement.Domain.Entities;

namespace VehicleManagement.Application.Mappings;

public static class VeiculoMapper
{
    public static VeiculoDto ToDto(Veiculo veiculo) =>
        new(
            veiculo.Id,
            veiculo.Descricao,
            veiculo.Marca,
            veiculo.Modelo,
            veiculo.Opcionais,
            veiculo.Valor);
}
