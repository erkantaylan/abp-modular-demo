using Volo.Abp.Application.Dtos;

namespace LayeredDemo.Application;

public class TodoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
