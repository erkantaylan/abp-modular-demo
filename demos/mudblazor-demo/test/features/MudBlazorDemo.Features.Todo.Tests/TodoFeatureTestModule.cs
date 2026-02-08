using MudBlazorDemo.EntityFrameworkCore;
using MudBlazorDemo.Features.Todo;
using Volo.Abp.Modularity;

namespace MudBlazorDemo.Features.Todo.Tests;

[DependsOn(
    typeof(TodoFeatureModule),
    typeof(MudBlazorDemoEntityFrameworkCoreTestModule)
)]
public class TodoFeatureTestModule : AbpModule
{
}
