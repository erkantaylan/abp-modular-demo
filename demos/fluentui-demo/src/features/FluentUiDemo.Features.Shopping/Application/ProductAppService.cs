using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentUiDemo.Domain;
using FluentUiDemo.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace FluentUiDemo.Application;

public class ProductAppService
    : CrudAppService<Product, ProductDto, Guid, ProductGetListInput, CreateUpdateProductDto>,
      IProductAppService
{
    public ProductAppService(IRepository<Product, Guid> repository)
        : base(repository)
    {
        GetPolicyName = ShoppingPermissions.Products.Default;
        GetListPolicyName = ShoppingPermissions.Products.Default;
        CreatePolicyName = ShoppingPermissions.Products.Create;
        UpdatePolicyName = ShoppingPermissions.Products.Edit;
        DeletePolicyName = ShoppingPermissions.Products.Delete;
    }

    public override async Task<PagedResultDto<ProductDto>> GetListAsync(ProductGetListInput input)
    {
        var queryable = await Repository.GetQueryableAsync();

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            queryable = queryable.Where(p =>
                p.Name.Contains(input.Filter) ||
                (p.Description != null && p.Description.Contains(input.Filter)));
        }

        var totalCount = await AsyncExecuter.CountAsync(queryable);

        if (!string.IsNullOrWhiteSpace(input.Sorting))
        {
            queryable = ApplySorting(queryable, input);
        }
        else
        {
            queryable = queryable.OrderBy(p => p.Name);
        }

        queryable = ApplyPaging(queryable, input);

        var products = await AsyncExecuter.ToListAsync(queryable);
        var dtos = ObjectMapper.Map<List<Product>, List<ProductDto>>(products);

        return new PagedResultDto<ProductDto>(totalCount, dtos);
    }
}
