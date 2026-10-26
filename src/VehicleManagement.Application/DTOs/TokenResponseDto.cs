namespace VehicleManagement.Application.DTOs;

public record TokenResponseDto(string Token, string TokenType, DateTime ExpiresAt);
