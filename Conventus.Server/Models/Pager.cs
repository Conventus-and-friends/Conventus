namespace Conventus.Server.Models;

public sealed class Pager : IModelValidating
{
    internal const int MAX_PAGE_SIZE = 30;
    public int Page { get; set; } = 1;

    private int _pageLength = 20;
    public int PageLength
    {
        get => _pageLength;
        set => _pageLength = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
    }

    public bool IsValid()
    {
        return Page > 0 && PageLength > 0;
    }
}
