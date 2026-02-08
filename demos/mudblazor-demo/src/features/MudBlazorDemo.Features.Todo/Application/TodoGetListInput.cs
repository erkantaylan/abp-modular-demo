using Volo.Abp.Application.Dtos;

namespace MudBlazorDemo.Application;

public class TodoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
