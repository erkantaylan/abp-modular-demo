using System;
using Volo.Abp.Application.Services;

namespace FluentUiDemo.Application;

public interface IProductAppService : ICrudAppService<ProductDto, Guid, ProductGetListInput, CreateUpdateProductDto>
{
}
