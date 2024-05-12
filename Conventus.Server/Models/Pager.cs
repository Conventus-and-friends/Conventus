using System.Diagnostics.Contracts;

namespace Conventus.Server.Models;

public sealed class Pager : IModelValidating
{
    internal const int MAX_PAGE_SIZE = 30;
    public int Page { get; set; } = 1;

    private int _length = 20;
    public int Length
    {
        get => _length;
        set => _length = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
    }

    [Pure]
    public bool IsValid()
    {
        return Page > 0 && Length > 0;
    }
}
