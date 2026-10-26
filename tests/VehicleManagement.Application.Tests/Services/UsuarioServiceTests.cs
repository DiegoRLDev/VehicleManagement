using VehicleManagement.Application.Exceptions;
using VehicleManagement.Application.Inputs;
using VehicleManagement.Application.Services;
using VehicleManagement.Application.Tests.Fakes;
using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Application.Tests.Services;

public class UsuarioServiceTests
{
    [Fact]
    public async Task AdicionarAsync_Cadastra_Usuario_Com_Senha_Hasheada_E_Role_User()
    {
        var repository = new FakeUsuarioRepository();
        var service = new UsuarioService(repository);

        var resultado = await service.AdicionarAsync(
            new AdicionarUsuarioInput("Ana Silva", "ana.silva", "senha123"),
            CancellationToken.None);

        Assert.Equal("Ana Silva", resultado.Nome);
        Assert.Equal("ana.silva", resultado.Login);
        Assert.Equal(nameof(UsuarioRole.User), resultado.Role);
        Assert.DoesNotContain("Senha", resultado.GetType().GetProperties().Select(p => p.Name));

        var persistido = repository.Items.Single();
        Assert.Equal(UsuarioRole.User, persistido.Role);
        Assert.NotEqual("senha123", persistido.SenhaHash);
        Assert.True(BCrypt.Net.BCrypt.Verify("senha123", persistido.SenhaHash));
    }

    [Fact]
    public async Task AdicionarAsync_Nao_Permite_Elevacao_De_Privilegio()
    {
        var repository = new FakeUsuarioRepository();
        var service = new UsuarioService(repository);

        // O contrato público (AdicionarUsuarioInput) não possui Role;
        // o service força User independentemente de qualquer tentativa de elevação.
        var resultado = await service.AdicionarAsync(
            new AdicionarUsuarioInput("Ana Silva", "ana.silva", "senha123"),
            CancellationToken.None);

        Assert.Equal(nameof(UsuarioRole.User), resultado.Role);
        Assert.Null(typeof(AdicionarUsuarioInput).GetProperty("Role"));
        Assert.All(repository.Items, u => Assert.Equal(UsuarioRole.User, u.Role));
    }

    [Fact]
    public async Task AdicionarAsync_Com_Login_Duplicado_Lanca_ConflictException()
    {
        var repository = new FakeUsuarioRepository();
        var service = new UsuarioService(repository);
        await service.AdicionarAsync(
            new AdicionarUsuarioInput("Ana", "ana.silva", "senha123"),
            CancellationToken.None);

        await Assert.ThrowsAsync<ConflictException>(() =>
            service.AdicionarAsync(
                new AdicionarUsuarioInput("Outra", "ana.silva", "senha456"),
                CancellationToken.None));
    }

    [Fact]
    public async Task ObterPorIdAsync_Retorna_Usuario_Existente_Sem_Senha()
    {
        var repository = new FakeUsuarioRepository();
        var service = new UsuarioService(repository);
        var criado = await service.AdicionarAsync(
            new AdicionarUsuarioInput("Bruno", "bruno", "senha123"),
            CancellationToken.None);

        var obtido = await service.ObterPorIdAsync(criado.Id, CancellationToken.None);

        Assert.Equal(criado.Id, obtido.Id);
        Assert.Equal("Bruno", obtido.Nome);
        Assert.Equal("bruno", obtido.Login);
        Assert.Equal(nameof(UsuarioRole.User), obtido.Role);
        Assert.Null(obtido.GetType().GetProperty("Senha"));
        Assert.Null(obtido.GetType().GetProperty("SenhaHash"));
    }

    [Fact]
    public async Task ObterPorIdAsync_Com_Id_Inexistente_Lanca_NotFoundException()
    {
        var service = new UsuarioService(new FakeUsuarioRepository());

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.ObterPorIdAsync(Guid.NewGuid(), CancellationToken.None));
    }
}
