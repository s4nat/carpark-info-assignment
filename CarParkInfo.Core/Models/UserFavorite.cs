namespace CarParkInfo.Core.Models;

public class UserFavorite
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string CarParkNo { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public User? User { get; set; }
    public CarPark? CarPark { get; set; }
}