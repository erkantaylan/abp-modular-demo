using AngularDemo.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AngularDemo.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AngularDemoEntityFrameworkCoreModule),
    typeof(AngularDemoApplicationContractsModule)
)]
public class AngularDemoDbMigratorModule : AbpModule
{
}
