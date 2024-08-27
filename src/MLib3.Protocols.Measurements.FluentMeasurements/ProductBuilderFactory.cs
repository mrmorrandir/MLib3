namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ProductBuilderFactory : IProductBuilderFactory
{
    public ProductBuilderFactory()
    {
        
    }

    public IProductBuilder Create()
    {
        return new ProductBuilder();
    }
}