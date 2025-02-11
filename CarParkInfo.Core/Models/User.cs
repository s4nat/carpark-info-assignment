using System.ComponentModel.DataAnnotations;

namespace CarParkInfo.Core.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation property for user's favorite carparks
    public ICollection<UserFavorite> Favorites { get; set; } = new List<UserFavorite>();
}