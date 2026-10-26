using VehicleManagement.Domain.Entities;
using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Application.Tests.Domain;

public class UsuarioTests
{
    [Fact]
    public void Construtor_Inicializa_Propriedades_Obrigatorias_E_Gera_Id()
    {
        var usuario = new Usuario("Ana Silva", "ana.silva", "hash-ou-senha-temporaria", UsuarioRole.User);

        Assert.NotEqual(Guid.Empty, usuario.Id);
        Assert.Equal("Ana Silva", usuario.Nome);
        Assert.Equal("ana.silva", usuario.Login);
        Assert.Equal("hash-ou-senha-temporaria", usuario.SenhaHash);
        Assert.Equal(UsuarioRole.User, usuario.Role);
    }

    [Fact]
    public void Construtor_Aceita_Role_Admin()
    {
        var usuario = new Usuario("Administrador", "admin", "hash", UsuarioRole.Admin);

        Assert.Equal(UsuarioRole.Admin, usuario.Role);
        Assert.Equal("admin", usuario.Login);
    }

    [Fact]
    public void Construtor_Gera_Ids_Distintos_Para_Instancias_Diferentes()
    {
        var primeiro = new Usuario("Ana", "ana", "s1", UsuarioRole.User);
        var segundo = new Usuario("Ana", "ana", "s1", UsuarioRole.User);

        Assert.NotEqual(primeiro.Id, segundo.Id);
    }
}
