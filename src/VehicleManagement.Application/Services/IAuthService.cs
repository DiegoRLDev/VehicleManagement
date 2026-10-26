using VehicleManagement.Application.DTOs;
using VehicleManagement.Application.Inputs;

namespace VehicleManagement.Application.Services;

public interface IAuthService
{
    Task<TokenResponseDto> LoginAsync(LoginInput input, CancellationToken cancellationToken);
}
