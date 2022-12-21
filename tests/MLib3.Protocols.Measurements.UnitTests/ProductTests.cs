namespace MLib3.Protocols.Measurements.UnitTests;

public class ProductTests
{
    [Fact]
    public void ShouldInitializeProduct_WhenDefaultConstructorIsUsed()
    {
        var product = new Product();

        product.Equipment.Should().BeNull();
        product.Material.Should().BeNull();
        product.MaterialText.Should().BeNull();
        product.Order.Should().BeNull();
        product.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldInitializeProduct_WhenConstructorWithParametersIsUsed()
    {
        var product = new Product("TestSerial", "TestMaterial", "TestMaterialText", "TestOrder");

        product.Equipment.Should().Be("TestSerial");
        product.Material.Should().Be("TestMaterial");
        product.MaterialText.Should().Be("TestMaterialText");
        product.Order.Should().Be("TestOrder");
        product.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndEquipmentIsNull()
    {
        Action action = () => new Product(null, "TestMaterial", "TestMaterialText", "TestOrder");

        action.Should().Throw<ArgumentException>();
    }
}