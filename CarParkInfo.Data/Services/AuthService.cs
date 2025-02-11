using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarParkInfo.Core.DTOs;
using CarParkInfo.Core.Interfaces;
using CarParkInfo.Core.Models;
using CarParkInfo.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CarParkInfo.Data.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Check if user already exists
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            throw new InvalidOperationException("Email already registered");
        }

        // Create new user
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Generate JWT token
        var token = GenerateJwtToken(user);

        return new AuthResponse
        {
            Token = token,
            Username = user.Username,
            Email = user.Email
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid credentials");
        }

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        // Generate JWT token
        var token = GenerateJwtToken(user);

        return new AuthResponse
        {
            Token = token,
            Username = user.Username,
            Email = user.Email
        };
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"] ?? 
                throw new InvalidOperationException("JWT secret not configured"));
            
            _logger.LogInformation($"Validating token with secret key length: {key.Length}");
            
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var validationResult = await Task.Run(() =>
            {
                try
                {
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                    _logger.LogInformation($"Token validated successfully for user: {principal.Identity?.Name}");
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Token validation failed: {ex.Message}");
                    return false;
                }
            });

            return validationResult;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Token validation outer exception: {ex.Message}");
            return false;
        }
    }

    public string GetUserIdFromToken(string token)
{
    try
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        _logger.LogInformation("All claims in token:");
        foreach (var claim in jwtToken.Claims)
        {
            _logger.LogInformation($"Type: {claim.Type}, Value: {claim.Value}");
        }

        // Try different claim types that might contain the user ID
        var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid") ??  // Try "nameid"
                         jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier) ?? // Try standard claim type
                         jwtToken.Claims.FirstOrDefault(x => x.Type == "sub"); // Try "sub"

        if (userIdClaim != null)
        {
            _logger.LogInformation($"Found userId claim with type {userIdClaim.Type}: {userIdClaim.Value}");
            return userIdClaim.Value;
        }

        throw new InvalidOperationException("Token does not contain user ID");
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error extracting user ID from token: {ex.Message}");
        throw;
    }
}

    private string GenerateJwtToken(User user)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"] ?? 
        throw new InvalidOperationException("JWT secret not configured"));
    
    var claims = new[]
    {
        new Claim("nameid", user.Id),                    // Add nameid claim
        new Claim(ClaimTypes.NameIdentifier, user.Id),   // Add standard NameIdentifier
        new Claim("sub", user.Id),                       // Add sub claim
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.Username)
    };

    _logger.LogInformation("Generating token with claims:");
    foreach (var claim in claims)
    {
        _logger.LogInformation($"Adding claim - Type: {claim.Type}, Value: {claim.Value}");
    }

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    var generatedToken = tokenHandler.WriteToken(token);

    // Verify the generated token
    var decodedToken = tokenHandler.ReadJwtToken(generatedToken);
    _logger.LogInformation("Verifying generated token claims:");
    foreach (var claim in decodedToken.Claims)
    {
        _logger.LogInformation($"Verified claim - Type: {claim.Type}, Value: {claim.Value}");
    }

    return generatedToken;
}
}