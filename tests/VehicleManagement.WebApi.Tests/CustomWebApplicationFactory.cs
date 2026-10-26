using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VehicleManagement.Infra.Context;

namespace VehicleManagement.WebApi.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"VehicleManagementDb_Tests_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            // Garante SigningKey antes do Program.cs validar a seção Jwt.
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "VehicleManagement",
                ["Jwt:Audience"] = "VehicleManagement",
                ["Jwt:SigningKey"] = "DEV_ONLY_VehicleManagement_SigningKey_Min32Chars!",
                ["Jwt:ExpirationMinutes"] = "60"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<VehicleManagementDbContext>));
            services.RemoveAll(typeof(VehicleManagementDbContext));

            services.AddDbContext<VehicleManagementDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }
}
