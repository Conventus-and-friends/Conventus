using Conventus.Server.Models.Contracts;
using Conventus.Server.Models.DTO;
using Conventus.Server.Models.Entities;
using Riok.Mapperly.Abstractions;

namespace Conventus.Server.Models.Mappers;

[Mapper]
public static partial class CommentMapper
{
    [MapperIgnoreSource(nameof(Comment.Post))]
    [MapperIgnoreSource(nameof(Comment.TimeCreated))]
    [MapperIgnoreSource(nameof(Comment.DateCreated))]
    public static partial CommentDto ToDto(this Comment entity);

    [MapperIgnoreTarget(nameof(Comment.Post))]
    [MapperIgnoreTarget(nameof(Comment.Created))]
    [MapProperty(nameof(CommentDto.Created), nameof(Comment.DateCreated))]
    [MapProperty(nameof(CommentDto.Created), nameof(Comment.TimeCreated))]
    public static partial Comment ToEntity(this CommentDto dto);

    [MapperIgnoreSource(nameof(CommentDto.Id))]
    [MapperIgnoreSource(nameof(CommentDto.Created))]
    public static partial CreateComment ToCreateContract(this CommentDto dto);
}
