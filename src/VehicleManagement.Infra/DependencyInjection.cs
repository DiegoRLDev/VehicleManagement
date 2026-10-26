using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleManagement.Domain.Interfaces;
using VehicleManagement.Infra.Context;
using VehicleManagement.Infra.Repositories;

namespace VehicleManagement.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        _ = configuration;

        services.AddDbContext<VehicleManagementDbContext>(options =>
            options.UseInMemoryDatabase("VehicleManagementDb"));

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IVeiculoRepository, VeiculoRepository>();

        return services;
    }
}
