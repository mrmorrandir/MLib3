using BenchmarkDotNet.Attributes;

namespace MLib3.MVVM.Benchmarks;

public class ViewModelBaseBenchmark
{
    [Params(1000)]
    public int Iterations { get; set; }
    
    [Benchmark]
    public TestVM SetModel_WithReflection()
    {
        var vm = new TestVM();
        for (int i = 0; i < Iterations; i++)
        {
            vm.Name = $"Test{i}";
        }

        return vm;
    }
    
    [Benchmark]
    public TestVM SetModel2_WithoutReflectionAndSeperateDictionaries()
    {
        var vm = new TestVM();
        for (int i = 0; i < Iterations; i++)
        {
            vm.Name2 = $"Test{i}";
        }

        return vm;
    }
    
    [Benchmark]
    public TestVM SetModel3_WithoutReflectionAndCombinedDictionary()
    {
        var vm = new TestVM();
        for (int i = 0; i < Iterations; i++)
        {
            vm.Name3 = $"Test{i}";
        }

        return vm;
    }
    
    [Benchmark]
    public TestVM SetModel4_WithSourceGeneration()
    {
        var vm = new TestVM();
        for (int i = 0; i < Iterations; i++)
        {
            vm.Name4 = $"Test{i}";
        }

        return vm;
    }
}