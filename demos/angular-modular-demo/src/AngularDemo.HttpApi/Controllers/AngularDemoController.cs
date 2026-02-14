using AngularDemo.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace AngularDemo.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AngularDemoController : AbpControllerBase
{
    protected AngularDemoController()
    {
        LocalizationResource = typeof(AngularDemoResource);
    }
}
