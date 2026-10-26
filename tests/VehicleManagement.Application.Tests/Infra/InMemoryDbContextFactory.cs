using Microsoft.EntityFrameworkCore;
using VehicleManagement.Infra.Context;

namespace VehicleManagement.Application.Tests.Infra;

internal static class InMemoryDbContextFactory
{
    public static VehicleManagementDbContext Create()
    {
        var options = new DbContextOptionsBuilder<VehicleManagementDbContext>()
            .UseInMemoryDatabase(databaseName: $"VehicleManagementDb_{Guid.NewGuid()}")
            .Options;

        return new VehicleManagementDbContext(options);
    }
}
