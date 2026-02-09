using System;
using Volo.Abp.Application.Services;

namespace FluentUiDemo.Application;

public interface ITodoAppService : ICrudAppService<TodoDto, Guid, TodoGetListInput, CreateUpdateTodoDto>
{
}
