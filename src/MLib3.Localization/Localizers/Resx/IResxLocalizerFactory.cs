using System.Globalization;
using System.Reflection;
using MLib3.Localization.Interfaces;

namespace MLib3.Localization.Localizers.Resx;

public interface IResxLocalizerFactory
{
    ILocalizer Create(string resource, CultureInfo cultureInfo, Assembly? assembly = null);
}