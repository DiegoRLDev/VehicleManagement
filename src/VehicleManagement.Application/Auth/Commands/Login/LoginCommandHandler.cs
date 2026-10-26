using MediatR;
using VehicleManagement.Application.DTOs;
using VehicleManagement.Application.Inputs;
using VehicleManagement.Application.Services;

namespace VehicleManagement.Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponseDto>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<TokenResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var input = new LoginInput(request.Login, request.Senha);
        return await _authService.LoginAsync(input, cancellationToken);
    }
}
