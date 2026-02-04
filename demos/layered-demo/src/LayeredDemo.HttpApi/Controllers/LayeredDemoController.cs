using LayeredDemo.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace LayeredDemo.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class LayeredDemoController : AbpControllerBase
{
    protected LayeredDemoController()
    {
        LocalizationResource = typeof(LayeredDemoResource);
    }
}
