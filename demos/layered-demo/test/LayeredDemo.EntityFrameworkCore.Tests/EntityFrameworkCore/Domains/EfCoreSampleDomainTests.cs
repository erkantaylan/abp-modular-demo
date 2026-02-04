using LayeredDemo.Samples;
using Xunit;

namespace LayeredDemo.EntityFrameworkCore.Domains;

[Collection(LayeredDemoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<LayeredDemoEntityFrameworkCoreTestModule>
{

}
