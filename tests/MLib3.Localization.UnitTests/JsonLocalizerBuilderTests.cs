using Microsoft.Extensions.DependencyInjection;
using MLib3.Localization.Interfaces;
using MLib3.Localization.Localizers.Json;
using MLib3.Localization.Localizers.Resx;

namespace MLib3.Localization.UnitTests;

public class JsonLocalizerBuilderTests
{
    [Fact]
    public void Build_ShouldReturnLocalizer_WhenConfigurationValid()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        var jsonLocalizerFactory = new JsonLocalizerFactory(serviceProvider);
        var builder = new JsonLocalizerBuilder(jsonLocalizerFactory);

        var localizer = builder.Build();

        localizer.Should().BeOfType<JsonLocalizer>();
    }
}

public class DependencyInjectionTests
{
    [Fact]
    public void UseJson_ShouldRegisterJsonLocalizer()
    {
        var services = new ServiceCollection();
        services.AddCustomLocalizer(config => config.UseJson());
        var serviceProvider = services.BuildServiceProvider();
        
        var builder = serviceProvider.GetService<ILocalizer>();

        builder.Should().NotBeNull();
        builder.Should().BeOfType<JsonLocalizer>();
    }
    
    // Not implemented yet
    // ------------------------------------------------------------
    // [Fact]
    // public void UseResx_ShouldRegisterResxLocalizer()
    // {
    //     var services = new ServiceCollection();
    //     services.AddCustomLocalizer(config => config.UseResx());
    //     var serviceProvider = services.BuildServiceProvider();
    //     
    //     var builder = serviceProvider.GetService<ILocalizer>();
    //
    //     builder.Should().NotBeNull();
    //     builder.Should().BeOfType<ResxLocalizer>();
    // }
}