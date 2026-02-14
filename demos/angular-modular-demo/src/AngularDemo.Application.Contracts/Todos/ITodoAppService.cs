using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AngularDemo.Todos;

public interface ITodoAppService : IApplicationService
{
    Task<ListResultDto<TodoDto>> GetListAsync();
    Task<TodoDto> CreateAsync(CreateTodoDto input);
    Task<TodoDto> CompleteAsync(Guid id);
}
