using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Domain.Entities;

public class Veiculo
{
    public Guid Id { get; private set; }
    public string Descricao { get; private set; } = null!;
    public Marca Marca { get; private set; }
    public string Modelo { get; private set; } = null!;
    public string? Opcionais { get; private set; }
    public decimal? Valor { get; private set; }

    private Veiculo()
    {
    }

    public Veiculo(
        string descricao,
        Marca marca,
        string modelo,
        string? opcionais = null,
        decimal? valor = null)
    {
        Id = Guid.NewGuid();
        Descricao = descricao;
        Marca = marca;
        Modelo = modelo;
        Opcionais = opcionais;
        Valor = valor;
    }

    public void Atualizar(
        string descricao,
        Marca marca,
        string modelo,
        string? opcionais,
        decimal? valor)
    {
        Descricao = descricao;
        Marca = marca;
        Modelo = modelo;
        Opcionais = opcionais;
        Valor = valor;
    }
}
