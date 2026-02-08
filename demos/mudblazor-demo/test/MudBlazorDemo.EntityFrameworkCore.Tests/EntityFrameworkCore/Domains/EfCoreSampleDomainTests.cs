using MudBlazorDemo.Samples;
using Xunit;

namespace MudBlazorDemo.EntityFrameworkCore.Domains;

[Collection(MudBlazorDemoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<MudBlazorDemoEntityFrameworkCoreTestModule>
{

}
