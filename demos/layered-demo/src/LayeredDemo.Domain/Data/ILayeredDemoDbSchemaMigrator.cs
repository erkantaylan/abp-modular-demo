using System.Threading.Tasks;

namespace LayeredDemo.Data;

public interface ILayeredDemoDbSchemaMigrator
{
    Task MigrateAsync();
}
