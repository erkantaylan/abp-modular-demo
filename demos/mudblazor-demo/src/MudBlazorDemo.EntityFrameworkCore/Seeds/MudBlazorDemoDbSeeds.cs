using System.Threading.Tasks;
using MudBlazorDemo.EntityFrameworkCore;
using MudBlazorDemo.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MudBlazorDemo.Seeds;

public class MudBlazorDemoDbSeeds : IDbSeeder<MudBlazorDemoDbContext>
{
    private readonly ILogger<MudBlazorDemoDbSeeds> _logger;

    public MudBlazorDemoDbSeeds(ILogger<MudBlazorDemoDbSeeds> logger)
    {
        _logger = logger;
    }

    public async Task SeedAsync(MudBlazorDemoDbContext context)
    {
        // Check if data already exists
        bool hasUsers = await context.Users.AnyAsync();

        if (hasUsers)
        {
            _logger.LogInformation("Database already seeded, skipping seed data");
            return;
        }

        _logger.LogInformation("Seeding database with initial data...");

        // Note: ABP Framework handles identity and role seeding through its own
        // IDataSeedContributor implementations (IdentityDataSeedContributor, etc.)
        // This seeder is for custom application-specific seed data.
        //
        // Add your custom seed data here. Examples:
        //
        // await context.YourEntities.AddRangeAsync(
        //     new YourEntity { Name = "Sample 1" },
        //     new YourEntity { Name = "Sample 2" }
        // );
        //
        // await context.SaveChangesAsync();

        _logger.LogInformation("Database seeding completed");
    }
}
