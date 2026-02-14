using Volo.Abp.Modularity;

namespace AngularDemo;

public abstract class AngularDemoApplicationTestBase<TStartupModule> : AngularDemoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
