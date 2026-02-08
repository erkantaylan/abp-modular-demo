using MudBlazorDemo.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MudBlazorDemo.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class MudBlazorDemoController : AbpControllerBase
{
    protected MudBlazorDemoController()
    {
        LocalizationResource = typeof(MudBlazorDemoResource);
    }
}
