using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;

namespace MLib3.AspNetCore.Application.UnitTests;

public class QueryableExtensionsTests
{
    [Fact]
    public async Task ToPaginatedListAsync_ShouldUseExplicitOffsetAndLimit_WhenValuesAreProvided()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        await SeedProductsAsync(dbContext);

        // Act
        var result = await dbContext.Products
            .OrderBy(product => product.Id)
            .ToPaginatedListAsync(offset: 2, limit: 2);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Offset.Should().Be(2);
        result.Value.Limit.Should().Be(2);
        result.Value.Total.Should().Be(5);
        result.Value.Items.Select(product => product.Name).Should().Equal("Third", "Fourth");
    }

    [Fact]
    public async Task ToPaginatedListAsync_ShouldUsePaginationValues_WhenPaginationIsProvided()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        await SeedProductsAsync(dbContext);
        var pagination = new TestPagination
        {
            Offset = 1,
            Limit = 3
        };

        // Act
        var result = await dbContext.Products
            .OrderBy(product => product.Id)
            .ToPaginatedListAsync(pagination);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Offset.Should().Be(1);
        result.Value.Limit.Should().Be(3);
        result.Value.Total.Should().Be(5);
        result.Value.Items.Select(product => product.Name).Should().Equal("Second", "Third", "Fourth");
    }

    private static TestDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestDbContext(options);
    }

    private static async Task SeedProductsAsync(TestDbContext dbContext)
    {
        dbContext.Products.AddRange(
            new Product { Id = 1, Name = "First" },
            new Product { Id = 2, Name = "Second" },
            new Product { Id = 3, Name = "Third" },
            new Product { Id = 4, Name = "Fourth" },
            new Product { Id = 5, Name = "Fifth" });

        await dbContext.SaveChangesAsync();
    }

    private sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products => Set<Product>();
    }

    private sealed class TestPagination : IPagination
    {
        public int? Offset { get; init; }
        public int? Limit { get; init; }
    }

    private sealed class Product
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
