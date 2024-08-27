namespace MLib3.Protocols.Measurements.FluentMeasurements;

public interface IProductBuilder
{
    IProductBuilder Equipment(string equipment);
    IProductBuilder Material(string material);
    IProductBuilder MaterialText(string materialText);
    IProductBuilder Order(string order);
    
    IProduct Build();
}