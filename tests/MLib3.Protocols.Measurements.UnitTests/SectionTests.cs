namespace MLib3.Protocols.Measurements.UnitTests;

public class SectionTests
{
    [Fact]
    public void ShouldInitializeSection_WhenDefaultConstructorIsUsed()
    {
        var section = new Section();

        section.Name.Should().BeEmpty();
        section.Description.Should().BeNull();
        section.Extensions.Should().BeNull();
        section.Ok.Should().BeFalse();
    }

    [Fact]
    public void ShouldInitializeSection_WhenConstructorWithParametersIsUsed()
    {
        var section = new Section("TestName", "TestDescription", true, new Flag());

        section.Name.Should().Be("TestName");
        section.Description.Should().Be("TestDescription");
        section.Data.Should().HaveCount(1);
        section.Extensions.Should().BeNull();
        section.Ok.Should().BeTrue();
    }
}