namespace MLib3.AspNetCore.Sample.Endpoints;

public class HealthEndpoint: IEndpoint
{
    public void Register(WebApplication app)
    {
        app.MapGet("/healthz", () => Results.Ok(new { Status = "Healthy" }));
    }
}