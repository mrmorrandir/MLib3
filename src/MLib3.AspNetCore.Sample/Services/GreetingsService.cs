namespace MLib3.AspNetCore.Sample.Services;

public class GreetingsService
{
    private static List<string> _greetings = new List<string>
    {
        "Hello, World!",
        "Hello, Universe!",
        "Hi there!",
        "Hola, Mundo!",
        "Bonjour le monde!",
        "Ciao mondo!"
    };

    public string GetRandomGreeting()
    {
        return _greetings[new Random().Next(_greetings.Count)];
    }
    
    public string AddGreeting(string greeting)
    {
        _greetings.Add(greeting);
        return greeting;
    }
}