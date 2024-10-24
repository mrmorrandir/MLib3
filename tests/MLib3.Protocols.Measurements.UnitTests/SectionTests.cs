namespace MLib3.Protocols.Measurements.UnitTests;

public class SectionTests
{
    [Fact]
    public void ShouldInitializeSection_WhenDefaultConstructorIsUsed()
    {
        var section = new Section();

        section.Name.Should().BeNullOrWhiteSpace();
        section.Description.Should().BeNull();
        section.Extensions.Should().BeNull();
        section.Ok.Should().BeFalse();
    }

    [Fact]
    public void ShouldInitializeSection_WhenConstructorWithParametersIsUsed()
    {
        var sectionSetting = new SectionSetting("TestName", "TestDescription");
        var section = new Section(sectionSetting, new[] { new Flag() }, true);

        section.Name.Should().Be("TestName");
        section.Description.Should().Be("TestDescription");
        section.Data.Should().HaveCount(1);
        section.Extensions.Should().BeNull();
        section.Ok.Should().BeTrue();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndSectionSettingIsNull()
    {
        Action action = () => new Section(null, Array.Empty<IElement>(), true);

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldNotThrowException_WhenConstructorWithParametersIsUsed_AndElementsIsNull()
    {
        var sectionSetting = new SectionSetting("TestName", "TestDescription");
        Action action = () => new Section(sectionSetting, null, true);

        action.Should().NotThrow();
    }
}