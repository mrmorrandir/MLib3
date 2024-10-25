namespace MLib3.Protocols.Measurements.UnitTests;

public class SectionSettingTests
{
    [Fact]
    public void ShouldInitializeSectionSetting_WhenDefaultConstructorIsUsed()
    {
        var sectionSetting = new SectionSetting();

        sectionSetting.Name.Should().BeEmpty();
        sectionSetting.Description.Should().BeNull();
        sectionSetting.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldInitializeSectionSetting_WhenConstructorWithParametersIsUsed()
    {
        var sectionSetting = new SectionSetting("TestName", "TestDescription");

        sectionSetting.Name.Should().Be("TestName");
        sectionSetting.Description.Should().Be("TestDescription");
        sectionSetting.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndNameIsNull()
    {
        Action action = () => _ = new SectionSetting(null!, "TestDescription");

        action.Should().Throw<ArgumentException>();
    }
}