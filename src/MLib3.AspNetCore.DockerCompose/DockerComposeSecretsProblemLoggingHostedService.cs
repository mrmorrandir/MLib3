namespace MLib3.AspNetCore.DockerCompose;

internal sealed class DockerComposeSecretsProblemLoggingHostedService(
    IReadOnlyCollection<DockerComposeSecretLoadProblem> problems,
    ILogger<DockerComposeSecretsProblemLoggingHostedService> logger)
    : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var problem in problems)
        {
            logger.LogWarning(
                problem.Exception,
                "Docker Compose secret '{SecretFile}' at '{SecretFilePath}' could not be loaded as JSON configuration. {Reason}",
                problem.FileName,
                problem.FilePath,
                problem.Reason);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
