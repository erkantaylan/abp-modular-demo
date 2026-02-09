using Volo.Abp.Application.Dtos;

namespace FluentUiDemo.Application;

public class TodoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
