namespace MLib3.AspNetCore.Application;

public interface IPagination
{
    int? Offset { get; init; }
    int? Limit { get; init; }
}