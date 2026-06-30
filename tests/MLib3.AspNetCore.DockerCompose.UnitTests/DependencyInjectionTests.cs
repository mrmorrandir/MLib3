using AwesomeAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MLib3.AspNetCore.DockerCompose;

namespace MLib3.AspNetCore.DockerCompose.UnitTests;

public class DependencyInjectionTests : IDisposable
{
    private readonly string _tempPath = Path.Combine(
        Path.GetTempPath(),
        "MLib3.AspNetCore.DockerCompose.UnitTests",
        Guid.NewGuid().ToString("N"));

    [Fact]
    public void AddDockerComposeSecrets_WithConfigurationBuilder_LoadsJsonAndReportsProblems()
    {
        Directory.CreateDirectory(_tempPath);
        File.WriteAllText(
            Path.Combine(_tempPath, "settings.json"),
            """{ "Settings": { "Value": "from-secret" } }""");
        File.WriteAllText(
            Path.Combine(_tempPath, "invalid.json"),
            """{ "Broken": """);
        var problems = new List<DockerComposeSecretLoadProblem>();
        var configurationBuilder = new ConfigurationBuilder();

        var result = configurationBuilder.AddDockerComposeSecrets(_tempPath, problems.Add);
        var configuration = configurationBuilder.Build();

        result.Should().BeSameAs(configurationBuilder);
        configuration["Settings:Value"].Should().Be("from-secret");
        problems.Should().ContainSingle()
            .Which.FileName.Should().Be("invalid.json");
    }

    [Fact]
    public void AddDockerComposeSecrets_WithWebApplicationBuilder_WhenProblemsExist_RegistersHostedService()
    {
        Directory.CreateDirectory(_tempPath);
        File.WriteAllText(
            Path.Combine(_tempPath, "invalid.json"),
            """{ "Broken": """);
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = []
        });
        var hostedServicesBefore = CountHostedServices(builder.Services);

        var result = builder.AddDockerComposeSecrets(_tempPath);

        result.Should().BeSameAs(builder);
        CountHostedServices(builder.Services).Should().Be(hostedServicesBefore + 1);
    }

    [Fact]
    public void AddDockerComposeSecrets_WithWebApplicationBuilder_WhenNoProblemsExist_DoesNotRegisterHostedService()
    {
        Directory.CreateDirectory(_tempPath);
        File.WriteAllText(
            Path.Combine(_tempPath, "settings.json"),
            """{ "Settings": { "Value": "from-secret" } }""");
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = []
        });
        var hostedServicesBefore = CountHostedServices(builder.Services);

        var result = builder.AddDockerComposeSecrets(_tempPath);

        result.Should().BeSameAs(builder);
        CountHostedServices(builder.Services).Should().Be(hostedServicesBefore);
        builder.Configuration["Settings:Value"].Should().Be("from-secret");
    }

    [Fact]
    public void AddDockerComposeSecrets_WithHostApplicationBuilder_WhenProblemsExist_RegistersHostedService()
    {
        Directory.CreateDirectory(_tempPath);
        File.WriteAllText(
            Path.Combine(_tempPath, "invalid.json"),
            """{ "Broken": """);
        var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
        {
            Args = [],
            DisableDefaults = true
        });
        var hostedServicesBefore = CountHostedServices(builder.Services);

        var result = builder.AddDockerComposeSecrets(_tempPath);

        result.Should().BeSameAs(builder);
        CountHostedServices(builder.Services).Should().Be(hostedServicesBefore + 1);
    }

    [Fact]
    public void AddDockerComposeSecrets_WithHostApplicationBuilder_WhenNoProblemsExist_DoesNotRegisterHostedService()
    {
        Directory.CreateDirectory(_tempPath);
        File.WriteAllText(
            Path.Combine(_tempPath, "settings.json"),
            """{ "Settings": { "Value": "from-secret" } }""");
        var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
        {
            Args = [],
            DisableDefaults = true
        });
        var hostedServicesBefore = CountHostedServices(builder.Services);

        var result = builder.AddDockerComposeSecrets(_tempPath);

        result.Should().BeSameAs(builder);
        CountHostedServices(builder.Services).Should().Be(hostedServicesBefore);
        builder.Configuration["Settings:Value"].Should().Be("from-secret");
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempPath))
            Directory.Delete(_tempPath, recursive: true);
    }

    private static int CountHostedServices(IServiceCollection services) =>
        services.Count(x => x.ServiceType == typeof(IHostedService));
}
