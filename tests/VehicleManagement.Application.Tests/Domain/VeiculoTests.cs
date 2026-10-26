using VehicleManagement.Domain.Entities;
using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Application.Tests.Domain;

public class VeiculoTests
{
    [Fact]
    public void Construtor_Inicializa_Campos_Obrigatorios_E_Gera_Id()
    {
        var veiculo = new Veiculo("Corolla XEi", Marca.Toyota, "Corolla");

        Assert.NotEqual(Guid.Empty, veiculo.Id);
        Assert.Equal("Corolla XEi", veiculo.Descricao);
        Assert.Equal(Marca.Toyota, veiculo.Marca);
        Assert.Equal("Corolla", veiculo.Modelo);
        Assert.Null(veiculo.Opcionais);
        Assert.Null(veiculo.Valor);
    }

    [Fact]
    public void Construtor_Aceita_Campos_Opcionais()
    {
        var veiculo = new Veiculo(
            "Corolla XEi",
            Marca.Toyota,
            "Corolla",
            "Ar-condicionado, camera de re",
            145000m);

        Assert.Equal("Ar-condicionado, camera de re", veiculo.Opcionais);
        Assert.Equal(145000m, veiculo.Valor);
    }

    [Fact]
    public void Atualizar_Altera_Propriedades_Sem_Mudar_Id()
    {
        var veiculo = new Veiculo("Descricao antiga", Marca.Fiat, "Uno", null, null);
        var idOriginal = veiculo.Id;

        veiculo.Atualizar(
            "Corolla XEi",
            Marca.Toyota,
            "Corolla",
            "Bancos de couro",
            150000m);

        Assert.Equal(idOriginal, veiculo.Id);
        Assert.Equal("Corolla XEi", veiculo.Descricao);
        Assert.Equal(Marca.Toyota, veiculo.Marca);
        Assert.Equal("Corolla", veiculo.Modelo);
        Assert.Equal("Bancos de couro", veiculo.Opcionais);
        Assert.Equal(150000m, veiculo.Valor);
    }
}
