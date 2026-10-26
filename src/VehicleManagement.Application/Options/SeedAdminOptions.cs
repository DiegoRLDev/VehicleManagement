namespace VehicleManagement.Application.Options;

public class SeedAdminOptions
{
    public const string SectionName = "SeedAdmin";

    public string Login { get; set; } = "admin";
    public string Nome { get; set; } = "Administrador";
    public string Senha { get; set; } = string.Empty;
}
