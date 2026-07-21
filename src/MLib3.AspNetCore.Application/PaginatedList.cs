using Microsoft.EntityFrameworkCore;

namespace MLib3.AspNetCore.Application;

public class PaginatedList<T>
{
    public int? Offset { get; }
    public int? Limit { get; }
    public int Total { get; }
    public List<T> Items { get; }

    public PaginatedList(List<T> items, int? offset, int? limit, int total)
    {
        Items = items;
        Offset = offset;
        Limit = limit;
        Total = total;
    }

    public static async Task<Result<PaginatedList<T>>> CreateAsync(IQueryable<T> source, int? offset = null, int? limit = null, CancellationToken cancellationToken = default)
    {
        var total = await source.CountAsync(cancellationToken);
        var itemsResult = await Result.Try(() => source.Skip(offset ?? 0).Take(limit ?? total).ToListAsync(cancellationToken));
        if (itemsResult.IsFailed)
            return new Error("Failed to get paginated list").CausedBy(itemsResult.Errors);
        return new PaginatedList<T>(itemsResult.Value, offset, limit, total);
    }
}