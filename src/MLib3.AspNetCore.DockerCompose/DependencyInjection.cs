using System.IO.Abstractions;

namespace MLib3.AspNetCore.DockerCompose;

public static class DependencyInjection
{
    private const string _defaultSecretsPath = "/run/secrets";
    
    /// <summary>
    ///     Adds Docker Compose secrets to the configuration manager.
    /// </summary>
    /// <param name="configuration">The configuration manager to which the Docker Compose secrets will be added.</param>
    /// <returns>The updated configuration manager with Docker Compose secrets and environment variables added.</returns>
    /// <remarks>
    ///     This method looks for Docker Compose secrets in the `/run/secrets` directory. If the directory exists,
    ///     it reads all files in the directory and adds them as JSON configuration files to the configuration manager.
    ///     Additionally, it adds environment variables to the configuration manager to overwrite settings if needed.
    /// </remarks>
    [Obsolete("Use AddDockerComposeSecrets(this WebApplicationBuilder builder) instead.")]
    public static IConfigurationManager AddDockerComposeSecrets(this IConfigurationManager configuration)
    {
        configuration.AddDockerComposeSecrets(_defaultSecretsPath);
        return configuration;
    }

    /// <summary>
    ///     Adds Docker Compose secrets to the application builder and logs skipped secret files during application startup.
    /// </summary>
    /// <param name="builder">The application builder to which the Docker Compose secrets will be added.</param>
    /// <param name="secretsPath">The directory in which Docker Compose stores mounted secrets.</param>
    /// <returns>The updated application builder with Docker Compose secrets and environment variables added.</returns>
    public static WebApplicationBuilder AddDockerComposeSecrets(
        this WebApplicationBuilder builder,
        string secretsPath = _defaultSecretsPath)
    {
        var problems = new List<DockerComposeSecretLoadProblem>();

        builder.Configuration.AddDockerComposeSecrets(secretsPath, problems.Add);
        builder.Services.AddDockerComposeSecretsProblemLogging(problems);

        return builder;
    }

    /// <summary>
    ///     Adds Docker Compose secrets to the host application builder and logs skipped secret files during application startup.
    /// </summary>
    /// <param name="builder">The host application builder to which the Docker Compose secrets will be added.</param>
    /// <param name="secretsPath">The directory in which Docker Compose stores mounted secrets.</param>
    /// <returns>The updated host application builder with Docker Compose secrets and environment variables added.</returns>
    public static IHostApplicationBuilder AddDockerComposeSecrets(
        this IHostApplicationBuilder builder,
        string secretsPath = _defaultSecretsPath)
    {
        var problems = new List<DockerComposeSecretLoadProblem>();

        builder.Configuration.AddDockerComposeSecrets(secretsPath, problems.Add);
        builder.Services.AddDockerComposeSecretsProblemLogging(problems);

        return builder;
    }


    /// <summary>
    ///     Adds Docker Compose secrets to the configuration builder.
    /// </summary>
    /// <param name="configuration">The configuration builder to which the Docker Compose secrets will be added.</param>
    /// <param name="secretsPath">The directory in which Docker Compose stores mounted secrets.</param>
    /// <param name="logProblem">An optional callback that receives files which could not be loaded as JSON configuration.</param>
    /// <returns>The updated configuration builder with Docker Compose secrets and environment variables added.</returns>
    /// <remarks>
    ///     This method looks for Docker Compose secrets in the configured secrets directory. If the directory exists,
    ///     it reads all files in the directory and adds valid JSON files as configuration files to the configuration builder.
    ///     Files that look like JSON but are not valid JSON are skipped and reported through <paramref name="logProblem"/>.
    ///     Additionally, it adds environment variables to the configuration builder to overwrite settings if needed.
    /// </remarks>
    public static IConfigurationBuilder AddDockerComposeSecrets(
        this IConfigurationBuilder configuration,
        string secretsPath = _defaultSecretsPath,
        Action<DockerComposeSecretLoadProblem>? logProblem = null)
    {
        var loader = new DockerComposeSecretsConfigurationLoader(new FileSystem());
        var problems = loader.AddDockerComposeSecrets(configuration, secretsPath);

        foreach (var problem in problems)
            logProblem?.Invoke(problem);

        configuration.AddEnvironmentVariables();
        return configuration;
    }

    private static void AddDockerComposeSecretsProblemLogging(
        this IServiceCollection services,
        IReadOnlyCollection<DockerComposeSecretLoadProblem> problems)
    {
        if (problems.Count == 0)
            return;

        services.AddHostedService(serviceProvider =>
            new DockerComposeSecretsProblemLoggingHostedService(
                problems,
                serviceProvider.GetRequiredService<ILogger<DockerComposeSecretsProblemLoggingHostedService>>()));
    }
}
