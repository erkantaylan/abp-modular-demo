using Volo.Abp.Modularity;

namespace LayeredDemo;

[DependsOn(
    typeof(LayeredDemoApplicationModule),
    typeof(LayeredDemoDomainTestModule)
)]
public class LayeredDemoApplicationTestModule : AbpModule
{

}
