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
    public const string SeedAdminLogin = "admin";
    public const string SeedAdminSenha = "AdminTestSenha123!";

    private readonly string _databaseName = $"VehicleManagementDb_Tests_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            // Garante SigningKey e senha de seed antes do Program.cs validar/configurar.
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "VehicleManagement",
                ["Jwt:Audience"] = "VehicleManagement",
                ["Jwt:SigningKey"] = "DEV_ONLY_VehicleManagement_SigningKey_Min32Chars!",
                ["Jwt:ExpirationMinutes"] = "60",
                ["SeedAdmin:Login"] = SeedAdminLogin,
                ["SeedAdmin:Nome"] = "Administrador",
                ["SeedAdmin:Senha"] = SeedAdminSenha
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
