using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LayeredDemo.Domain;
using LayeredDemo.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace LayeredDemo.Application;

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

        var creatorIds = todos
            .Where(t => t.CreatorId.HasValue)
            .Select(t => t.CreatorId!.Value)
            .Distinct()
            .ToList();

        if (creatorIds.Count > 0)
        {
            var users = await _userRepository.GetListByIdsAsync(creatorIds);
            var userDict = users.ToDictionary(u => u.Id, u => u.UserName);

            foreach (var dto in dtos)
            {
                if (dto.CreatorId.HasValue && userDict.TryGetValue(dto.CreatorId.Value, out var userName))
                {
                    dto.CreatorUserName = userName;
                }
            }
        }

        return new PagedResultDto<TodoDto>(totalCount, dtos);
    }
}
