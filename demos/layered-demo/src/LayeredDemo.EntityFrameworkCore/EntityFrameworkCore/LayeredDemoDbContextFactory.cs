using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LayeredDemo.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class LayeredDemoDbContextFactory : IDesignTimeDbContextFactory<LayeredDemoDbContext>
{
    public LayeredDemoDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        LayeredDemoEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<LayeredDemoDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"));
        
        return new LayeredDemoDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../LayeredDemo.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
