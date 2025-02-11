using CarParkInfo.Core.Interfaces;
using CarParkInfo.Core.Models;
using CarParkInfo.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CarParkInfo.Data.Services;

public class CarParkService : ICarParkService
{
    private readonly AppDbContext _context;

    public CarParkService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CarPark>> GetFreeParkingCarParksAsync()
    {
        return await _context.CarParks
            .Where(cp => cp.FreeParking.ToUpper() != "NO")
            .ToListAsync();
    }

    public async Task<IEnumerable<CarPark>> GetNightParkingCarParksAsync()
    {
        return await _context.CarParks
            .Where(cp => cp.NightParking.ToUpper() == "YES")
            .ToListAsync();
    }

    public async Task<IEnumerable<CarPark>> GetCarParksByHeightAsync(float minHeight)
    {
        return await _context.CarParks
            .Where(cp => cp.GantryHeight >= minHeight)
            .ToListAsync();
    }

    public async Task<bool> AddToFavoritesAsync(string carParkNo, string userId)
    {
        try
        {
            // Check if carpark exists using CarParkNo
            var carPark = await _context.CarParks
                .FirstOrDefaultAsync(cp => cp.CarParkNo == carParkNo);
            
            if (carPark == null)
            {
                return false;
            }

            // Check if already favorited
            var existingFavorite = await _context.UserFavorites
                .FirstOrDefaultAsync(f => f.CarParkNo == carParkNo && f.UserId == userId);

            if (existingFavorite != null)
            {
                return true; // Already favorited
            }

            // Add new favorite
            var favorite = new UserFavorite
            {
                UserId = userId,
                CarParkNo = carParkNo,
                CreatedAt = DateTime.UtcNow
            };

            await _context.UserFavorites.AddAsync(favorite);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<IEnumerable<CarPark>> GetUserFavoritesAsync(string userId)
    {
        return await _context.UserFavorites
            .Where(f => f.UserId == userId)
            .Include(f => f.CarPark)
            .Select(f => f.CarPark!)
            .ToListAsync();
    }

    public async Task<bool> RemoveFromFavoritesAsync(string carParkNo, string userId)
    {
        try
        {
            var favorite = await _context.UserFavorites
                .FirstOrDefaultAsync(f => f.CarParkNo == carParkNo && f.UserId == userId);

            if (favorite == null)
            {
                return false;
            }

            _context.UserFavorites.Remove(favorite);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}