using System.Globalization;
using MLib3.Localization.Interfaces;

namespace MLib3.Localization.UnitTests;

public abstract class LocalizerTests
{
    internal abstract ILocalizer Localizer { get; }
    
    [Fact]
    public void T_ShouldNotThrowException_WhenCalledWithValidKey()
    {
        var func = () => Localizer.T("error-1");
        
        func.Should().NotThrow();
    }
    
    [Fact]
    public void T_ShouldNotThrowException_WhenCalledWithInvalidKey()
    {
        var func = () => Localizer.T("xyz");

        func.Should().NotThrow();
    }
    
    [Fact]
    public void T_ShouldReturnKey_WhenCalledWithInvalidKey()
    {
        var result = Localizer.T("xyz");
        
        result.Should().Be("[xyz]");
    }
    
    [Fact]
    public void T_ShouldReturnTranslation_WhenCalledWithValidKey()
    {
        var result = Localizer.T("error-1");
        
        result.Should().Be("Error 1");
    }
    
    [Fact]
    public void T_ShouldReturnTranslation_WhenCalledWithValidKeyAndArguments()
    {
        var result = Localizer.T("error-2", "arg1", "arg2");
        
        result.Should().Be("Error 2: arg1, arg2");
    }

    [Fact]
    public void T_ShouldReturnTranslation_WhenCalledWithValidKeyAndArgumentsAndCulture()
    {
        var result = Localizer.T("error-2", CultureInfo.GetCultureInfo("de-DE"), "arg1", "arg2");

        result.Should().Be("Fehler 2: arg1, arg2");
    }
    
    [Fact]
    public void Indexer_ShouldNotThrowException_WhenCalledWithValidKey()
    {
        var func = () => Localizer["error-1"];
        
        func.Should().NotThrow();
    }
    
    [Fact]
    public void Indexer_ShouldNotThrowException_WhenCalledWithInvalidKey()
    {
        var func = () => Localizer["xyz"];

        func.Should().NotThrow();
    }
    
    [Fact]
    public void Indexer_ShouldReturnKey_WhenCalledWithInvalidKey()
    {
        var result = Localizer["xyz"];
        
        result.Should().Be("[xyz]");
    }
    
    [Fact]
    public void Indexer_ShouldReturnTranslation_WhenCalledWithValidKey()
    {
        var result = Localizer["error-1"];
        
        result.Should().Be("Error 1");
    }
    
    [Fact]
    public void Indexer_ShouldReturnTranslation_WhenCalledWithValidKeyAndArguments()
    {
        var result = Localizer["error-2", "arg1", "arg2"];
        
        result.Should().Be("Error 2: arg1, arg2");
    }

    [Fact]
    public void Indexer_ShouldReturnTranslation_WhenCalledWithValidKeyAndArgumentsAndCulture()
    {
        var result = Localizer["error-2", CultureInfo.GetCultureInfo("de-DE"), "arg1", "arg2"];

        result.Should().Be("Fehler 2: arg1, arg2");
    }
}