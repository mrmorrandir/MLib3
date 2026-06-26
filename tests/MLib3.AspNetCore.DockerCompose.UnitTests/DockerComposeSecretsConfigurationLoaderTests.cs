using AwesomeAssertions;
using Microsoft.Extensions.Configuration;
using MLib3.AspNetCore.DockerCompose;
using System.IO.Abstractions.TestingHelpers;

namespace MLib3.AspNetCore.DockerCompose.UnitTests;

public class DockerComposeSecretsConfigurationLoaderTests
{
    private const string SecretsPath = @"C:\run\secrets";

    [Fact]
    public void AddDockerComposeSecrets_WhenDirectoryDoesNotExist_ReturnsNoProblems()
    {
        var fileSystem = new MockFileSystem();
        var configurationBuilder = new ConfigurationBuilder();
        var sut = new DockerComposeSecretsConfigurationLoader(fileSystem);

        var problems = sut.AddDockerComposeSecrets(configurationBuilder, SecretsPath);
        var configuration = configurationBuilder.Build();

        problems.Should().BeEmpty();
        configuration["Settings:Value"].Should().BeNull();
    }

    [Fact]
    public void AddDockerComposeSecrets_LoadsValidJsonFilesInAlphabeticalOrder()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            [Path.Combine(SecretsPath, "02-settings.json")] = new("""{ "Settings": { "Value": "second" } }"""),
            [Path.Combine(SecretsPath, "01-settings.json")] = new("""{ "Settings": { "Value": "first" } }""")
        });
        var configurationBuilder = new ConfigurationBuilder();
        var sut = new DockerComposeSecretsConfigurationLoader(fileSystem);

        var problems = sut.AddDockerComposeSecrets(configurationBuilder, SecretsPath);
        var configuration = configurationBuilder.Build();

        problems.Should().BeEmpty();
        configuration["Settings:Value"].Should().Be("second");
    }

    [Fact]
    public void AddDockerComposeSecrets_WhenJsonFileIsInvalid_ReturnsProblemAndSkipsFile()
    {
        var filePath = Path.Combine(SecretsPath, "settings.json");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            [filePath] = new("""{ "Settings": """)
        });
        var configurationBuilder = new ConfigurationBuilder();
        var sut = new DockerComposeSecretsConfigurationLoader(fileSystem);

        var problems = sut.AddDockerComposeSecrets(configurationBuilder, SecretsPath);
        var configuration = configurationBuilder.Build();

        problems.Should().ContainSingle()
            .Which.Should().Match<DockerComposeSecretLoadProblem>(problem =>
                problem.FileName == "settings.json"
                && problem.FilePath == filePath
                && problem.Reason.Contains("Line:"));
        configuration["Settings:Value"].Should().BeNull();
    }

    [Fact]
    public void AddDockerComposeSecrets_WhenPlainTextSecretIsInvalidJson_DoesNotReturnProblem()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            [Path.Combine(SecretsPath, "password")] = new("plain-secret-value")
        });
        var configurationBuilder = new ConfigurationBuilder();
        var sut = new DockerComposeSecretsConfigurationLoader(fileSystem);

        var problems = sut.AddDockerComposeSecrets(configurationBuilder, SecretsPath);

        problems.Should().BeEmpty();
    }

    [Fact]
    public void AddDockerComposeSecrets_WhenExtensionlessSecretLooksLikeJson_ReturnsProblem()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            [Path.Combine(SecretsPath, "settings")] = new(""" { "Settings": """)
        });
        var configurationBuilder = new ConfigurationBuilder();
        var sut = new DockerComposeSecretsConfigurationLoader(fileSystem);

        var problems = sut.AddDockerComposeSecrets(configurationBuilder, SecretsPath);

        problems.Should().ContainSingle()
            .Which.FileName.Should().Be("settings");
    }
}
