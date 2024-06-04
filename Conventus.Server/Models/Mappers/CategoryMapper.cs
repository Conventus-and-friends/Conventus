using Conventus.Server.Models.Contracts;
using Conventus.Server.Models.DTO;
using Conventus.Server.Models.Entities;
using Riok.Mapperly.Abstractions;

namespace Conventus.Server.Models.Mappers;

[Mapper]
public static partial class CategoryMapper
{
    [MapperIgnoreSource(nameof(Category.Posts))]
    public static partial CategoryDto ToDto(this Category category);
    [MapperIgnoreTarget(nameof(Category.Posts))]
    public static partial Category ToEntity(this CategoryDto categoryDto);
    [MapperIgnoreSource(nameof(CategoryDto.Id))]
    public static partial CreateCategory ToCreateContract(this CategoryDto categoryDto);
}
