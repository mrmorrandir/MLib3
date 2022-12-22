using MLib3.Protocols.Measurements.FluentMeasurements.Abstractions;

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ProductBuilder : IProductBuilder
{
    private readonly Product _product;
    private readonly List<Action> _builders = new();
    
    private ProductBuilder()
    {
        _product = new Product();
    }
    
    public static IProductBuilder Create()
    {
        return new ProductBuilder();
    }

    public IProductBuilder Equipment(string equipment)
    {
        _builders.Add(() => _product.Equipment = equipment);
        return this;
    }

    public IProductBuilder Material(string material)
    {
        _builders.Add(() => _product.Material = material);
        return this;
    }

    public IProductBuilder MaterialText(string materialText)
    {
        _builders.Add(() => _product.MaterialText = materialText);
        return this;
    }

    public IProductBuilder Order(string order)
    {
        _builders.Add(() => _product.Order = order);
        return this;
    }

    public IProduct Build()
    {
        foreach (var builder in _builders)
            builder();
        if (string.IsNullOrWhiteSpace(_product.Equipment))
            throw new InvalidOperationException($"{nameof(_product.Equipment)} is not set.");
        return _product;
    }
}