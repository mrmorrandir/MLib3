using System.Globalization;
using MLib3.Localization.Interfaces;

namespace MLib3.Localization.Localizers.Resx;

public class ResxLocalizerBuilder : IResxLocalizerBuilder
{
    public IResxLocalizerConfig SetCulture(CultureInfo cultureInfo)
    {
        throw new NotImplementedException();
    }

    public IResxLocalizerConfig AddFile(string filename)
    {
        throw new NotImplementedException();
    }

    public ILocalizer Build()
    {
        throw new NotImplementedException();
    }
}