using VehicleManagement.Domain.Entities;

namespace VehicleManagement.Application.Tests.Domain;

public class UsuarioTests
{
    [Fact]
    public void Construtor_Inicializa_Propriedades_Obrigatorias_E_Gera_Id()
    {
        var usuario = new Usuario("Ana Silva", "ana.silva", "hash-ou-senha-temporaria");

        Assert.NotEqual(Guid.Empty, usuario.Id);
        Assert.Equal("Ana Silva", usuario.Nome);
        Assert.Equal("ana.silva", usuario.Login);
        Assert.Equal("hash-ou-senha-temporaria", usuario.Senha);
    }

    [Fact]
    public void Construtor_Gera_Ids_Distintos_Para_Instancias_Diferentes()
    {
        var primeiro = new Usuario("Ana", "ana", "s1");
        var segundo = new Usuario("Ana", "ana", "s1");

        Assert.NotEqual(primeiro.Id, segundo.Id);
    }
}
