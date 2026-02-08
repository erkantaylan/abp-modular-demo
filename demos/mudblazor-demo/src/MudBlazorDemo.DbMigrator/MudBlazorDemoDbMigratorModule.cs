using MudBlazorDemo.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MudBlazorDemo.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(MudBlazorDemoEntityFrameworkCoreModule),
    typeof(MudBlazorDemoApplicationContractsModule)
)]
public class MudBlazorDemoDbMigratorModule : AbpModule
{
}
