using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MudBlazorDemo.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class MudBlazorDemoDbContextFactory : IDesignTimeDbContextFactory<MudBlazorDemoDbContext>
{
    public MudBlazorDemoDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        MudBlazorDemoEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<MudBlazorDemoDbContext>()
            .UseNpgsql(configuration.GetConnectionString("Default"));
        
        return new MudBlazorDemoDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MudBlazorDemo.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
