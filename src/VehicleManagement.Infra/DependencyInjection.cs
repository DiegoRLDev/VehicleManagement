using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VehicleManagement.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        // Persistência (DbContext, repositórios) será registrada nas próximas fases.
        _ = configuration;
        return services;
    }
}
