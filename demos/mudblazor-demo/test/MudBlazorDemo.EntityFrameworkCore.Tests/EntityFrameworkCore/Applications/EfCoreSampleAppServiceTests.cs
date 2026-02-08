using MudBlazorDemo.Samples;
using Xunit;

namespace MudBlazorDemo.EntityFrameworkCore.Applications;

[Collection(MudBlazorDemoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<MudBlazorDemoEntityFrameworkCoreTestModule>
{

}
