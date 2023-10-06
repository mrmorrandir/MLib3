namespace MLib3.Localization.Localizers.Json;

public class TranslationCulture 
{
    public string Name {get; init;}
    public List<TranslationEntry> Strings {get; init;} = new();
}