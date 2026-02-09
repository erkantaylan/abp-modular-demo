using Volo.Abp.Threading;

namespace FluentUiDemo.EntityFrameworkCore;

public static class FluentUiDemoEfCoreEntityExtensionMappings
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        FluentUiDemoGlobalFeatureConfigurator.Configure();
        FluentUiDemoModuleExtensionConfigurator.Configure();

        OneTimeRunner.Run(() =>
        {
        });
    }
}
