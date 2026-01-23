namespace ERP.Shared.Application.Paging;

public sealed class AppPagedResult<T>
{
    public IEnumerable<T> Items { get; init; } = [];
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }

    public static AppPagedResult<T> Create(IEnumerable<T> items, int pageNumber, int pageSize, int totalItems)
    {
        var totalPages = pageSize == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);

        return new AppPagedResult<T>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < totalPages
        };
    }
}




