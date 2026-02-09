using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FluentUiDemo.EntityFrameworkCore;

public class FluentUiDemoDbContextFactory : IDesignTimeDbContextFactory<FluentUiDemoDbContext>
{
    public FluentUiDemoDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        FluentUiDemoEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<FluentUiDemoDbContext>()
            .UseNpgsql(configuration.GetConnectionString("Default"));

        return new FluentUiDemoDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../FluentUiDemo.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
