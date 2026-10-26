using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleManagement.Domain.Interfaces;
using VehicleManagement.Infra;
using VehicleManagement.Infra.Context;
using VehicleManagement.Infra.Repositories;

namespace VehicleManagement.Application.Tests.Infra;

public class DependencyInjectionTests
{
    [Fact]
    public void AddInfra_Registra_DbContext_E_Repositorios()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddInfra(configuration);

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        Assert.NotNull(scope.ServiceProvider.GetService<VehicleManagementDbContext>());
        Assert.IsType<UsuarioRepository>(scope.ServiceProvider.GetRequiredService<IUsuarioRepository>());
        Assert.IsType<VeiculoRepository>(scope.ServiceProvider.GetRequiredService<IVeiculoRepository>());
    }
}
