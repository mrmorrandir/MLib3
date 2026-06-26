using System.IO.Abstractions;
using System.Text.Json;

namespace MLib3.AspNetCore.DockerCompose;

public sealed record DockerComposeSecretLoadProblem(
    string FileName,
    string FilePath,
    string Reason,
    JsonException Exception)
{
    internal static DockerComposeSecretLoadProblem FromJsonException(IFileInfo file, JsonException exception)
    {
        var location = exception.LineNumber is not null || exception.BytePositionInLine is not null
            ? $" Line: {exception.LineNumber?.ToString() ?? "unknown"}, byte position in line: {exception.BytePositionInLine?.ToString() ?? "unknown"}."
            : string.Empty;

        return new DockerComposeSecretLoadProblem(
            file.Name,
            file.FullName,
            $"{exception.Message}{location}",
            exception);
    }
}
