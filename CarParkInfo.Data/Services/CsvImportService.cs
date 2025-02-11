using System.Globalization;
using CarParkInfo.Core.Interfaces;
using CarParkInfo.Core.Models;
using CarParkInfo.Data.Context;
using CarParkInfo.Data.Mappings;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CarParkInfo.Data.Services;

public class CsvImportService : ICsvImportService
{
   private readonly AppDbContext _context;

   public CsvImportService(AppDbContext context)
   {
       _context = context;
   }

   public async Task ImportCarParksAsync(Stream csvStream)
   {
       using var transaction = await _context.Database.BeginTransactionAsync();
       try
       {
           _context.CarParks.RemoveRange(await _context.CarParks.ToListAsync());

           using var reader = new StreamReader(csvStream);
           using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
           {
               HeaderValidated = null,
               MissingFieldFound = null
           });

           csv.Context.RegisterClassMap<CarParkMap>();
           var records = csv.GetRecords<CarPark>();
           
           foreach (var record in records)
           {
               record.Id = Guid.NewGuid().ToString();
               await _context.CarParks.AddAsync(record);
           }

           await _context.SaveChangesAsync();
           await transaction.CommitAsync();
       }
       catch (Exception)
       {
           await transaction.RollbackAsync();
           throw;
       }
   }
}