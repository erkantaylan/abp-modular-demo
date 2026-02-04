using Xunit;

namespace LayeredDemo.EntityFrameworkCore;

[CollectionDefinition(LayeredDemoTestConsts.CollectionDefinitionName)]
public class LayeredDemoEntityFrameworkCoreCollection : ICollectionFixture<LayeredDemoEntityFrameworkCoreFixture>
{

}
