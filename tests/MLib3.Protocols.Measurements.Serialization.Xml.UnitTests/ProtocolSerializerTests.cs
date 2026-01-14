using Microsoft.Extensions.DependencyInjection;
using MLib3.Protocols.Measurements.Serialization.Xml.UnitTests.Common;
using MLib3.Protocols.Measurements.TestCore;

namespace MLib3.Protocols.Measurements.Serialization.Xml.UnitTests;

public class ProtocolSerializerTests
{
    
    [Fact]
    public void Serialize_ShouldSucceed()
    {
        var serviceProvider = Setup(config => config.UseV3());
        var protocolSerializer = serviceProvider.GetRequiredService<IProtocolSerializer>();
        var protocol = DummyProtocolGenerator.Generate();
        
        var result = protocolSerializer.Serialize(protocol);
        
        result.IsSuccess.Should().BeTrue();
        var validationResults = XmlSchemaValidatorV3.Validate(result.Value);
        validationResults.Should().BeSuccess();
    }

    [Fact]
    public void Serialize_ShouldNotIncludeNullMinNomMax()
    {
        // Arrange
        var serviceProvider = Setup(config => config.UseV3());
        var protocolSerializer = serviceProvider.GetRequiredService<IProtocolSerializer>();
        
        var protocol = new Protocol
        {
            Product = new Product(),
            Meta = new Meta { Timestamp = DateTime.UtcNow },
            Results = new Results
            {
                Data =
                [
                    new ValueSetting
                    {
                        Name = "TestValueSetting",
                        Unit = "V",
                        Precision = 0.0,
                        Min = null,
                        Nom = null,
                        Max = null
                    },
                    new Value
                    {
                        Name = "TestValue",
                        Unit = "A",
                        Precision = 0.0,
                        Result = 1.5,
                        Min = null,
                        Nom = null,
                        Max = null,
                        Ok = true
                    }
                ]
            }
        };

        // Act
        var result = protocolSerializer.Serialize(protocol);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotContain("<Min");
        result.Value.Should().NotContain("<Nom");
        result.Value.Should().NotContain("<Max");
    }

    [Fact]
    public void Serialize_ShouldSucceed_ForV2()
    {
        var serviceProvider = Setup(config => config.UseV2());
        var protocolSerializer = serviceProvider.GetRequiredService<IProtocolSerializer>();
        var protocol = DummyProtocolGenerator.Generate();
        
        var result = protocolSerializer.Serialize(protocol);
        
        result.IsSuccess.Should().BeTrue();
        var validationResults = XmlSchemaValidatorV2.Validate(result.Value);
        validationResults.Should().BeSuccess();
    }

    [Fact]
    public void Serialize_ShouldNotIncludeNullMinNomMax_ForV2()
    {
        // Arrange
        var serviceProvider = Setup(config => config.UseV2());
        var protocolSerializer = serviceProvider.GetRequiredService<IProtocolSerializer>();
        
        var protocol = new Protocol
        {
            Product = new Product(),
            Meta = new Meta { Timestamp = DateTime.UtcNow },
            Results = new Results
            {
                Data =
                [
                    new ValueSetting
                    {
                        Name = "TestValueSetting",
                        Unit = "V",
                        Precision = 0.0,
                        Min = null,
                        Nom = null,
                        Max = null
                    },
                    new Value
                    {
                        Name = "TestValue",
                        Unit = "A",
                        Precision = 0.0,
                        Result = 1.5,
                        Min = null,
                        Nom = null,
                        Max = null,
                        Ok = true
                    }
                ]
            }
        };

        // Act
        var result = protocolSerializer.Serialize(protocol);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotContain("<ValueLimitMinimum");
        result.Value.Should().NotContain("<ValueNominal");
        result.Value.Should().NotContain("<ValueLimitMaximum");
    }

    private ServiceProvider Setup(Action<ProtocolConfigurationBuilder> configAction)
    {
        var services = new ServiceCollection();
        services.AddXmlProtocolServices(configAction);
        return services.BuildServiceProvider();
    }
}