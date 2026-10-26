using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using VehicleManagement.Application.Exceptions;
using VehicleManagement.Application.Inputs;
using VehicleManagement.Application.Services;
using VehicleManagement.Application.Tests.Fakes;
using VehicleManagement.Domain.Entities;
using VehicleManagement.Domain.Enums;
using JwtOptions = VehicleManagement.Application.Options.JwtOptions;

namespace VehicleManagement.Application.Tests.Services;

public class AuthServiceTests
{
    private static AuthService CreateService(FakeUsuarioRepository repository, int expirationMinutes = 60)
    {
        IOptions<JwtOptions> options = new OptionsWrapper<JwtOptions>(new JwtOptions
        {
            Issuer = "VehicleManagement.Tests",
            Audience = "VehicleManagement.Tests",
            SigningKey = "TEST_ONLY_VehicleManagement_SigningKey_Min32Chars!",
            ExpirationMinutes = expirationMinutes
        });

        return new AuthService(repository, options);
    }

    [Fact]
    public async Task LoginAsync_Com_Credenciais_Validas_Retorna_Jwt_Com_Role_User()
    {
        var repository = new FakeUsuarioRepository();
        var usuarioService = new UsuarioService(repository);
        await usuarioService.AdicionarAsync(
            new AdicionarUsuarioInput("Ana", "ana.silva", "senha123"),
            CancellationToken.None);

        var authService = CreateService(repository);
        var resultado = await authService.LoginAsync(
            new LoginInput("ana.silva", "senha123"),
            CancellationToken.None);

        Assert.Equal("Bearer", resultado.TokenType);
        Assert.False(string.IsNullOrWhiteSpace(resultado.Token));
        Assert.True(resultado.ExpiresAt > DateTime.UtcNow);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(resultado.Token);

        Assert.Equal("ana.silva", token.Claims.First(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value);
        Assert.Contains(token.Claims, c => c.Type == JwtRegisteredClaimNames.Sub);
        Assert.True(token.ValidTo > DateTime.UtcNow);
        Assert.Contains(token.Claims, c => IsRoleClaim(c, "User"));
    }

    [Fact]
    public async Task LoginAsync_Com_Administrador_Retorna_Jwt_Com_Role_Admin()
    {
        var repository = new FakeUsuarioRepository();
        var admin = new Usuario("Administrador", "admin", BCrypt.Net.BCrypt.HashPassword("senhaAdmin"), UsuarioRole.Admin);
        await repository.AdicionarAsync(admin, CancellationToken.None);

        var authService = CreateService(repository);
        var resultado = await authService.LoginAsync(
            new LoginInput("admin", "senhaAdmin"),
            CancellationToken.None);

        var token = new JwtSecurityTokenHandler().ReadJwtToken(resultado.Token);
        Assert.Contains(token.Claims, c => IsRoleClaim(c, "Admin"));
    }

    [Fact]
    public async Task LoginAsync_Com_Senha_Incorreta_Lanca_UnauthorizedException()
    {
        var repository = new FakeUsuarioRepository();
        var usuarioService = new UsuarioService(repository);
        await usuarioService.AdicionarAsync(
            new AdicionarUsuarioInput("Ana", "ana.silva", "senha123"),
            CancellationToken.None);

        var authService = CreateService(repository);

        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            authService.LoginAsync(new LoginInput("ana.silva", "errada"), CancellationToken.None));
    }

    [Fact]
    public async Task LoginAsync_Com_Usuario_Inexistente_Lanca_UnauthorizedException()
    {
        var authService = CreateService(new FakeUsuarioRepository());

        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            authService.LoginAsync(new LoginInput("nao.existe", "senha123"), CancellationToken.None));
    }

    [Fact]
    public async Task LoginAsync_Respeita_Expiracao_Configurada()
    {
        var repository = new FakeUsuarioRepository();
        await new UsuarioService(repository).AdicionarAsync(
            new AdicionarUsuarioInput("Ana", "ana.silva", "senha123"),
            CancellationToken.None);

        var authService = CreateService(repository, expirationMinutes: 5);
        var before = DateTime.UtcNow;

        var resultado = await authService.LoginAsync(
            new LoginInput("ana.silva", "senha123"),
            CancellationToken.None);

        var token = new JwtSecurityTokenHandler().ReadJwtToken(resultado.Token);
        Assert.InRange(token.ValidTo, before.AddMinutes(4), before.AddMinutes(6));
        Assert.True(resultado.ExpiresAt > before.AddMinutes(4));
        Assert.True(resultado.ExpiresAt < before.AddMinutes(6));
    }

    private static bool IsRoleClaim(Claim claim, string role) =>
        claim.Value == role &&
        (claim.Type == ClaimTypes.Role || claim.Type == "role" || claim.Type.EndsWith("/role", StringComparison.Ordinal));
}
