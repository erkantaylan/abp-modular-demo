using Xunit;

namespace MudBlazorDemo.EntityFrameworkCore;

[CollectionDefinition(MudBlazorDemoTestConsts.CollectionDefinitionName)]
public class MudBlazorDemoEntityFrameworkCoreCollection : ICollectionFixture<MudBlazorDemoEntityFrameworkCoreFixture>
{

}
