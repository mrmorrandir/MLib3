namespace MLib3.Protocols.Measurements.UnitTests;

public class RawDataTests
{
    [Fact]
    public void ShouldInitializeValue_WhenDefaultConstructorIsUsed()
    {
        var rawData = new RawData();

        rawData.Name.Should().BeEmpty();
        rawData.Description.Should().BeNull();
        rawData.Format.Should().BeEmpty();
        rawData.Raw.Should().BeEmpty();
        rawData.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldInitializeValue_WhenConstructorWithParametersIsUsed()
    {
        var rawDataSetting = new RawDataSetting()
        {
            Name = "Test",
            Description = "Test",
            Format = "csv"
        };
        var rawData = new RawData(rawDataSetting, "raw");

        rawData.Name.Should().Be(rawDataSetting.Name);
        rawData.Description.Should().Be(rawDataSetting.Description);
        rawData.Format.Should().Be(rawDataSetting.Format);
        rawData.Raw.Should().Be("raw");
        rawData.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndValueSettingsIsNull()
    {
        Action action = () => new RawData(null!, "raw");

        action.Should().Throw<Exception>();
    }
}