using System.Globalization;
using MLib3.Localization.Interfaces;
using MLib3.Localization.Localizers.Resx;

namespace MLib3.Localization.UnitTests;

public class ResxLocalizerTests: LocalizerTests
{
    internal override ILocalizer Localizer => new ResxLocalizer("MLib3.Localization.UnitTests.Translations.Test", new CultureInfo("en-US"));
}