using System;
using Volo.Abp.Application.Services;

namespace LayeredDemo.Application;

public interface ITodoAppService : ICrudAppService<TodoDto, Guid, TodoGetListInput, CreateUpdateTodoDto>
{
}
