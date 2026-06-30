using Microsoft.Extensions.Logging;
using MLib3.AspNetCore.DockerCompose;
using NSubstitute;
using System.Text.Json;

namespace MLib3.AspNetCore.DockerCompose.UnitTests;

public class DockerComposeSecretsProblemLoggingHostedServiceTests
{
    [Fact]
    public async Task StartAsync_LogsProblemsAsWarnings()
    {
        var exception = new JsonException("Invalid JSON.");
        var problem = new DockerComposeSecretLoadProblem(
            "invalid.json",
            "/run/secrets/invalid.json",
            "Invalid JSON.",
            exception);
        var logger = Substitute.For<ILogger<DockerComposeSecretsProblemLoggingHostedService>>();
        var sut = new DockerComposeSecretsProblemLoggingHostedService([problem], logger);

        await sut.StartAsync(CancellationToken.None);

        logger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                state.ToString()!.Contains("invalid.json")
                && state.ToString()!.Contains("/run/secrets/invalid.json")
                && state.ToString()!.Contains("Invalid JSON.")),
            exception,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task StopAsync_CompletesSuccessfully()
    {
        var logger = Substitute.For<ILogger<DockerComposeSecretsProblemLoggingHostedService>>();
        var sut = new DockerComposeSecretsProblemLoggingHostedService([], logger);

        await sut.StopAsync(CancellationToken.None);
    }
}
