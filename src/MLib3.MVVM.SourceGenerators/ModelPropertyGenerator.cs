using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace MLib3.MVVM.SourceGenerators;

[Generator]
public class ModelPropertyGenerator : IIncrementalGenerator
{
    private const string AttributeSourceCode = @"
namespace MLib3.MVVM.SourceGenerators
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ModelPropertyAttribute : Attribute
    {
    }
}";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Add the marker attribute to the compilation.
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource("ModelPropertyAttribute.g.cs", SourceText.From(AttributeSourceCode, Encoding.UTF8)));

        // Filter classes that have properties annotated with the [ModelProperty] attribute.
        var provider = context.SyntaxProvider
            .CreateSyntaxProvider(
                (s, _) => s is PropertyDeclarationSyntax,
                (ctx, _) => GetPropertyDeclarationForSourceGen(ctx))
            .Where(t => t.modelPropertyAttributeFound)
            .Select((t, _) => t.Item1);

        // Generate the source code.
        context.RegisterSourceOutput(context.CompilationProvider.Combine(provider.Collect()),
            (ctx, t) => GenerateCode(ctx, t.Left, t.Right));
    }

    private void GenerateCode(SourceProductionContext ctx, Compilation compilation, ImmutableArray<PropertyDeclarationSyntax> properties)
    {
        // group the properties by class
        var classGroup = properties.GroupBy(p => p.Parent as ClassDeclarationSyntax).ToImmutableArray();
        foreach (var @class in classGroup)
        {
            var classDeclaration = @class.Key;
            if (classDeclaration == null) continue;
            
            var className = classDeclaration.Identifier.Text;

            // find the namespace declaration or the file namespace declaration
            string? namespaceName = null;
            var namespaceDeclaration = classDeclaration.Parent;
            if (namespaceDeclaration is NamespaceDeclarationSyntax namespaceDeclarationSyntax)
            {
                namespaceName = namespaceDeclarationSyntax.Name.ToString();
            }
            if (namespaceDeclaration is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclarationSyntax)
            {
                namespaceName = fileScopedNamespaceDeclarationSyntax.Name.ToString();
            }
            if (namespaceName is null) continue;
            
            var switchSource = new StringBuilder();
            switchSource.AppendLine($@"
                switch(property) {{
            ");
            foreach (var property in @class)
            {
                var propertyName = property.Identifier.Text;
                var propertyType = property.Type.ToString();
                switchSource.AppendLine($@"
                    case nameof({propertyName}):
                        var oldValue = Model.{propertyName};
                        if (EqualityComparer<{propertyType}>.Default.Equals(oldValue, value)) return;
                        Model.{propertyName} = value;
                        callback?.Invoke(oldValue, value);
                        OnPropertyChanged(nameof({propertyName}));
                        break;
                ");
            }
            switchSource.AppendLine($@"
                default:
                    throw new ArgumentException($""Property {{property}} not found on model {{typeof({className}).Name}}"");
                }}");

            var source = $@"
namespace {namespaceName}
{{
    public partial class {className}
    {{
        public void SetModel<TValue>(TValue? value, ValueChangedCallback<TValue?>? callback = null, [CallerMemberName] string? property = null)
        {{
            {switchSource}
        }}
    }}
}}";
            ctx.AddSource($"{className}_g.cs", SourceText.From(source, Encoding.UTF8));
        }
    }

    private static (PropertyDeclarationSyntax, bool modelPropertyAttributeFound) GetPropertyDeclarationForSourceGen(
        GeneratorSyntaxContext ctx)
    {
        var propertyDeclarationSyntax = (PropertyDeclarationSyntax)ctx.Node;
        // find the Property that has the ModelPropertyAttribute
        var modelPropertyAttributeFound = false;
        foreach (var attributeListSyntax in propertyDeclarationSyntax.AttributeLists)
        foreach (var attributeSyntax in attributeListSyntax.Attributes)
        {
            if (ctx.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                continue; // if we can't get the symbol, ignore it

            var attributeName = attributeSymbol.ContainingType.ToDisplayString();
            if (attributeName == "MLib3.MVVM.SourceGenerators.ModelPropertyAttribute")
            {
                modelPropertyAttributeFound = true;
                break;
            }
        }
        return (propertyDeclarationSyntax, modelPropertyAttributeFound);
    }
}