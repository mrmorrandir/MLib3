namespace MLib3.AspNetCore.DockerCompose;

public static class DependencyInjection
{
    public static IConfigurationManager AddDockerComposeSecrets(this IConfigurationManager configuration)
    {
        var directoryInfo = new DirectoryInfo("/run/secrets");
        if (!directoryInfo.Exists) 
            return configuration;
        
        directoryInfo.GetFiles().ToList().ForEach(file =>
        {
            configuration.AddJsonFile(file.FullName);
        });
        configuration.AddEnvironmentVariables();
        return configuration;
    }
}