using AwesomeAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MLib3.AspNetCore.UnitTests;

public class ApiLoggingDependencyInjectionTests
{
    [Fact]
    public void AddApiLoggingMiddleware_RegistersMiddlewareAndService()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
        services.AddLogging();

        services.AddApiLoggingMiddleware();

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        scope.ServiceProvider.GetRequiredService<IApiLoggingService>().Should().NotBeNull();
        scope.ServiceProvider.GetRequiredService<ApiLoggingMiddleware>().Should().NotBeNull();
    }

    [Fact]
    public void AddApiLoggingMiddleware_WithOptionsBuilder_ConfiguresExcludedPathsAndFiles()
    {
        var services = new ServiceCollection();
        services.AddLogging();

        services.AddApiLoggingMiddleware(options =>
        {
            options.WithExcludePaths("/health", "/swagger");
            options.WithExcludedFiles("/assets/*.js", "/favicon.ico");
        });

        using var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<ApiLoggingOptions>>().Value;

        options.ExcludedPaths.Should().Equal("/health", "/swagger");
        options.ExcludedFiles.Should().Equal("/assets/*.js", "/favicon.ico");
    }

    [Fact]
    public void AddApiLogHandler_RegistersHandlerAsApiLogHandler()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddApiLoggingMiddleware();

        services.AddApiLogHandler<TestApiLogHandler>();

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        scope.ServiceProvider.GetServices<IApiLogHandler>()
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .BeOfType<TestApiLogHandler>();
    }

    [Fact]
    public void AddApiLogHandler_WithLifetime_RegistersHandlerWithRequestedLifetime()
    {
        var services = new ServiceCollection();

        services.AddApiLogHandler<TestApiLogHandler>(ServiceLifetime.Singleton);

        services.Should().ContainSingle(descriptor =>
            descriptor.ServiceType == typeof(IApiLogHandler)
            && descriptor.ImplementationType == typeof(TestApiLogHandler)
            && descriptor.Lifetime == ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddApiLogHandler_WithFactory_RegistersFactoryHandler()
    {
        var services = new ServiceCollection();

        services.AddApiLogHandler(_ => new FactoryApiLogHandler("Factory"), ServiceLifetime.Transient);

        services.Should().ContainSingle(descriptor =>
            descriptor.ServiceType == typeof(IApiLogHandler)
            && descriptor.ImplementationFactory != null
            && descriptor.Lifetime == ServiceLifetime.Transient);

        using var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetRequiredService<IApiLogHandler>()
            .Should()
            .BeEquivalentTo(new FactoryApiLogHandler("Factory"));
    }

    private sealed class TestApiLogHandler : IApiLogHandler
    {
        public Task HandleAsync(ApiLog log, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }

    private sealed record FactoryApiLogHandler(string Name) : IApiLogHandler
    {
        public Task HandleAsync(ApiLog log, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }
}
