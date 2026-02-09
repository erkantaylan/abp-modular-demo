using Volo.Abp.Application.Dtos;

namespace FluentUiDemo.Application;

public class ProductGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
