using MLib3.Localization.Interfaces;

namespace MLib3.Localization;

public interface ICustomLocalizerBuilder : ICustomLocalizerConfig
{
    ILocalizer Build();
}