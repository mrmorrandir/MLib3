namespace MLib3.AspNetCore.Application;

public static class QueryableExtensions
{
    public static Task<Result<PaginatedList<T>>> ToPaginatedListAsync<T>(this IQueryable<T> queryable, int? offset = null, int? limit = null, CancellationToken cancellationToken = default) => PaginatedList<T>.CreateAsync(queryable, offset, limit, cancellationToken);
    
    public static Task<Result<PaginatedList<T>>> ToPaginatedListAsync<T>(this IQueryable<T> queryable, IPagination pagination, CancellationToken cancellationToken = default) => PaginatedList<T>.CreateAsync(queryable, pagination.Offset, pagination.Limit, cancellationToken);
}