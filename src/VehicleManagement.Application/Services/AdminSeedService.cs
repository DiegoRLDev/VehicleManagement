using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VehicleManagement.Application.Options;
using VehicleManagement.Domain.Entities;
using VehicleManagement.Domain.Enums;
using VehicleManagement.Domain.Interfaces;

namespace VehicleManagement.Application.Services;

public class AdminSeedService : IAdminSeedService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly SeedAdminOptions _options;
    private readonly ILogger<AdminSeedService> _logger;

    public AdminSeedService(
        IUsuarioRepository usuarioRepository,
        IOptions<SeedAdminOptions> options,
        ILogger<AdminSeedService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _options = options.Value;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.Senha))
        {
            _logger.LogInformation(
                "SeedAdmin:Senha não configurada. Administrador inicial não será criado.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_options.Login))
        {
            _logger.LogInformation(
                "SeedAdmin:Login não configurado. Administrador inicial não será criado.");
            return;
        }

        if (await _usuarioRepository.ExisteLoginAsync(_options.Login, cancellationToken))
        {
            _logger.LogInformation(
                "Administrador inicial com login '{Login}' já existe. Seed ignorado.",
                _options.Login);
            return;
        }

        var nome = string.IsNullOrWhiteSpace(_options.Nome) ? "Administrador" : _options.Nome;
        var senhaHash = BCrypt.Net.BCrypt.HashPassword(_options.Senha);
        var admin = new Usuario(nome, _options.Login, senhaHash, UsuarioRole.Admin);

        await _usuarioRepository.AdicionarAsync(admin, cancellationToken);

        _logger.LogInformation(
            "Administrador inicial '{Login}' criado com sucesso.",
            _options.Login);
    }
}
