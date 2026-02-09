using Volo.Abp.GlobalFeatures;
using Volo.Abp.Threading;

namespace FluentUiDemo;

public static class FluentUiDemoGlobalFeatureConfigurator
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
        });
    }
}
