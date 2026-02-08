using System;
using Volo.Abp.Application.Services;

namespace MudBlazorDemo.Application;

public interface ITodoAppService : ICrudAppService<TodoDto, Guid, TodoGetListInput, CreateUpdateTodoDto>
{
}
