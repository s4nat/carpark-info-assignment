namespace CarParkInfo.Core.Models;

public class CarPark
{
    public string Id { get; set; } = string.Empty;
    public string CarParkNo { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public float XCoord { get; set; }
    public float YCoord { get; set; }
    public string CarParkType { get; set; } = string.Empty;
    public string TypeOfParkingSystem { get; set; } = string.Empty;
    public string ShortTermParking { get; set; } = string.Empty;
    public string FreeParking { get; set; } = string.Empty;
    public string NightParking { get; set; } = string.Empty;
    public int CarParkDecks { get; set; }
    public float GantryHeight { get; set; }
    public string CarParkBasement { get; set; } = string.Empty;
}