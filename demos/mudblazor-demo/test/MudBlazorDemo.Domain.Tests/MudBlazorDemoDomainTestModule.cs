using Volo.Abp.Modularity;

namespace MudBlazorDemo;

[DependsOn(
    typeof(MudBlazorDemoDomainModule),
    typeof(MudBlazorDemoTestBaseModule)
)]
public class MudBlazorDemoDomainTestModule : AbpModule
{

}
