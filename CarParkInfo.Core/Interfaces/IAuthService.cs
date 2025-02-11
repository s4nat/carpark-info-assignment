using CarParkInfo.Core.DTOs;

namespace CarParkInfo.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<bool> ValidateTokenAsync(string token);
    string GetUserIdFromToken(string token);
}