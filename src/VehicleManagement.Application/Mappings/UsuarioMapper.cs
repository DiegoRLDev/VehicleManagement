using VehicleManagement.Application.DTOs;
using VehicleManagement.Domain.Entities;

namespace VehicleManagement.Application.Mappings;

public static class UsuarioMapper
{
    public static UsuarioDto ToDto(Usuario usuario) =>
        new(usuario.Id, usuario.Nome, usuario.Login);
}
