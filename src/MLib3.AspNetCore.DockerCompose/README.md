# MLib3.AspNetCore.DockerCompose

Extensions for ASP.NET Core applications that load Docker Compose secrets as JSON configuration files.

Docker Compose mounts secrets as files, commonly below `/run/secrets`. This package reads the files from that directory, validates that they contain valid JSON, adds valid files to the application configuration, and then adds environment variables so they can override loaded values.

## Installation

```shell
dotnet add package MLib3.AspNetCore.DockerCompose
```

## Usage With `WebApplicationBuilder`

For ASP.NET Core applications, call `AddDockerComposeSecrets` during startup before building the application.

```csharp
using MLib3.AspNetCore.DockerCompose;

var builder = WebApplication.CreateBuilder(args);

builder.AddDockerComposeSecrets();

var app = builder.Build();

app.MapGet("/", (IConfiguration configuration) =>
{
    var value = configuration["MySettings:Value"];
    return Results.Ok(value);
});

app.Run();
```

By default, secrets are loaded from `/run/secrets`. A custom path can be passed when needed:

```csharp
builder.AddDockerComposeSecrets("/custom/secrets/path");
```

This overload also registers startup logging for files that looked like JSON configuration but could not be parsed.

## Usage With `IHostApplicationBuilder`

For worker services or generic host applications, use the `IHostApplicationBuilder` overload.

```csharp
using MLib3.AspNetCore.DockerCompose;

var builder = Host.CreateApplicationBuilder(args);

builder.AddDockerComposeSecrets();

builder.Services.AddHostedService<Worker>();

var app = builder.Build();
app.Run();
```

A custom secrets directory can be provided as well:

```csharp
builder.AddDockerComposeSecrets("/custom/secrets/path");
```

## Usage With `IConfigurationBuilder`

If you configure the configuration pipeline directly, call the `IConfigurationBuilder` overload.

```csharp
using MLib3.AspNetCore.DockerCompose;

var configuration = new ConfigurationBuilder()
    .AddDockerComposeSecrets()
    .Build();
```

You can also receive skipped JSON files through a callback:

```csharp
var configuration = new ConfigurationBuilder()
    .AddDockerComposeSecrets(
        secretsPath: "/run/secrets",
        logProblem: problem =>
        {
            Console.WriteLine(
                $"Secret '{problem.FileName}' could not be loaded: {problem.Reason}");
        })
    .Build();
```

## Secret File Format

Each secret file that should become configuration must contain valid JSON.

```json
{
  "ConnectionStrings": {
    "Default": "Host=db;Database=app;Username=app;Password=secret"
  },
  "MySettings": {
    "Value": "example"
  }
}
```

Files are loaded in alphabetical order by file name. Environment variables are added after the secret files, so environment variables can override values from Docker Compose secrets.

## Startup Warnings

When the `WebApplicationBuilder` or `IHostApplicationBuilder` overload is used, invalid JSON files are skipped and logged as warnings during application startup.

If loaded settings are missing or unexpected, check these warnings first. They usually indicate that a mounted secret file could not be parsed, for example because a JSON file was accidentally formatted incorrectly.
