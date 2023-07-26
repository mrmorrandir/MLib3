using System.Globalization;
using System.Text.Json;
using MLib3.Localization.Interfaces;

namespace MLib3.Localization.Localizers.Json;

public class JsonLocalizerBuilder : IJsonLocalizerBuilder
{
    private readonly IJsonLocalizerFactory _jsonLocalizerFactory;
    private readonly List<string> _filenames = new();
    private CultureInfo _cultureInfo = new("en-US");

    public JsonLocalizerBuilder(IJsonLocalizerFactory jsonLocalizerFactory)
    {
        _jsonLocalizerFactory = jsonLocalizerFactory;
    }
    
    public IJsonLocalizerConfig SetCulture(CultureInfo cultureInfo)
    {
        _cultureInfo = cultureInfo;
        return this;
    }

    public IJsonLocalizerConfig AddFile(string filename)
    {
        _filenames.Add(filename);
        return this;
    }

    public ILocalizer Build()
    {
        var translationDoc = new TranslationDoc();
        var nonExistingFiles = _filenames.Where(filename => !new FileInfo(filename).Exists).ToArray();
        if (nonExistingFiles.Any())
        {
            var filenameList = string.Join(", ", nonExistingFiles.ToArray());
            throw new Exception($"Translation files {filenameList} do not exist.");
        }

        foreach(var filename in _filenames) 
        {
            try
            {
                var singleTranslationDoc = JsonSerializer.Deserialize<TranslationDoc>(filename);
                translationDoc.Cultures.AddRange(singleTranslationDoc.Cultures.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading {filename} for translation {ex.Message}.", ex);
            }
        }
        return _jsonLocalizerFactory.Create(translationDoc, _cultureInfo);
    }
}