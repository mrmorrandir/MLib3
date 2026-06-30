using MLib3.AspNetCore.Sample.Services;

namespace MLib3.AspNetCore.Sample.Endpoints;

public class AddGreetingEndpoint :IEndpoint
{
    public void Register(WebApplication app)
    {
        app.MapPost("/greetings", (GreetingsService greetingsService, string greeting) =>
        {
            var addedGreeting = greetingsService.AddGreeting(greeting);
            return Results.Ok(new { Greeting = addedGreeting });
        });
    }
}