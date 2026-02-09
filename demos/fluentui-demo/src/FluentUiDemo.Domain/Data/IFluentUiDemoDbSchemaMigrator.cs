using System.Threading.Tasks;

namespace FluentUiDemo.Data;

public interface IFluentUiDemoDbSchemaMigrator
{
    Task MigrateAsync();
}
