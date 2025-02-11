using System.Text.Json;

namespace MLib3.AspNetCore.DockerCompose;

public static class DependencyInjection
{
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
    public static IConfigurationManager AddDockerComposeSecrets(this IConfigurationManager configuration)
    {
        var directoryInfo = new DirectoryInfo("/run/secrets");
        if (!directoryInfo.Exists)
            return configuration;

        var secretFiles = directoryInfo.GetFiles().OrderBy(x => x.Name).ToList();
        foreach (var file in secretFiles)
        {
            try
            {
                using var stream = file.OpenRead();
                using var doc = JsonDocument.Parse(stream);
            }
            catch (JsonException)
            {
                continue;
            }

            configuration.AddJsonFile(file.FullName);
        }

        configuration.AddEnvironmentVariables();
        return configuration;
    }
}