using System.Threading.Tasks;

namespace MudBlazorDemo.Data;

public interface IMudBlazorDemoDbSchemaMigrator
{
    Task MigrateAsync();
}
