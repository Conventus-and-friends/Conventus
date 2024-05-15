using Conventus.Server.Models.DTO;
using Conventus.Server.Models.Entities;
using Riok.Mapperly.Abstractions;

namespace Conventus.Server.Models.Mappers;

[Mapper]
public static partial class PostMapper
{
    [MapperIgnoreSource(nameof(Post.Category))]
    [MapperIgnoreSource(nameof(Post.TimeCreated))]
    [MapperIgnoreSource(nameof(Post.DateCreated))]
    [MapperIgnoreSource(nameof(Post.Comments))]
    public static partial PostDto ToDto(this Post post);

    [MapperIgnoreTarget(nameof(Post.Category))]
    [MapperIgnoreTarget(nameof(Post.Created))]
    [MapperIgnoreTarget(nameof(Post.Comments))]
    [MapProperty(nameof(PostDto.Created), nameof(Post.DateCreated))]
    [MapProperty(nameof(PostDto.Created), nameof(Post.TimeCreated))]
    public static partial Post ToEntity(this PostDto postDto);
}
