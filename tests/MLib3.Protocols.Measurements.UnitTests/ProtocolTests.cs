namespace MLib3.Protocols.Measurements.UnitTests;

public class ProtocolTests
{
    [Fact]
    public void ShouldInitialize_WhenCalledWithoutParameters()
    {
        var protocol = new Protocol();

        protocol.Product.Should().NotBeNull();
        protocol.Product.Equipment.Should().Be("Unknown");
        protocol.Meta.Should().NotBeNull();
        protocol.Meta.Type.Should().Be("Unknown");
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