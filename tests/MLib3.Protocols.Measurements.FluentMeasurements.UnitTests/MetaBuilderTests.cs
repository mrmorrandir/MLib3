using System.Reflection;

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class MetaBuilderTests
{
    [Fact]
    public void ShouldBuildMeta_WithAllOptions()
    {
        var meta = MetaBuilder.Create()
            .Operator("Test operator")
            .Software("Test software")
            .SoftwareVersion("Test software version")
            .Timestamp(DateTime.Now)
            .Type("Test type")
            .DeviceId("Test device id")
            .DeviceName("Test device name")
            .TestRoutine("Test test routine")
            .TestRoutineVersion("Test test routine version")
            .Build();
        
        meta.Operator.Should().Be("Test operator");
        meta.Software.Should().Be("Test software");
        meta.SoftwareVersion.Should().Be("Test software version");
        meta.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        meta.Type.Should().Be("Test type");
        meta.DeviceId.Should().Be("Test device id");
        meta.DeviceName.Should().Be("Test device name");
        meta.TestRoutine.Should().Be("Test test routine");
        meta.TestRoutineVersion.Should().Be("Test test routine version");
    }
    
    [Fact]
    public void ShouldBuildMeta_WithOnlyRequiredOptions()
    {
        var meta = MetaBuilder.Create()
            .Timestamp(DateTime.Now)
            .Type("Test type")
            .Build();

        meta.Operator.Should().Be(Environment.UserName);
        meta.Software.Should().Be(Assembly.GetEntryAssembly()?.GetName().Name);
        meta.SoftwareVersion.Should().Be(Assembly.GetEntryAssembly()?.GetName().Version?.ToString());
        meta.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        meta.Type.Should().Be("Test type");
        meta.DeviceId.Should().Be(Environment.MachineName);
        meta.DeviceName.Should().Be(Environment.MachineName);
        meta.TestRoutine.Should().BeNull();
        meta.TestRoutineVersion.Should().BeNull();
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenTypeWasNotSet()
    {
        Action action = () => MetaBuilder.Create()
            .Timestamp(DateTime.Now)
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Type*");
    }

    [Fact]
    public void ShouldThrowInvalidOperationException_WhenTimestampWasSetDefault()
    {
        Action action = () => MetaBuilder.Create()
            .Timestamp(default)
            .Type("Test type")
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Timestamp*");
    }
}

public class ProductBuilderTests
{
    [Fact]
    public void ShouldBuildProduct_WithAllOptions()
    {
        var product = ProductBuilder.Create()
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
        var product = ProductBuilder.Create()
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
        Action action = () => ProductBuilder.Create()
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Equipment*");
    }
}

public class ProtocolBuilderTests
{
    [Fact]
    public void ShouldBuildProtocol_WithAllOptions()
    {
        var protocol = ProtocolBuilder.Create()
            .Meta(meta => meta
                .Operator("Test operator")
                .Software("Test software")
                .SoftwareVersion("Test software version")
                .Timestamp(DateTime.Now)
                .Type("Test type")
                .DeviceId("Test device id")
                .DeviceName("Test device name")
                .TestRoutine("Test test routine")
                .TestRoutineVersion("Test test routine version")
                .Build())
            .Product(product => product
                .Equipment("Test equipment")
                .Material("Test material")
                .MaterialText("Test material text")
                .Order("Test order")
                .Build())
            .Results(results => results.OK())
            .Build();
        
        protocol.Meta.Operator.Should().Be("Test operator");
        protocol.Meta.Software.Should().Be("Test software");
        protocol.Meta.SoftwareVersion.Should().Be("Test software version");
        protocol.Meta.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        protocol.Meta.Type.Should().Be("Test type");
        protocol.Meta.DeviceId.Should().Be("Test device id");
        protocol.Meta.DeviceName.Should().Be("Test device name");
        protocol.Meta.TestRoutine.Should().Be("Test test routine");
        protocol.Meta.TestRoutineVersion.Should().Be("Test test routine version");
        
        protocol.Product.Equipment.Should().Be("Test equipment");
        protocol.Product.Material.Should().Be("Test material");
        protocol.Product.MaterialText.Should().Be("Test material text");
        protocol.Product.Order.Should().Be("Test order");
    }
    
    [Fact]
    public void ShouldBuildProtocol_WithOnlyRequiredOptions()
    {
        var protocol = ProtocolBuilder.Create()
            .Meta(meta => meta
                .Timestamp(DateTime.Now)
                .Type("Test type"))
            .Product(product => product
                .Equipment("Test equipment"))
            .Results(results => results.OK())
            .Build();
        
        protocol.Meta.Operator.Should().Be(Environment.UserName);
        protocol.Meta.Software.Should().Be(Assembly.GetEntryAssembly()?.GetName().Name);
        protocol.Meta.SoftwareVersion.Should().Be(Assembly.GetEntryAssembly()?.GetName().Version?.ToString());
        protocol.Meta.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        protocol.Meta.Type.Should().Be("Test type");
        protocol.Meta.DeviceId.Should().Be(Environment.MachineName);
        protocol.Meta.DeviceName.Should().Be(Environment.MachineName);
        protocol.Meta.TestRoutine.Should().BeNull();
        protocol.Meta.TestRoutineVersion.Should().BeNull();
        
        protocol.Product.Equipment.Should().Be("Test equipment");
        protocol.Product.Material.Should().BeNull();
        protocol.Product.MaterialText.Should().BeNull();
        protocol.Product.Order.Should().BeNull();
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenMetaWasNotSet()
    {
        Action action = () => ProtocolBuilder.Create()
            .Product(product => product
                .Equipment("Test equipment"))
            .Results(results => results.OK())
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Meta*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenProductWasNotSet()
    {
        Action action = () => ProtocolBuilder.Create()
            .Meta(meta => meta
                .Timestamp(DateTime.Now)
                .Type("Test type"))
            .Results(results => results.OK())
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Product*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenResultsWasNotSet()
    {
        Action action = () => ProtocolBuilder.Create()
            .Meta(meta => meta
                .Timestamp(DateTime.Now)
                .Type("Test type"))
            .Product(product => product
                .Equipment("Test equipment"))
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Results*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenMetaWasSetNull()
    {
        Action action = () => ProtocolBuilder.Create()
            .Meta((IMeta)null!)
            .Product(product => product
                .Equipment("Test equipment"))
            .Results(results => results.OK())
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Meta*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenProductWasSetNull()
    {
        Action action = () => ProtocolBuilder.Create()
            .Meta(meta => meta
                .Timestamp(DateTime.Now)
                .Type("Test type"))
            .Product((IProduct)null!)
            .Results(results => results.OK())
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Product*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenResultsWasSetNull()
    {
        Action action = () => ProtocolBuilder.Create()
            .Meta(meta => meta
                .Timestamp(DateTime.Now)
                .Type("Test type"))
            .Product(product => product
                .Equipment("Test equipment"))
            .Results((IResults)null!)
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Results*");
    }
}