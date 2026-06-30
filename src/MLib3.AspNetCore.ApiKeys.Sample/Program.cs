using MLib3.AspNetCore.ApiKeys;
using MLib3.AspNetCore.DockerCompose;

var builder = WebApplication.CreateBuilder(args);
builder.AddDockerComposeSecrets();
builder.Services
        .AddAuthentication()
        .AddApiKeyAuthentication(storeOptions =>
        {
                storeOptions.AddHashedKey("BackendA", "", "orders:read");
                storeOptions.AddHashedKey("BackendB", "", "orders:read", "orders:write");
        });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
