using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularDemo.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace AngularDemo.Todos;

[Authorize]
public class TodoAppService : AngularDemoAppService, ITodoAppService
{
    private readonly IRepository<Todo, Guid> _todoRepository;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IHubContext<TodoHub> _hubContext;

    public TodoAppService(
        IRepository<Todo, Guid> todoRepository,
        IIdentityUserRepository userRepository,
        IHubContext<TodoHub> hubContext)
    {
        _todoRepository = todoRepository;
        _userRepository = userRepository;
        _hubContext = hubContext;
    }

    public async Task<ListResultDto<TodoDto>> GetListAsync()
    {
        var todos = await _todoRepository.GetListAsync();
        var orderedTodos = todos.OrderByDescending(t => t.CreationTime).ToList();

        var creatorIds = orderedTodos.Where(t => t.CreatorId.HasValue).Select(t => t.CreatorId!.Value).Distinct().ToList();
        var completedByIds = orderedTodos.Where(t => t.CompletedBy.HasValue).Select(t => t.CompletedBy!.Value).Distinct().ToList();
        var allUserIds = creatorIds.Union(completedByIds).Distinct().ToList();

        var users = new Dictionary<Guid, string>();
        foreach (var userId in allUserIds)
        {
            var user = await _userRepository.FindAsync(userId);
            if (user != null)
            {
                users[userId] = user.UserName!;
            }
        }

        var dtos = orderedTodos.Select(todo => new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            IsCompleted = todo.IsCompleted,
            CompletedBy = todo.CompletedBy,
            CompletionTime = todo.CompletionTime,
            CreationTime = todo.CreationTime,
            CreatorId = todo.CreatorId,
            LastModificationTime = todo.LastModificationTime,
            LastModifierId = todo.LastModifierId,
            CreatorUserName = todo.CreatorId.HasValue && users.ContainsKey(todo.CreatorId.Value) ? users[todo.CreatorId.Value] : null,
            CompletedByUserName = todo.CompletedBy.HasValue && users.ContainsKey(todo.CompletedBy.Value) ? users[todo.CompletedBy.Value] : null
        }).ToList();

        return new ListResultDto<TodoDto>(dtos);
    }

    [Authorize(AngularDemoPermissions.Todos.Create)]
    public async Task<TodoDto> CreateAsync(CreateTodoDto input)
    {
        var todo = new Todo(GuidGenerator.Create(), input.Title, input.Description);
        await _todoRepository.InsertAsync(todo);

        var currentUser = await _userRepository.FindAsync(CurrentUser.GetId());
        var dto = new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            IsCompleted = false,
            CreationTime = todo.CreationTime,
            CreatorId = todo.CreatorId,
            CreatorUserName = currentUser?.UserName
        };

        await _hubContext.Clients.All.SendAsync("TodoCreated", dto);

        return dto;
    }

    [Authorize(AngularDemoPermissions.Todos.Complete)]
    public async Task<TodoDto> CompleteAsync(Guid id)
    {
        var todo = await _todoRepository.GetAsync(id);
        todo.Complete(CurrentUser.GetId());
        await _todoRepository.UpdateAsync(todo);

        var users = new Dictionary<Guid, string>();
        if (todo.CreatorId.HasValue)
        {
            var creator = await _userRepository.FindAsync(todo.CreatorId.Value);
            if (creator != null) users[creator.Id] = creator.UserName!;
        }
        var completedByUser = await _userRepository.FindAsync(CurrentUser.GetId());
        if (completedByUser != null) users[completedByUser.Id] = completedByUser.UserName!;

        var dto = new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            IsCompleted = true,
            CompletedBy = todo.CompletedBy,
            CompletionTime = todo.CompletionTime,
            CreationTime = todo.CreationTime,
            CreatorId = todo.CreatorId,
            CreatorUserName = todo.CreatorId.HasValue && users.ContainsKey(todo.CreatorId.Value) ? users[todo.CreatorId.Value] : null,
            CompletedByUserName = todo.CompletedBy.HasValue && users.ContainsKey(todo.CompletedBy.Value) ? users[todo.CompletedBy.Value] : null
        };

        await _hubContext.Clients.All.SendAsync("TodoCompleted", dto);

        return dto;
    }
}
