﻿namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ValueBuilderTests
{
    [Fact]
    public void ShouldBuildValue_WithAllOptions()
    {
        var value = new ValueBuilder()
            .Name("Test")
            .Description("Test description")
            .Unit("Test unit")
            .Precision(0.001)
            .Max(999)
            .Min(0)
            .Nom(500)
            .Result(234)
            .OK()
            .Build();
        
        value.Name.Should().Be("Test");
        value.Description.Should().Be("Test description");
        value.Unit.Should().Be("Test unit");
        value.Max.Should().Be(999);
        value.Min.Should().Be(0);
        value.Nom.Should().Be(500);
        value.Result.Should().Be(234);
        value.Ok.Should().BeTrue();
    }

    [Fact]
    public void ShouldBuildValue_WithAllOptions_AndNOK()
    {
        var value = new ValueBuilder()
            .Name("Test")
            .Description("Test description")
            .Unit("Test unit")
            .Precision(0.001)
            .Max(999)
            .Min(0)
            .Nom(500)
            .Result(234)
            .OK() // first OK
            .NOK() // then NOK
            .Build();
        
        value.Name.Should().Be("Test");
        value.Description.Should().Be("Test description");
        value.Unit.Should().Be("Test unit");
        value.Max.Should().Be(999);
        value.Min.Should().Be(0);
        value.Nom.Should().Be(500);
        value.Result.Should().Be(234);
        value.Ok.Should().BeFalse();
    }

    [Fact]
    public void ShouldBuildValue_WithOnlyRequiredOptions()
    {
        var value = new ValueBuilder()
            .Name("Test")
            .Unit("Test unit")
            .Result(234)
            .OK()
            .Build();
        
        value.Name.Should().Be("Test");
        value.Description.Should().BeNull();
        value.Unit.Should().Be("Test unit");
        value.Max.Should().BeNull();
        value.Min.Should().BeNull();
        value.Nom.Should().BeNull();
        value.Result.Should().Be(234);
        value.Ok.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildValue_WithEvaluateWhenNoLimitsGiven()
    {
        var value = new ValueBuilder()
            .Name("Test")
            .Unit("Test unit")
            .Result(234)
            .Evaluate()
            .Build();
        
        value.Ok.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildValue_WithEvaluateWhenLimitsGiven()
    {
        var value = new ValueBuilder()
            .Name("Test")
            .Unit("Test unit")
            .Result(234)
            .Min(0)
            .Max(999)
            .Evaluate()
            .Build();
        
        value.Ok.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildValue_WithEvaluateWhenLimitsGivenAndResultIsOutsideLimits()
    {
        var value = new ValueBuilder()
            .Name("Test")
            .Unit("Test unit")
            .Result(234)
            .Min(0)
            .Max(100)
            .Evaluate()
            .Build();
        
        value.Ok.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldBuildValue_WhenCreatedFromSetting()
    {
        IValueSetting setting = new ValueSetting("Test", description: "Test description", unit: "Test unit", precision: 0.001, minimum: 0, nominal: 500, maximum: 999);
        var value = new ValueBuilder(setting)
            .Result(1000)
            .Evaluate()
            .Build();
        
        value.Name.Should().Be("Test");
        value.Description.Should().Be("Test description");
        value.Unit.Should().Be("Test unit");
        value.Precision.Should().Be(0.001);
        value.Max.Should().Be(999);
        value.Min.Should().Be(0);
        value.Nom.Should().Be(500);
        value.Result.Should().Be(1000);
        value.Ok.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenNameWasNotSet()
    {
        Action action = () => new ValueBuilder()
            .Result(234)
            .OK()
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Name*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenUnitWasNotSet()
    {
        Action action = () => new ValueBuilder()
            .Name("Test")
            .Result(234)
            .OK()
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Unit*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenNomIsOutsideLimits()
    {
        Action action = () => new ValueBuilder()
            .Name("Test")
            .Unit("Test unit")
            .Result(234)
            .Min(0)
            .Max(100)
            .Nom(200)
            .OK()
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*nom*");
    }

    [Fact]
    public void ShouldThrowInvalidOperationException_WhenResultValueWasNotSet()
    {
        Action action = () => new ValueBuilder()
            .Name("Test")
            .Unit("Test unit")
            .OK()
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Result*");
    }

    [Fact]
    public void ShouldThrowInvalidOperationException_WhenResultWasNotSet()
    {
        Action action = () => new ValueBuilder()
            .Name("Test")
            .Unit("Test unit")
            .Result(123.4)
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Result*OK*NOK*Evaluate*");
    }
}