using VehicleManagement.Application.DTOs;
using VehicleManagement.Application.Exceptions;
using VehicleManagement.Application.Inputs;
using VehicleManagement.Application.Mappings;
using VehicleManagement.Domain.Entities;
using VehicleManagement.Domain.Interfaces;

namespace VehicleManagement.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioDto> AdicionarAsync(AdicionarUsuarioInput input, CancellationToken cancellationToken)
    {
        if (await _usuarioRepository.ExisteLoginAsync(input.Login, cancellationToken))
        {
            throw new ConflictException($"Login '{input.Login}' já está em uso.");
        }

        var senhaHash = BCrypt.Net.BCrypt.HashPassword(input.Senha);
        var usuario = new Usuario(input.Nome, input.Login, senhaHash);

        await _usuarioRepository.AdicionarAsync(usuario, cancellationToken);

        return UsuarioMapper.ToDto(usuario);
    }

    public async Task<UsuarioDto> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(id, cancellationToken);

        if (usuario is null)
        {
            throw new NotFoundException($"Usuário com Id '{id}' não foi encontrado.");
        }

        return UsuarioMapper.ToDto(usuario);
    }
}
