using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Application.Tests.Domain;

public class MarcaTests
{
    [Theory]
    [InlineData(Marca.Chevrolet, 1)]
    [InlineData(Marca.Fiat, 2)]
    [InlineData(Marca.Ford, 3)]
    [InlineData(Marca.Honda, 4)]
    [InlineData(Marca.Hyundai, 5)]
    [InlineData(Marca.Toyota, 6)]
    [InlineData(Marca.Volkswagen, 7)]
    public void Valores_Numericos_Do_Enumerador_Sao_Estaveis(Marca marca, int valorEsperado)
    {
        Assert.Equal(valorEsperado, (int)marca);
    }

    [Fact]
    public void Enumerador_Contem_Sete_Marcas()
    {
        var valores = Enum.GetValues<Marca>();

        Assert.Equal(7, valores.Length);
    }
}
