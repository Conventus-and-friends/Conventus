using Conventus.Server.Models.DTO;
using Conventus.Server.Models.Entities;
using Riok.Mapperly.Abstractions;

namespace Conventus.Server.Models.Mappers;

[Mapper]
public static partial class PostMapper
{
    [MapperIgnoreSource(nameof(Post.Category))]
    public static partial PostDto ToDto(this Post post);
    [MapperIgnoreTarget(nameof(Post.Category))]
    public static partial Post ToEntity(this PostDto postDto);
}
