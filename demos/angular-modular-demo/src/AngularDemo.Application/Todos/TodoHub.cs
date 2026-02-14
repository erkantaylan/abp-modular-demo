using Microsoft.AspNetCore.Authorization;
using Volo.Abp.AspNetCore.SignalR;

namespace AngularDemo.Todos;

[Authorize]
public class TodoHub : AbpHub
{
}
