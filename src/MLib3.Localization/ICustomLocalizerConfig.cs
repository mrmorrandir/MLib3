namespace MLib3.Localization;

public interface ICustomLocalizerConfig
{
    IJsonLocalizerConfig UseJson();
    IResxLocalizerConfig UseResx();
   
}