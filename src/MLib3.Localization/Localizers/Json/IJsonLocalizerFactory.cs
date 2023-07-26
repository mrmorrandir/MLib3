using System.Globalization;
using MLib3.Localization.Interfaces;

namespace MLib3.Localization.Localizers.Json;

public interface IJsonLocalizerFactory
{
    ILocalizer Create(TranslationDoc translationDoc, CultureInfo cultureInfo);
}