using AngularDemo.Localization;
using Volo.Abp.Application.Services;

namespace AngularDemo;

/* Inherit your application services from this class.
 */
public abstract class AngularDemoAppService : ApplicationService
{
    protected AngularDemoAppService()
    {
        LocalizationResource = typeof(AngularDemoResource);
    }
}
