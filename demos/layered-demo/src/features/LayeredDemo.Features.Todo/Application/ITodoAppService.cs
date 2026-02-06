using System;
using Volo.Abp.Application.Services;

namespace LayeredDemo.Todos;

public interface ITodoAppService : ICrudAppService<TodoDto, Guid, TodoGetListInput, CreateUpdateTodoDto>
{
}
