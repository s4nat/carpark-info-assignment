using CsvHelper.Configuration;
using CarParkInfo.Core.Models;

namespace CarParkInfo.Data.Mappings;

public class CarParkMap : ClassMap<CarPark>
{
    public CarParkMap()
    {
        Map(m => m.CarParkNo).Name("car_park_no");
        Map(m => m.Address).Name("address");
        Map(m => m.XCoord).Name("x_coord");
        Map(m => m.YCoord).Name("y_coord");
        Map(m => m.CarParkType).Name("car_park_type");
        Map(m => m.TypeOfParkingSystem).Name("type_of_parking_system");
        Map(m => m.ShortTermParking).Name("short_term_parking");
        Map(m => m.FreeParking).Name("free_parking");
        Map(m => m.NightParking).Name("night_parking");
        Map(m => m.CarParkDecks).Name("car_park_decks");
        Map(m => m.GantryHeight).Name("gantry_height");
        Map(m => m.CarParkBasement).Name("car_park_basement");
    }
}