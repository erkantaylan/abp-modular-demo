using LayeredDemo.EntityFrameworkCore;
using LayeredDemo.Features.Todo;
using Volo.Abp.Modularity;

namespace LayeredDemo.Features.Todo.Tests;

[DependsOn(
    typeof(TodoFeatureModule),
    typeof(LayeredDemoEntityFrameworkCoreTestModule)
)]
public class TodoFeatureTestModule : AbpModule
{
}
