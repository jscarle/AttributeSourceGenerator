using System.Text;
using AttributeSourceGenerator.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator;

public abstract class AttributeIncrementalGeneratorBase : IIncrementalGenerator
{
    protected abstract string AttributeFullyQualifiedName { get; }
    protected virtual string AttributeSource => "";
    protected abstract FilterType AttributeFilter { get; }
    protected abstract Func<Symbol, string> GenerateSource { get; }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(initializationContext => AddSource(initializationContext, AttributeFullyQualifiedName, AttributeSource));

        var pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(AttributeFullyQualifiedName, (node, token) => Filter(node, AttributeFilter, token), Transform);

        context.RegisterSourceOutput(pipeline, (productionContext, symbol) => GenerateSourceForSymbol(productionContext, symbol, GenerateSource));
    }

    private static void AddSource(IncrementalGeneratorPostInitializationContext context, string name, string source)
    {
        if (source.Length > 0)
            context.AddSource($"{name}.g.cs", SourceText.From(source, Encoding.UTF8));
    }

    private static bool Filter(SyntaxNode syntaxNode, FilterType filter, CancellationToken _)
    {
        if (filter.HasFlag(FilterType.Interface) && syntaxNode is InterfaceDeclarationSyntax)
            return true;
        if (filter.HasFlag(FilterType.Class) && syntaxNode is ClassDeclarationSyntax)
            return true;
        if (filter.HasFlag(FilterType.Record) && syntaxNode is RecordDeclarationSyntax recordDeclaration && recordDeclaration.Kind() == SyntaxKind.ClassDeclaration)
            return true;
        if (filter.HasFlag(FilterType.Struct) && syntaxNode is StructDeclarationSyntax)
            return true;
        if (filter.HasFlag(FilterType.RecordStruct) && syntaxNode is RecordDeclarationSyntax recordStructDeclaration && recordStructDeclaration.Kind() == SyntaxKind.StructDeclaration)
            return true;
        if (filter.HasFlag(FilterType.Method) && syntaxNode is MethodDeclarationSyntax)
            return true;
        return false;
    }

    private static Symbol Transform(GeneratorAttributeSyntaxContext context, CancellationToken _)
    {
        if (context.TargetSymbol is not INamedTypeSymbol namedTypeSymbol)
            throw new InvalidOperationException($"{nameof(AttributeIncrementalGeneratorBase)} unexpectedly tried to transform a {nameof(context.TargetSymbol)} that was not an {nameof(INamedTypeSymbol)}.");
        
        var markerAttribute = context.GetMarkerAttribute();
        var containingDeclarations = namedTypeSymbol.GetContainingDeclarations();
        var symbolName = namedTypeSymbol.Name;
        var genericTypeParameters = namedTypeSymbol.GetGenericTypeParameters();
        var symbol = new Symbol(markerAttribute, containingDeclarations, symbolName, genericTypeParameters);

        return symbol;
    }

    private static void GenerateSourceForSymbol(SourceProductionContext context, Symbol symbol, Func<Symbol, string> generate)
    {
        var sourceText = generate(symbol);
        context.AddSource($"{symbol.FullyQualifiedName}.g.cs", sourceText);
    }
}
