using System.Collections.Immutable;

namespace MLib3.MVVM.SourceGenerators;

internal readonly record struct ViewModelBaseType(string Namespace, string Name, ImmutableArray<string> ExposedProperties)
{
    public string Namespace { get; } = Namespace;
    public string Name { get; } = Name;
    public ImmutableArray<string> ExposedProperties { get; } = ExposedProperties;
}