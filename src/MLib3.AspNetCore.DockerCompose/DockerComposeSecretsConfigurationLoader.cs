using System.IO.Abstractions;
using System.Text.Json;

namespace MLib3.AspNetCore.DockerCompose;

public sealed class DockerComposeSecretsConfigurationLoader(IFileSystem fileSystem)
{
    public IReadOnlyCollection<DockerComposeSecretLoadProblem> AddDockerComposeSecrets(
        IConfigurationBuilder configuration,
        string secretsPath)
    {
        var problems = new List<DockerComposeSecretLoadProblem>();

        if (!fileSystem.Directory.Exists(secretsPath))
            return problems;

        var secretFiles = fileSystem.DirectoryInfo
            .New(secretsPath)
            .GetFiles()
            .OrderBy(x => x.Name)
            .ToList();

        foreach (var file in secretFiles)
        {
            // Create a MemoryStream to hold the configuration data
            // and parse the JSON file into a JsonDocument
            // as well as use it later to AddJsonStream
            // (by using AddJsonFile tests with MockFileSystem would fail,
            // because AddJsonFile always uses the real file system)
            MemoryStream? configurationStream = null;
            try
            {
                using var stream = file.OpenRead();
                configurationStream = new MemoryStream();
                stream.CopyTo(configurationStream);
                configurationStream.Position = 0;

                using var doc = JsonDocument.Parse(configurationStream);
                configurationStream.Position = 0;
            }
            catch (JsonException exception)
            {
                configurationStream?.Dispose();

                if (ShouldReportInvalidJsonSecret(file))
                    problems.Add(DockerComposeSecretLoadProblem.FromJsonException(file, exception));

                continue;
            }
            
            configuration.AddJsonStream(configurationStream);
        }

        return problems;
    }

    private static bool ShouldReportInvalidJsonSecret(IFileInfo file)
    {
        if (file.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
            return true;

        using var stream = file.OpenRead();
        int current;
        do
        {
            current = stream.ReadByte();
        } while (current is ' ' or '\t' or '\r' or '\n');

        return current is '{' or '[';
    }
}
