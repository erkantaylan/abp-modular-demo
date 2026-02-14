using AngularDemo.Samples;
using Xunit;

namespace AngularDemo.EntityFrameworkCore.Applications;

[Collection(AngularDemoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<AngularDemoEntityFrameworkCoreTestModule>
{

}
