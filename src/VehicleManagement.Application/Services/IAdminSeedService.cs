namespace VehicleManagement.Application.Services;

public interface IAdminSeedService
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
