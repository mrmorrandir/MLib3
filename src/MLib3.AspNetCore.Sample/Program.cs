using MLib3.AspNetCore.DockerCompose;
using MLib3.AspNetCore.Sample.Services;

var builder = WebApplication.CreateBuilder(args);

// From MLib3.AspNetCore.DockerCompose
builder.AddDockerComposeSecrets();

// From MLib3.AspNetCore.Logging
builder.Services.AddApiLoggingMiddleware();

// From MLib3.AspNetCore
builder.Services.AddEndpoints();

// Demo-specific services
builder.Services.AddSingleton<GreetingsService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// From MLib3.AspNetCore.Logging
app.UseApiLoggingMiddleware();

// From MLib3.AspNetCore
app.UseEndpoints();

// Demo-specific
app.UseSwagger();
app.UseSwaggerUI();

app.Run();