using System;
using System.Threading.Tasks;
using MudBlazorDemo.Application;
using MudBlazorDemo.Domain;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace MudBlazorDemo.Features.Todo.Tests;

public sealed class TodoAppServiceTests : TodoFeatureTestBase
{
    private readonly ITodoAppService _todoAppService;

    public TodoAppServiceTests()
    {
        _todoAppService = GetRequiredService<ITodoAppService>();
    }

    [Fact]
    public async Task Should_Create_Todo()
    {
        TodoDto result = await _todoAppService.CreateAsync(
            new CreateUpdateTodoDto
            {
                Title = "Test Todo",
                Description = "Test Description",
                Status = TodoStatus.NotStarted,
                DueDate = DateTime.UtcNow.AddDays(7)
            });

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Title.ShouldBe("Test Todo");
        result.Description.ShouldBe("Test Description");
        result.Status.ShouldBe(TodoStatus.NotStarted);
        result.DueDate.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        await _todoAppService.CreateAsync(
            new CreateUpdateTodoDto
            {
                Title = "Todo 1",
                Status = TodoStatus.NotStarted
            });

        await _todoAppService.CreateAsync(
            new CreateUpdateTodoDto
            {
                Title = "Todo 2",
                Status = TodoStatus.InProgress
            });

        PagedResultDto<TodoDto> result = await _todoAppService.GetListAsync(new TodoGetListInput());

        result.ShouldNotBeNull();
        result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task Should_Update_Todo()
    {
        TodoDto created = await _todoAppService.CreateAsync(
            new CreateUpdateTodoDto
            {
                Title = "Original Title",
                Status = TodoStatus.NotStarted
            });

        TodoDto updated = await _todoAppService.UpdateAsync(
            created.Id,
            new CreateUpdateTodoDto
            {
                Title = "Updated Title",
                Status = TodoStatus.InProgress,
                Description = "Added description"
            });

        updated.Title.ShouldBe("Updated Title");
        updated.Status.ShouldBe(TodoStatus.InProgress);
        updated.Description.ShouldBe("Added description");
    }

    [Fact]
    public async Task Should_Delete_Todo()
    {
        TodoDto created = await _todoAppService.CreateAsync(
            new CreateUpdateTodoDto
            {
                Title = "To Be Deleted",
                Status = TodoStatus.NotStarted
            });

        await _todoAppService.DeleteAsync(created.Id);

        PagedResultDto<TodoDto> list = await _todoAppService.GetListAsync(
            new TodoGetListInput
            {
                Filter = "To Be Deleted"
            });

        list.Items.ShouldNotContain(x => x.Id == created.Id);
    }

    [Fact]
    public async Task Should_Filter_By_Title()
    {
        await _todoAppService.CreateAsync(
            new CreateUpdateTodoDto
            {
                Title = "Unique Filter Test",
                Status = TodoStatus.NotStarted
            });

        await _todoAppService.CreateAsync(
            new CreateUpdateTodoDto
            {
                Title = "Other Todo",
                Status = TodoStatus.Done
            });

        PagedResultDto<TodoDto> result = await _todoAppService.GetListAsync(
            new TodoGetListInput
            {
                Filter = "Unique Filter"
            });

        result.Items.ShouldContain(x => x.Title == "Unique Filter Test");
        result.Items.ShouldNotContain(x => x.Title == "Other Todo");
    }

    [Fact]
    public async Task Should_Track_Creator()
    {
        TodoDto created = await _todoAppService.CreateAsync(
            new CreateUpdateTodoDto
            {
                Title = "Creator Tracking Test",
                Status = TodoStatus.NotStarted
            });

        created.CreatorId.ShouldNotBeNull();
        created.CreationTime.ShouldNotBe(default);
    }

    [Fact]
    public async Task Should_Mark_As_Done()
    {
        TodoDto created = await _todoAppService.CreateAsync(
            new CreateUpdateTodoDto
            {
                Title = "Mark Done Test",
                Status = TodoStatus.NotStarted
            });

        TodoDto updated = await _todoAppService.UpdateAsync(
            created.Id,
            new CreateUpdateTodoDto
            {
                Title = created.Title,
                Description = created.Description,
                Status = TodoStatus.Done,
                DueDate = created.DueDate
            });

        updated.Status.ShouldBe(TodoStatus.Done);
    }
}