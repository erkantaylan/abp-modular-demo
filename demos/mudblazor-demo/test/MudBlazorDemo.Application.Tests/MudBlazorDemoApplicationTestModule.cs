using Volo.Abp.Modularity;

namespace MudBlazorDemo;

[DependsOn(
    typeof(MudBlazorDemoApplicationModule),
    typeof(MudBlazorDemoDomainTestModule)
)]
public class MudBlazorDemoApplicationTestModule : AbpModule
{

}
