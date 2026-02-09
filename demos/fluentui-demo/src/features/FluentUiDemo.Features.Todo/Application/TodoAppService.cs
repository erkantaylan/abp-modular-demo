using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentUiDemo.Domain;
using FluentUiDemo.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace FluentUiDemo.Application;

public class TodoAppService
    : CrudAppService<Todo, TodoDto, Guid, TodoGetListInput, CreateUpdateTodoDto>,
      ITodoAppService
{
    private readonly IIdentityUserRepository _userRepository;

    public TodoAppService(
        IRepository<Todo, Guid> repository,
        IIdentityUserRepository userRepository)
        : base(repository)
    {
        _userRepository = userRepository;

        GetPolicyName = TodoPermissions.Todos.Default;
        GetListPolicyName = TodoPermissions.Todos.Default;
        CreatePolicyName = TodoPermissions.Todos.Create;
        UpdatePolicyName = TodoPermissions.Todos.Edit;
        DeletePolicyName = TodoPermissions.Todos.Delete;
    }

    public override async Task<TodoDto> UpdateAsync(Guid id, CreateUpdateTodoDto input)
    {
        var entity = await Repository.GetAsync(id);

        if (input.Status == TodoStatus.Done && entity.Status != TodoStatus.Done)
        {
            entity.CompleterId = CurrentUser.GetId();
            entity.CompletionTime = DateTime.UtcNow;
        }
        else if (input.Status != TodoStatus.Done)
        {
            entity.CompleterId = null;
            entity.CompletionTime = null;
        }

        MapToEntity(input, entity);
        await Repository.UpdateAsync(entity, autoSave: true);

        return await MapToGetOutputDtoAsync(entity);
    }

    public override async Task<PagedResultDto<TodoDto>> GetListAsync(TodoGetListInput input)
    {
        var queryable = await Repository.GetQueryableAsync();

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            queryable = queryable.Where(t =>
                t.Title.Contains(input.Filter));
        }

        var totalCount = await AsyncExecuter.CountAsync(queryable);

        if (!string.IsNullOrWhiteSpace(input.Sorting))
        {
            queryable = ApplySorting(queryable, input);
        }
        else
        {
            queryable = queryable.OrderByDescending(t => t.CreationTime);
        }

        queryable = ApplyPaging(queryable, input);

        var todos = await AsyncExecuter.ToListAsync(queryable);
        var dtos = ObjectMapper.Map<List<Todo>, List<TodoDto>>(todos);

        var userIds = todos
            .SelectMany(t => new[] { t.CreatorId, t.CompleterId })
            .Where(id => id.HasValue)
            .Select(id => id!.Value)
            .Distinct()
            .ToList();

        if (userIds.Count > 0)
        {
            var users = await _userRepository.GetListByIdsAsync(userIds);
            var userDict = users.ToDictionary(u => u.Id, u => u.UserName);

            foreach (var dto in dtos)
            {
                if (dto.CreatorId.HasValue && userDict.TryGetValue(dto.CreatorId.Value, out var creatorName))
                {
                    dto.CreatorUserName = creatorName;
                }

                if (dto.CompleterId.HasValue && userDict.TryGetValue(dto.CompleterId.Value, out var completerName))
                {
                    dto.CompleterUserName = completerName;
                }
            }
        }

        return new PagedResultDto<TodoDto>(totalCount, dtos);
    }
}
