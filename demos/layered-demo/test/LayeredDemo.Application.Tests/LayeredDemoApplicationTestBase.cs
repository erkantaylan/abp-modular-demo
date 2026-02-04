using Volo.Abp.Modularity;

namespace LayeredDemo;

public abstract class LayeredDemoApplicationTestBase<TStartupModule> : LayeredDemoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
