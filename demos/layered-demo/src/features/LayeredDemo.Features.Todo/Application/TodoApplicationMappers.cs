using LayeredDemo.Domain;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LayeredDemo.Application;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TodoToTodoDtoMapper : MapperBase<Todo, TodoDto>
{
    public override partial TodoDto Map(Todo source);

    public override partial void Map(Todo source, TodoDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CreateUpdateTodoDtoToTodoMapper : MapperBase<CreateUpdateTodoDto, Todo>
{
    public override partial Todo Map(CreateUpdateTodoDto source);

    public override partial void Map(CreateUpdateTodoDto source, Todo destination);
}
