using LayeredDemo.Samples;
using Xunit;

namespace LayeredDemo.EntityFrameworkCore.Applications;

[Collection(LayeredDemoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<LayeredDemoEntityFrameworkCoreTestModule>
{

}
