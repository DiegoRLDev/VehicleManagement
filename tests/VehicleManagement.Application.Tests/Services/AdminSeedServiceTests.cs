using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using VehicleManagement.Application.Options;
using VehicleManagement.Application.Services;
using VehicleManagement.Application.Tests.Fakes;
using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Application.Tests.Services;

public class AdminSeedServiceTests
{
    private static AdminSeedService CreateService(
        FakeUsuarioRepository repository,
        SeedAdminOptions? options = null)
    {
        options ??= new SeedAdminOptions
        {
            Login = "admin",
            Nome = "Administrador",
            Senha = "SenhaAdminSegura123!"
        };

        return new AdminSeedService(
            repository,
            new OptionsWrapper<SeedAdminOptions>(options),
            NullLogger<AdminSeedService>.Instance);
    }

    [Fact]
    public async Task SeedAsync_Cria_Administrador_Com_Role_Admin_E_Senha_Hasheada()
    {
        var repository = new FakeUsuarioRepository();
        var service = CreateService(repository);

        await service.SeedAsync(CancellationToken.None);

        var admin = repository.Items.Single();
        Assert.Equal("admin", admin.Login);
        Assert.Equal("Administrador", admin.Nome);
        Assert.Equal(UsuarioRole.Admin, admin.Role);
        Assert.NotEqual("SenhaAdminSegura123!", admin.SenhaHash);
        Assert.True(BCrypt.Net.BCrypt.Verify("SenhaAdminSegura123!", admin.SenhaHash));
    }

    [Fact]
    public async Task SeedAsync_Nao_Duplica_Administrador_Existente()
    {
        var repository = new FakeUsuarioRepository();
        var service = CreateService(repository);

        await service.SeedAsync(CancellationToken.None);
        await service.SeedAsync(CancellationToken.None);

        Assert.Single(repository.Items);
        Assert.Equal(UsuarioRole.Admin, repository.Items.Single().Role);
    }

    [Fact]
    public async Task SeedAsync_Sem_Senha_Nao_Cria_Administrador()
    {
        var repository = new FakeUsuarioRepository();
        var service = CreateService(repository, new SeedAdminOptions
        {
            Login = "admin",
            Nome = "Administrador",
            Senha = ""
        });

        await service.SeedAsync(CancellationToken.None);

        Assert.Empty(repository.Items);
    }
}
