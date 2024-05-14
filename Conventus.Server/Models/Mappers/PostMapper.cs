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
    [MapProperty(nameof(Post.Created), nameof(PostDto.Created), Use = nameof(TupleToDateTime))]
    public static partial PostDto ToDto(this Post post);

    [MapperIgnoreTarget(nameof(Post.Category))]
    [MapperIgnoreTarget(nameof(Post.Created))]
    [MapProperty(nameof(PostDto.Created), nameof(Post.DateCreated), Use = nameof(DateTimeToDate))]
    [MapProperty(nameof(PostDto.Created), nameof(Post.TimeCreated), Use = nameof(DateTimeToTime))]
    public static partial Post ToEntity(this PostDto postDto);

    private static DateOnly DateTimeToDate(DateTime time)
        => DateOnly.FromDateTime(time);

    private static TimeOnly DateTimeToTime(DateTime time)
        => TimeOnly.FromDateTime(time);

    private static DateTime TupleToDateTime((DateOnly date, TimeOnly time) tuple)
        => tuple.date.ToDateTime(tuple.time);
}
