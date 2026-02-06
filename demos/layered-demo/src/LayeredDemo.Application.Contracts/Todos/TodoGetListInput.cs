using Volo.Abp.Application.Dtos;

namespace LayeredDemo.Todos;

public class TodoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
