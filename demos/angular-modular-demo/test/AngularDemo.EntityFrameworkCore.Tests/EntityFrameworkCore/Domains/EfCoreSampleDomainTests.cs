using AngularDemo.Samples;
using Xunit;

namespace AngularDemo.EntityFrameworkCore.Domains;

[Collection(AngularDemoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<AngularDemoEntityFrameworkCoreTestModule>
{

}
