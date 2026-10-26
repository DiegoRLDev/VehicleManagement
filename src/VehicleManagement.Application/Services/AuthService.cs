using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VehicleManagement.Application.DTOs;
using VehicleManagement.Application.Exceptions;
using VehicleManagement.Application.Inputs;
using VehicleManagement.Application.Options;
using VehicleManagement.Domain.Entities;
using VehicleManagement.Domain.Interfaces;

namespace VehicleManagement.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly JwtOptions _jwtOptions;

    public AuthService(IUsuarioRepository usuarioRepository, IOptions<JwtOptions> jwtOptions)
    {
        _usuarioRepository = usuarioRepository;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<TokenResponseDto> LoginAsync(LoginInput input, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.ObterPorLoginAsync(input.Login, cancellationToken);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(input.Senha, usuario.SenhaHash))
        {
            throw new UnauthorizedException("Credenciais inválidas.");
        }

        return GenerateToken(usuario);
    }

    private TokenResponseDto GenerateToken(Usuario usuario)
    {
        if (string.IsNullOrWhiteSpace(_jwtOptions.SigningKey))
        {
            throw new InvalidOperationException(
                "Jwt:SigningKey não configurada. Defina via User Secrets, variáveis de ambiente ou appsettings.");
        }

        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Login),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Login),
            // Tipo curto "role" para alinhar com RoleClaimType do JwtBearer (.NET 8).
            new Claim("role", usuario.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenResponseDto(tokenValue, "Bearer", expiresAt);
    }
}
