using CarParkInfo.Core.Models;

namespace CarParkInfo.Core.Interfaces;

public interface ICarParkService
{
    Task<IEnumerable<CarPark>> GetFreeParkingCarParksAsync();
    Task<IEnumerable<CarPark>> GetNightParkingCarParksAsync();
    Task<IEnumerable<CarPark>> GetCarParksByHeightAsync(float minHeight);
    Task<bool> AddToFavoritesAsync(string carParkNo, string userId);  // Changed parameter name
    Task<IEnumerable<CarPark>> GetUserFavoritesAsync(string userId);
    Task<bool> RemoveFromFavoritesAsync(string carParkNo, string userId);  // Changed parameter name
}