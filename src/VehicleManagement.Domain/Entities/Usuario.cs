namespace VehicleManagement.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string Login { get; private set; } = null!;
    public string Senha { get; private set; } = null!;

    private Usuario()
    {
    }

    public Usuario(string nome, string login, string senha)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Login = login;
        Senha = senha;
    }
}
