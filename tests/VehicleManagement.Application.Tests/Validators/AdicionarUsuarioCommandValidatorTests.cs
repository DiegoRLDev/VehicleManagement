using VehicleManagement.Application.Usuarios.Commands.Adicionar;

namespace VehicleManagement.Application.Tests.Validators;

public class AdicionarUsuarioCommandValidatorTests
{
    private readonly AdicionarUsuarioCommandValidator _validator = new();

    [Fact]
    public void Cadastro_Valido_Passa()
    {
        var result = _validator.Validate(new AdicionarUsuarioCommand("Ana Silva", "ana.silva", "senha123"));
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Ab")]
    public void Nome_Invalido_Falha(string nome)
    {
        var result = _validator.Validate(new AdicionarUsuarioCommand(nome, "ana.silva", "senha123"));
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(AdicionarUsuarioCommand.Nome));
    }

    [Theory]
    [InlineData("")]
    [InlineData("ab")]
    public void Login_Invalido_Falha(string login)
    {
        var result = _validator.Validate(new AdicionarUsuarioCommand("Ana Silva", login, "senha123"));
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(AdicionarUsuarioCommand.Login));
    }

    [Theory]
    [InlineData("")]
    [InlineData("12345")]
    public void Senha_Invalida_Falha(string senha)
    {
        var result = _validator.Validate(new AdicionarUsuarioCommand("Ana Silva", "ana.silva", senha));
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(AdicionarUsuarioCommand.Senha));
    }
}
