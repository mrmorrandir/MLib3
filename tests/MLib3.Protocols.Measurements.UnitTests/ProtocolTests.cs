namespace MLib3.Protocols.Measurements.UnitTests;

public class ProtocolTests
{
    [Fact]
    public void ShouldInitialize_WhenCalledWithoutParameters()
    {
        var protocol = new Protocol();

        protocol.Product.Should().NotBeNull();
        protocol.Product.Equipment.Should().BeNull();
        protocol.Meta.Should().NotBeNull();
        protocol.Meta.Type.Should().BeNull();
        protocol.Results.Should().NotBeNull();
        protocol.Specification.Should().NotBeNullOrWhiteSpace();
        protocol.Version.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void ShouldInitialize_WhenCalledWithMinimumParameters()
    {
        var protocol = new Protocol("Test", "1234567");

        protocol.Product.Should().NotBeNull();
        protocol.Product.Equipment.Should().Be("1234567");
        protocol.Meta.Should().NotBeNull();
        protocol.Meta.Type.Should().Be("Test");
        protocol.Results.Should().NotBeNull();
        protocol.Specification.Should().NotBeNullOrWhiteSpace();
        protocol.Version.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void ShouldInitialize_WhenCalledWithMaximumParameters()
    {
        var product = new Product("ValidSerial");
        var meta = new Meta("ValidType", DateTime.Now);
        var results = new Results();

        var func = () => new Protocol(product, meta, results);
        func.Should().NotThrow();

        var protocol = func();
        protocol.Product.Should().Be(product);
        protocol.Meta.Should().Be(meta);
        protocol.Results.Should().Be(results);
        protocol.Results.Data.Should().BeEmpty();
        protocol.Specification.Should().NotBeNullOrWhiteSpace();
        protocol.Version.Should().NotBeNullOrWhiteSpace();
    }
}