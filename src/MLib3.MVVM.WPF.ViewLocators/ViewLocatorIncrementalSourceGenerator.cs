using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace MLib3.MVVM.WPF.ViewLocators;

[Generator]
public class ViewLocatorIncrementalSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var viewModelProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                (s, _) => IsPotentialViewModelBaseImplementor(s),
                (ctx, _) => GetViewModelBaseImplementors(ctx))
            .Where(t => t != null)
            .Collect();

        var viewProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                (s, _) => IsPotentialViewForViewModel(s),
                (ctx, _) => GetViews(ctx))
            .Where(t => t != null)
            .Collect();

        var viewModelViewProvider = viewModelProvider
            .Combine(viewProvider)
            .Select((combination, _) =>
            {
                var list = new List<View2ViewModelType>();
                foreach (var vm in combination.Left)
                foreach (var view in combination.Right)
                {
                    if (!vm.HasValue || !view.HasValue || vm.Value.Name.Substring(0, vm.Value.Name.Length - 2) != view.Value.Name.Substring(0, view.Value.Name.Length - 1)) continue;

                    if (!list.Any(x => x.ViewModelBaseType == vm && x.ViewForViewModelType == view))
                        list.Add(new View2ViewModelType(vm, view));
                }

                return list.ToImmutableArray();
            });

        context.RegisterSourceOutput(context.CompilationProvider.Combine(viewModelViewProvider),
            (ctx, t) => GenerateViewLocator(ctx, t.Left, t.Right));
    }

    private bool IsPotentialViewModelBaseImplementor(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not ClassDeclarationSyntax classDeclaration)
            return false;

        // Check if the class name ends with VM
        if (!classDeclaration.Identifier.Text.EndsWith("VM"))
            return false;

        // Check if class derives from ViewModelBase
        if (classDeclaration.BaseList is null)
            return false;

        return true;
    }

    private static ViewModelBaseType? GetViewModelBaseImplementors(GeneratorSyntaxContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        var semanticModel = context.SemanticModel;
        var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

        if (classSymbol == null)
            return null;

        if (!classSymbol.Name.EndsWith("VM"))
            return null;

        var implementsViewModelBase = false;
        var baseType = classSymbol.BaseType;
        while (baseType != null)
        {
            if (baseType.ToDisplayString() == "MLib3.MVVM.ViewModel" || baseType.ToDisplayString() == "MLib3.MVVM.ViewModelValidator")
            {
                implementsViewModelBase = true;
                break;
            }

            baseType = baseType.BaseType;
        }

        if (!implementsViewModelBase) return null;

        // Get the Namespace of the class
        var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
        // Get the Name of the class
        var className = classSymbol.Name;

        return new ViewModelBaseType(namespaceName, className);
    }

    private bool IsPotentialViewForViewModel(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not ClassDeclarationSyntax classDeclaration)
            return false;

        // Check if the class name ends with V
        if (!classDeclaration.Identifier.Text.EndsWith("V"))
            return false;

        // Check if class derives from FrameworkElement
        if (classDeclaration.BaseList is null)
            return false;

        return true;
    }

    private static ViewForViewModelType? GetViews(GeneratorSyntaxContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        var semanticModel = context.SemanticModel;
        var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

        if (classSymbol == null)
            return null;

        if (!classSymbol.Name.EndsWith("V"))
            return null;

        // view must derive from FrameworkElement
        var implementsFrameworkElement = false;
        var baseType = classSymbol.BaseType;
        while (baseType != null)
        {
            if (baseType.ToDisplayString() == "System.Windows.FrameworkElement")
            {
                implementsFrameworkElement = true;
                break;
            }

            baseType = baseType.BaseType;
        }

        if (!implementsFrameworkElement) return null;

        // Get the Namespace of the class
        var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
        // Get the Name of the class
        var className = classSymbol.Name;

        return new ViewForViewModelType(namespaceName, className);
    }

    private void GenerateViewLocator(SourceProductionContext context, Compilation compilation, ImmutableArray<View2ViewModelType> view2ViewModelTypes)
    {
        var viewLocatorMethods = new StringBuilder();
        var registerMethodCalls = new StringBuilder();

        foreach (var v2vm in view2ViewModelTypes)
        {
            var viewModelName = v2vm.ViewModelBaseType?.Name;
            var viewModelNamespace = v2vm.ViewModelBaseType?.Namespace;

            var viewName = v2vm.ViewForViewModelType?.Name;
            var viewNamespace = v2vm.ViewForViewModelType?.Namespace;

            viewLocatorMethods.AppendLine($@"
        private static DataTemplate Create{viewModelName}Template()
        {{
            return new DataTemplate
            {{
                DataType = typeof({viewModelNamespace}.{viewModelName}),
                VisualTree = new FrameworkElementFactory(typeof({viewNamespace}.{viewName}))
            }};
        }}");

            registerMethodCalls.AppendLine($"            resourceDictionary.Add(new DataTemplateKey(typeof({viewModelNamespace}.{viewModelName})), Create{viewModelName}Template());");
        }

        // get the root namespace of the project to be able to generate the correct namespace for the view locator
        var rootNamespace = compilation.AssemblyName;
        var source = $@"// <auto-generated/>
using System.Windows;
using System.Windows.Controls;

namespace {rootNamespace} 
{{
    public static class ViewLocator
    {{
{viewLocatorMethods}

        public static void Register(ResourceDictionary resourceDictionary)
        {{
{registerMethodCalls}
        }}
    }}
}}";

        context.AddSource("ViewLocator.g.cs", SourceText.From(source, Encoding.UTF8));
    }
}