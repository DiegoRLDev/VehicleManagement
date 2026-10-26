using VehicleManagement.Domain.Enums;

namespace VehicleManagement.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string Login { get; private set; } = null!;
    public string SenhaHash { get; private set; } = null!;
    public UsuarioRole Role { get; private set; }

    private Usuario()
    {
    }

    public Usuario(string nome, string login, string senhaHash, UsuarioRole role)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Login = login;
        SenhaHash = senhaHash;
        Role = role;
    }
}
