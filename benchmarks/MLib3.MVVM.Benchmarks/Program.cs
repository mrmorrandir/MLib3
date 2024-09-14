using BenchmarkDotNet.Running;
using MLib3.MVVM.Benchmarks;

var summary = BenchmarkRunner.Run<ViewModelBaseBenchmark>();