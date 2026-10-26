using VehicleManagement.Application.Auth.Commands.Login;

namespace VehicleManagement.Application.Tests.Validators;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator = new();

    [Fact]
    public void Login_Valido_Passa()
    {
        var result = _validator.Validate(new LoginCommand("ana.silva", "senha123"));
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Login_Obrigatorio()
    {
        var result = _validator.Validate(new LoginCommand("", "senha123"));
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(LoginCommand.Login));
    }

    [Fact]
    public void Senha_Obrigatoria()
    {
        var result = _validator.Validate(new LoginCommand("ana.silva", ""));
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(LoginCommand.Senha));
    }
}
