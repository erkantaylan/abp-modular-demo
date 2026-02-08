using System.Threading.Tasks;
using MudBlazorDemo.ServiceDefaults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace MudBlazorDemo.DbMigrator;

class Program
{
    static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
#if DEBUG
                .MinimumLevel.Override("MudBlazorDemo", LogEventLevel.Debug)
#else
                .MinimumLevel.Override("MudBlazorDemo", LogEventLevel.Information)
#endif
                .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console())
            .CreateLogger();

        var builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.AddJsonFile("appsettings.secrets.json", optional: true, reloadOnChange: true);
        builder.AddServiceDefaults();
        builder.Logging.ClearProviders();
        builder.Services.AddHostedService<DbMigratorHostedService>();

        var host = builder.Build();
        await host.RunAsync();
    }
}
