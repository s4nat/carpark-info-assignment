using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CarParkInfo.Data.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=../app.db");
        return new AppDbContext(optionsBuilder.Options);
    }
}