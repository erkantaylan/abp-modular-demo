using Volo.Abp.Threading;

namespace FluentUiDemo;

public static class FluentUiDemoDtoExtensions
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
        });
    }
}
