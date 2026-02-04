using LayeredDemo.Localization;
using Volo.Abp.Application.Services;

namespace LayeredDemo;

/* Inherit your application services from this class.
 */
public abstract class LayeredDemoAppService : ApplicationService
{
    protected LayeredDemoAppService()
    {
        LocalizationResource = typeof(LayeredDemoResource);
    }
}
