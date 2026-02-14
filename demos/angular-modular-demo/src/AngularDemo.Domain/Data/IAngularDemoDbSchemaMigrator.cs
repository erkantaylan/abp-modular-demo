using System.Threading.Tasks;

namespace AngularDemo.Data;

public interface IAngularDemoDbSchemaMigrator
{
    Task MigrateAsync();
}
