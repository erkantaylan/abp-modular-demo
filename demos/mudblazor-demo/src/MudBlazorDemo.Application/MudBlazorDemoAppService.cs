using MudBlazorDemo.Localization;
using Volo.Abp.Application.Services;

namespace MudBlazorDemo;

/* Inherit your application services from this class.
 */
public abstract class MudBlazorDemoAppService : ApplicationService
{
    protected MudBlazorDemoAppService()
    {
        LocalizationResource = typeof(MudBlazorDemoResource);
    }
}
