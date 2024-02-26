using System.Text;
using AttributeSourceGenerator.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator;

/// <summary>Provides a base class for incremental source generators that generate source using marker attributes.</summary>
public abstract class AttributeIncrementalGeneratorBase : IIncrementalGenerator
{
    private readonly AttributeIncrementalGeneratorConfiguration _configuration;

    /// <summary>Initializes a new instance of the <see cref="AttributeIncrementalGeneratorBase" /> class with the given configuration initializer.</summary>
    /// <param name="configuration">The configuration for the generator.</param>
    protected AttributeIncrementalGeneratorBase(AttributeIncrementalGeneratorConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>Initializes a new instance of the <see cref="AttributeIncrementalGeneratorBase" /> class with the given configuration initializer.</summary>
    /// <param name="initializer">A function that provides the configuration for the generator.</param>
    protected AttributeIncrementalGeneratorBase(Func<AttributeIncrementalGeneratorConfiguration> initializer)
    {
        if (initializer is null)
            throw new ArgumentNullException(nameof(initializer));

        _configuration = initializer();
    }

    /// <summary>Initializes the incremental generator.</summary>
    /// <param name="context">The initialization context.</param>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(initializationContext => AddSource(initializationContext, _configuration.AttributeFullyQualifiedName, _configuration.AttributeSource));

        var pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(_configuration.AttributeFullyQualifiedName, (syntaxNode, _) => Filter(syntaxNode, _configuration.SymbolFilter), (syntaxContext, _) => Transform(syntaxContext));

        context.RegisterSourceOutput(pipeline, (productionContext, symbol) => GenerateSourceForSymbol(productionContext, symbol, _configuration.SourceGenerator));
    }

    /// <summary>Adds a source file to the output.</summary>
    /// <param name="context">The post-initialization context.</param>
    /// <param name="name">The name of the source file.</param>
    /// <param name="source">The source code for the file.</param>
    private static void AddSource(IncrementalGeneratorPostInitializationContext context, string name, string? source)
    {
        if (source?.Length > 0)
            context.AddSource($"{name}.g.cs", SourceText.From(source, Encoding.UTF8));
    }

    /// <summary>Determines whether a syntax node should be included based on the filter settings.</summary>
    /// <param name="syntaxNode">The syntax node to filter.</param>
    /// <param name="filter">The filter configuration.</param>
    /// <returns><see langword="true" /> if the syntax node should be included, otherwise <see langword="false" />.</returns>
    private static bool Filter(SyntaxNode syntaxNode, FilterType filter)
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

    /// <summary>Transforms a generator attribute syntax context into a symbol for source generation.</summary>
    /// <param name="context">The generator attribute syntax context.</param>
    /// <returns>The transformed symbol.</returns>
    private static Symbol Transform(GeneratorAttributeSyntaxContext context)
    {
        if (context.TargetSymbol is not INamedTypeSymbol namedTypeSymbol)
            throw new InvalidOperationException($"{nameof(AttributeIncrementalGeneratorBase)} unexpectedly tried to transform a {nameof(context.TargetSymbol)} that was not an {nameof(INamedTypeSymbol)}.");

        var markerAttribute = context.GetMarkerAttribute();
        var containingDeclarations = namedTypeSymbol.GetContainingDeclarations();
        var symbolType = namedTypeSymbol.GetSymbolType();
        var symbolName = namedTypeSymbol.Name;
        var genericTypeParameters = namedTypeSymbol.GetGenericTypeParameters();
        var symbol = new Symbol(markerAttribute, containingDeclarations, symbolType, symbolName, genericTypeParameters);

        return symbol;
    }

    /// <summary>Generates source code for a given symbol.</summary>
    /// <param name="context">The source production context.</param>
    /// <param name="symbol">The symbol to generate source for.</param>
    /// <param name="generate">A function that generates the source code for a symbol.</param>
    private static void GenerateSourceForSymbol(SourceProductionContext context, Symbol symbol, Func<Symbol, string> generate)
    {
        var sourceText = generate(symbol);
        context.AddSource($"{symbol.FullyQualifiedName}.g.cs", sourceText);
    }
}
