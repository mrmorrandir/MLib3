namespace MLib3.Protocols.Measurements.UnitTests;

public class ExtensionTests
{
    [Fact]
    public void ShouldInitializeExtension_WhenDefaultConstructorIsUsed()
    {
        var extension = new Extension();

        extension.Key.Should().BeNull();
        extension.Value.Should().BeNull();
    }

    [Fact]
    public void ShouldInitializeExtension_WhenConstructorIsUsed()
    {
        var extension = new Extension("TestKey", "TestValue");

        extension.Key.Should().Be("TestKey");
        extension.Value.Should().Be("TestValue");
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorIsUsed_AndKeyIsNull()
    {
        Action action = () => _ = new Extension(null!, "TestValue");

        action.Should().Throw<ArgumentException>();
    }
}