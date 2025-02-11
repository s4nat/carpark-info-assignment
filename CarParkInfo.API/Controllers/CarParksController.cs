using CarParkInfo.API.Attributes;
using CarParkInfo.Core.Interfaces;
using CarParkInfo.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarParkInfo.API.Controllers;

/// <summary>
/// Controller for managing carpark information
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CarParksController : ControllerBase
{
    private readonly ICarParkService _carParkService;
    private readonly ICsvImportService _csvImportService;

    public CarParksController(ICarParkService carParkService, ICsvImportService csvImportService)
    {
        _carParkService = carParkService;
        _csvImportService = csvImportService;
    }

    /// <summary>
    /// Get all carparks that offer free parking
    /// </summary>
    [HttpGet("free-parking")]
    [ProducesResponseType(typeof(IEnumerable<CarPark>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarPark>>> GetFreeParkingCarParks()
    {
        var carParks = await _carParkService.GetFreeParkingCarParksAsync();
        return Ok(carParks);
    }

    /// <summary>
    /// Get all carparks that offer night parking
    /// </summary>
    [HttpGet("night-parking")]
    [ProducesResponseType(typeof(IEnumerable<CarPark>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarPark>>> GetNightParkingCarParks()
    {
        var carParks = await _carParkService.GetNightParkingCarParksAsync();
        return Ok(carParks);
    }

    /// <summary>
    /// Get carparks that meet the minimum height requirement
    /// </summary>
    /// <param name="minHeight">Minimum vehicle height in meters</param>
    [HttpGet("by-height/{minHeight}")]
    [ProducesResponseType(typeof(IEnumerable<CarPark>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarPark>>> GetCarParksByHeight(float minHeight)
    {
        var carParks = await _carParkService.GetCarParksByHeightAsync(minHeight);
        return Ok(carParks);
    }

    /// <summary>
    /// Import carpark data from a CSV file
    /// </summary>
    [HttpPost("import")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportCarParks(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        using var stream = file.OpenReadStream();
        try
        {
            await _csvImportService.ImportCarParksAsync(stream);
            return Ok("Import successful");
        }
        catch (Exception ex)
        {
            return BadRequest($"Import failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Add a carpark to user's favorites
    /// </summary>
    /// <param name="carParkNo">Number of the carpark to favorite</param>
    [Authorize]
    [HttpPost("favorites/{carParkNo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddToFavorites(string carParkNo)
    {
        var userId = (string)HttpContext.Items["UserId"]!;
        
        var result = await _carParkService.AddToFavoritesAsync(carParkNo, userId);
        if (result)
        {
            return Ok("Added to favorites");
        }
        return BadRequest("Failed to add to favorites");
    }

    /// <summary>
    /// Get user's favorite carparks
    /// </summary>
    [Authorize]
    [HttpGet("favorites")]
    [ProducesResponseType(typeof(IEnumerable<CarPark>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<CarPark>>> GetUserFavorites()
    {
        var userId = (string)HttpContext.Items["UserId"]!;
        
        var favorites = await _carParkService.GetUserFavoritesAsync(userId);
        return Ok(favorites);
    }

    /// <summary>
    /// Remove a carpark from user's favorites
    /// </summary>
    /// <param name="carParkNo">Number of the carpark to remove from favorites</param>
    [Authorize]
    [HttpDelete("favorites/{carParkNo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveFromFavorites(string carParkNo)
    {
        var userId = (string)HttpContext.Items["UserId"]!;
        
        var result = await _carParkService.RemoveFromFavoritesAsync(carParkNo, userId);
        if (result)
        {
            return Ok("Removed from favorites");
        }
        return BadRequest("Failed to remove from favorites");
    }
}