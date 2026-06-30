using MLib3.AspNetCore.Sample.Services;

namespace MLib3.AspNetCore.Sample.Endpoints;

public class GetGreetingEndpoint : IEndpoint
{
    public void Register(WebApplication app)
    {
        app.MapGet("/greetings", (GreetingsService greetingsService) => Results.Ok(new { Greeting = greetingsService.GetRandomGreeting() }));
    }
}