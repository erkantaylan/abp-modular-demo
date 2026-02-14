using Xunit;

namespace AngularDemo.EntityFrameworkCore;

[CollectionDefinition(AngularDemoTestConsts.CollectionDefinitionName)]
public class AngularDemoEntityFrameworkCoreCollection : ICollectionFixture<AngularDemoEntityFrameworkCoreFixture>
{

}
