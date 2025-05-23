﻿namespace MLib3.Protocols.Measurements.UnitTests;

public class ResultsTests
{
    [Fact]
    public void ShouldInitializeResults_WhenDefaultConstructorIsUsed()
    {
        var results = new Results();

        results.Data.Should().NotBeNull();
        results.Extensions.Should().BeNull();
        results.Ok.Should().BeFalse();
    }

    [Fact]
    public void ShouldInitializeResults_WhenConstructorWithParametersIsUsed()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);
        var results = new Results(true, flag);

        results.Data.Should().HaveCount(1);
        results.Extensions.Should().BeNull();
        results.Ok.Should().BeTrue();
    }

    [Fact]
    public void Add_ShouldAddElement_WhenElementIsNotNull()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);
        var results = new Results();

        results.Add(flag);

        results.Data.Should().HaveCount(1);
    }

    [Fact]
    public void Add_ShouldThrowArgumentException_WhenElementIsNull()
    {
        var results = new Results();

        Action action = () => results.Add((Element)null!);

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Add_ShouldThrowArgumentException_WhenElementAlreadyExists()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);
        var results = new Results(new Element[] { flag });

        Action action = () => results.Add(flag);

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddRange_ShouldAddElements_WhenElementsAreNotNull()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);

        var flagSetting2 = new FlagSetting("TestFlag2", "TestDescription2");
        var flag2 = new Flag(flagSetting2, true);

        var results = new Results();

        results.AddRange(new[] { flag, flag2 });

        results.Data.Should().HaveCount(2);
    }

    [Fact]
    public void AddRange_ShouldThrowArgumentException_WhenElementsAreNull()
    {
        var results = new Results();

        Action action = () => results.AddRange((Element[])null!);

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddRange_ShouldThrowArgumentException_WhenElementsContainsNull()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);

        var flagSetting2 = new FlagSetting("TestFlag2", "TestDescription2");
        var flag2 = new Flag(flagSetting2, true);

        var results = new Results();

        Action action = () => results.AddRange(new[] { flag, null!, flag2 });

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddRange_ShouldThrowArgumentException_WhenElementsContainsElementsWithNonUniqueName()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);

        var flagSetting2 = new FlagSetting("TestFlag", "TestDescription2");
        var flag2 = new Flag(flagSetting2, true);

        var results = new Results();

        Action action = () => results.AddRange(new[] { flag, flag2 });

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddRange_ShouldThrowArgumentException_WhenElementsContainsElementWhichAlreadyExists()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);

        var flagSetting2 = new FlagSetting("TestFlag2", "TestDescription2");
        var flag2 = new Flag(flagSetting2, true);

        var results = new Results();
        results.Add(flag);

        Action action = () => results.AddRange(new[] { flag, flag2 });

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddRange2_ShouldAddElements_WhenElementsAreNotNull()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);
        var flagSetting2 = new FlagSetting("TestFlag2", "TestDescription2");
        var flag2 = new Flag(flagSetting2, true);
        var results = new Results();
        results.AddRange(new[] { flag, flag2 });
    
        var results2 = new Results();
        results2.Add(results);
    
        results2.Data.Should().HaveCount(2);
    }
    
    [Fact]
    public void AddRange2_ShouldThrowArgumentException_WhenElementsAreNull()
    {
        var results = new Results();
    
        Action action = () => results.Add((Results)null!);
    
        action.Should().Throw<Exception>();
    }
    
    [Fact]
    public void AddRange2_ShouldThrowArgumentException_WhenElementsContainsElementWhichAlreadyExists()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);
        var flagSetting2 = new FlagSetting("TestFlag2", "TestDescription2");
        var flag2 = new Flag(flagSetting2, true);
        var results = new Results();
        results.AddRange(new[] { flag, flag2 });
    
        var results2 = new Results();
        results2.Add(flag);
        Action action = () => results2.Add(results);
    
        action.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void Remove_ShouldRemoveElement_WhenElementIsNotNull()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);
        var results = new Results(flag);
    
        results.Remove(flag);
    
        results.Data.Should().HaveCount(0);
    }
    
    [Fact]
    public void Remove_ShouldThrowArgumentException_WhenElementIsNull()
    {
        var results = new Results();
    
        Action action = () => results.Remove((Element)null!);
    
        action.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void Remove_ShouldThrowArgumentException_WhenElementDoesNotExist()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);
        var results = new Results();
    
        Action action = () => results.Remove(flag);
    
        action.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void RemoveRange_ShouldRemoveElements_WhenElementsAreNotNull()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);
    
        var flagSetting2 = new FlagSetting("TestFlag2", "TestDescription2");
        var flag2 = new Flag(flagSetting2, true);
    
        var results = new Results(flag, flag2);
    
        results.RemoveRange(new[] { flag, flag2 });
    
        results.Data.Should().HaveCount(0);
    }
    
    [Fact]
    public void RemoveRange_ShouldThrowArgumentException_WhenElementsAreNull()
    {
        var results = new Results();
    
        Action action = () => results.RemoveRange((Element[])null!);
    
        action.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void RemoveRange_ShouldThrowArgumentException_WhenElementsContainsNull()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);
    
        var flagSetting2 = new FlagSetting("TestFlag2", "TestDescription2");
        var flag2 = new Flag(flagSetting2, true);
    
        var results = new Results(flag, flag2);
    
        Action action = () => results.RemoveRange(new[] { flag, null!, flag2 });
    
        action.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void RemoveRange_ShouldThrowArgumentException_WhenElementsContainsElementsWhichDoNotExist()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);
    
        var flagSetting2 = new FlagSetting("TestFlag2", "TestDescription2");
        var flag2 = new Flag(flagSetting2, true);
    
        var results = new Results();
    
        Action action = () => results.RemoveRange(new[] { flag, flag2 });
    
        action.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void RemoveRange2_ShouldRemoveElements_WhenElementsAreNotNull()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);
        var flagSetting2 = new FlagSetting("TestFlag2", "TestDescription2");
        var flag2 = new Flag(flagSetting2, true);
        var results = new Results(flag, flag2);
    
        var results2 = new Results(flag, flag2);
        results.Remove(results2);
    
        results.Data.Should().HaveCount(0);
    }
    
    [Fact]
    public void RemoveRange2_ShouldThrowArgumentException_WhenElementsAreNull()
    {
        var results = new Results();
    
        Action action = () => results.Remove((Results)null!);
    
        action.Should().Throw<Exception>();
    }
    
    [Fact]
    public void Clear_ShouldClearElements()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);
        var results = new Results(flag);
    
        results.Clear();
    
        results.Data.Should().HaveCount(0);
    }

    [Fact]
    public void Indexer_ShouldReturnElement_WhenElementExists()
    {
        var flagSetting = new FlagSetting("TestFlag", "TestDescription");
        var flag = new Flag(flagSetting, true);
        var results = new Results(flag);

        var result = results["TestFlag"];

        result.Should().Be(flag);
    }

    [Fact]
    public void Indexer_ShouldThrowArgumentException_WhenElementDoesNotExist()
    {
        var results = new Results();

        Action action = () => _ = results["TestFlag"];

        action.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void Indexer_ShouldThrowArgumentException_WhenNameIsNullOrWhitespace()
    {
        var results = new Results();

        Action action = () => _ = results[null!];

        action.Should().Throw<KeyNotFoundException>();
    }
}