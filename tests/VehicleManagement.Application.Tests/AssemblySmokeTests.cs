using VehicleManagement.Application;

namespace VehicleManagement.Application.Tests;

public class AssemblySmokeTests
{
    [Fact]
    public void Application_Assembly_Is_Loadable()
    {
        var assembly = typeof(DependencyInjection).Assembly;

        Assert.NotNull(assembly);
        Assert.Equal("VehicleManagement.Application", assembly.GetName().Name);
    }
}
