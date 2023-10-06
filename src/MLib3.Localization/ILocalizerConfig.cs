using System.Globalization;

namespace MLib3.Localization;

public interface ILocalizerConfig<out T>
{
    public T SetCulture(CultureInfo cultureInfo);
}