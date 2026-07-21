using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;

namespace MLib3.AspNetCore.Application.UnitTests;

public class PaginatedListTests
{
    [Fact]
    public async Task CreateAsync_ShouldReturnAllItems_WhenOffsetAndLimitAreNull()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        dbContext.Products.AddRange(
            new Product { Id = 1, Name = "First" },
            new Product { Id = 2, Name = "Second" },
            new Product { Id = 3, Name = "Third" });
        await dbContext.SaveChangesAsync();

        // Act
        var result = await PaginatedList<Product>.CreateAsync(
            dbContext.Products.OrderBy(product => product.Id));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Offset.Should().BeNull();
        result.Value.Limit.Should().BeNull();
        result.Value.Total.Should().Be(3);
        result.Value.Items.Select(product => product.Name).Should().Equal("First", "Second", "Third");
    }

    [Fact]
    public async Task CreateAsync_ShouldApplyOffsetAndLimit_WhenValuesAreProvided()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        dbContext.Products.AddRange(
            new Product { Id = 1, Name = "First" },
            new Product { Id = 2, Name = "Second" },
            new Product { Id = 3, Name = "Third" },
            new Product { Id = 4, Name = "Fourth" });
        await dbContext.SaveChangesAsync();

        // Act
        var result = await PaginatedList<Product>.CreateAsync(
            dbContext.Products.OrderBy(product => product.Id),
            offset: 1,
            limit: 2);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Offset.Should().Be(1);
        result.Value.Limit.Should().Be(2);
        result.Value.Total.Should().Be(4);
        result.Value.Items.Select(product => product.Name).Should().Equal("Second", "Third");
    }

    private static TestDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestDbContext(options);
    }

    private sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products => Set<Product>();
    }

    private sealed class Product
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
