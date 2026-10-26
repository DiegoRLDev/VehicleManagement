using VehicleManagement.Domain.Entities;
using VehicleManagement.Domain.Enums;
using VehicleManagement.Infra.Repositories;

namespace VehicleManagement.Application.Tests.Infra;

public class UsuarioRepositoryTests
{
    [Fact]
    public async Task AdicionarAsync_Persiste_Usuario_Com_Role()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var repository = new UsuarioRepository(context);
        var usuario = new Usuario("Ana Silva", "ana.silva", "senha-hash", UsuarioRole.User);

        await repository.AdicionarAsync(usuario, CancellationToken.None);

        var persistido = await repository.ObterPorIdAsync(usuario.Id, CancellationToken.None);

        Assert.NotNull(persistido);
        Assert.Equal(usuario.Id, persistido.Id);
        Assert.Equal("Ana Silva", persistido.Nome);
        Assert.Equal("ana.silva", persistido.Login);
        Assert.Equal("senha-hash", persistido.SenhaHash);
        Assert.Equal(UsuarioRole.User, persistido.Role);
    }

    [Fact]
    public async Task AdicionarAsync_Persiste_Role_Admin()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var repository = new UsuarioRepository(context);
        var usuario = new Usuario("Administrador", "admin", "hash", UsuarioRole.Admin);

        await repository.AdicionarAsync(usuario, CancellationToken.None);

        var persistido = await repository.ObterPorIdAsync(usuario.Id, CancellationToken.None);

        Assert.NotNull(persistido);
        Assert.Equal(UsuarioRole.Admin, persistido.Role);
    }

    [Fact]
    public async Task ObterPorIdAsync_Retorna_Usuario_Quando_Existe()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var repository = new UsuarioRepository(context);
        var usuario = new Usuario("Bruno", "bruno", "hash", UsuarioRole.User);
        await repository.AdicionarAsync(usuario, CancellationToken.None);

        var obtido = await repository.ObterPorIdAsync(usuario.Id, CancellationToken.None);

        Assert.NotNull(obtido);
        Assert.Equal(usuario.Login, obtido.Login);
    }

    [Fact]
    public async Task ObterPorIdAsync_Retorna_Null_Quando_Nao_Existe()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var repository = new UsuarioRepository(context);

        var obtido = await repository.ObterPorIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(obtido);
    }

    [Fact]
    public async Task ObterPorLoginAsync_Retorna_Usuario_Quando_Existe()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var repository = new UsuarioRepository(context);
        var usuario = new Usuario("Carla", "carla.login", "hash", UsuarioRole.User);
        await repository.AdicionarAsync(usuario, CancellationToken.None);

        var obtido = await repository.ObterPorLoginAsync("carla.login", CancellationToken.None);

        Assert.NotNull(obtido);
        Assert.Equal(usuario.Id, obtido.Id);
        Assert.Equal("Carla", obtido.Nome);
    }

    [Fact]
    public async Task ObterPorLoginAsync_Retorna_Null_Quando_Nao_Existe()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var repository = new UsuarioRepository(context);

        var obtido = await repository.ObterPorLoginAsync("inexistente", CancellationToken.None);

        Assert.Null(obtido);
    }

    [Fact]
    public async Task ExisteLoginAsync_Retorna_True_Quando_Login_Existe()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var repository = new UsuarioRepository(context);
        await repository.AdicionarAsync(new Usuario("Diego", "diego", "hash", UsuarioRole.User), CancellationToken.None);

        var existe = await repository.ExisteLoginAsync("diego", CancellationToken.None);

        Assert.True(existe);
    }

    [Fact]
    public async Task ExisteLoginAsync_Retorna_False_Quando_Login_Nao_Existe()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var repository = new UsuarioRepository(context);

        var existe = await repository.ExisteLoginAsync("sem-cadastro", CancellationToken.None);

        Assert.False(existe);
    }
}
