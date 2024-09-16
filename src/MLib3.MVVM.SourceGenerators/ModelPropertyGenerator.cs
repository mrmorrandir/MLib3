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
        foreach (var property in properties)
        {
            var classDeclaration = property.Parent as ClassDeclarationSyntax;
            if (classDeclaration == null) continue;

            var namespaceDeclaration = classDeclaration.Ancestors().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            if (namespaceDeclaration == null) continue;

            var namespaceName = namespaceDeclaration.Name.ToString();
            var className = classDeclaration.Identifier.Text;
            var propertyName = property.Identifier.Text;
            var propertyType = property.Type.ToString();

            var source = $@"
namespace {namespaceName}
{{
    public partial class {className}
    {{
        public {propertyType} Get{propertyName}()
        {{
            return Model.{propertyName};
        }}

        public void Set{propertyName}({propertyType} value)
        {{
            Model.{propertyName} = value;
        }}
    }}
}}";

            ctx.AddSource($"{className}_{propertyName}_Generated.cs", SourceText.From(source, Encoding.UTF8));
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