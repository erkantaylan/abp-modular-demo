using Volo.Abp.Modularity;

namespace MudBlazorDemo;

public abstract class MudBlazorDemoApplicationTestBase<TStartupModule> : MudBlazorDemoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
