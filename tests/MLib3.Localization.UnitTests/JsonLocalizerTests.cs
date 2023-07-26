using System.Globalization;
using System.Text.Json;
using MLib3.Localization.Interfaces;
using MLib3.Localization.Localizers.Json;

namespace MLib3.Localization.UnitTests;

public class JsonLocalizerTests : LocalizerTests
{
    internal override ILocalizer Localizer
    {
        get
        {
            var translations = JsonSerializer.Deserialize<TranslationDoc>(File.ReadAllText("./Translations/Test.json"));
            return new JsonLocalizer(translations!, new CultureInfo("en-US"));
        }
    }
}