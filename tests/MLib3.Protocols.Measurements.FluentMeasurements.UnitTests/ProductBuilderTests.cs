namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ProductBuilderTests
{
    [Fact]
    public void ShouldBuildProduct_WithAllOptions()
    {
        var product = new ProductBuilder()
            .Equipment("Test equipment")
            .Material("Test material")
            .MaterialText("Test material text")
            .Order("Test order")
            .Build();
        
        product.Equipment.Should().Be("Test equipment");
        product.Material.Should().Be("Test material");
        product.MaterialText.Should().Be("Test material text");
        product.Order.Should().Be("Test order");
    }
    
    [Fact]
    public void ShouldBuildProduct_WithOnlyRequiredOptions()
    {
        var product = new ProductBuilder()
            .Equipment("Test equipment")
            .Build();

        product.Equipment.Should().Be("Test equipment");
        product.Material.Should().BeNull();
        product.MaterialText.Should().BeNull();
        product.Order.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowInvalidOperationException_WhenEquipmentWasNotSet()
    {
        Action action = () => new ProductBuilder()
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Equipment*");
    }
}