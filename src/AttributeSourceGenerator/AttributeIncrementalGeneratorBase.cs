using System.Text;
using AttributeSourceGenerator.Common;
using AttributeSourceGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

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
        context.RegisterPostInitializationOutput(AddMarkerAttributeSource);

        //var optionsProvider = GetGeneratorOptions(context);
        //context.RegisterSourceOutput(optionsProvider, AddMarkerAttributeSource);

        var syntaxProvider = context.SyntaxProvider.ForAttributeWithMetadataName(_configuration.MarkerAttributeName, Filter, Transform);
        context.RegisterSourceOutput(syntaxProvider, (productionContext, symbol) => GenerateSourceForSymbol(productionContext, symbol));
    }

    /// <summary>Adds the marker attribute source to the output.</summary>
    /// <param name="context">The post-initialization context.</param>
    private void AddMarkerAttributeSource(IncrementalGeneratorPostInitializationContext context)
    {
        if (_configuration.MarkerAttributeSource?.Length > 0)
            context.AddSource($"{_configuration.MarkerAttributeName}.g.cs", SourceText.From(_configuration.MarkerAttributeSource, Encoding.UTF8));
    }

    /// <summary>Adds the marker attribute source to the output.</summary>
    /// <param name="context">The source production context.</param>
    /// <param name="options">The source generator options.</param>
    private void AddMarkerAttributeSource(SourceProductionContext context, AttributeIncrementalGeneratorOptions options)
    {
        if (options.IncludeMarkerAttributeSource && _configuration.MarkerAttributeSource?.Length > 0)
            context.AddSource($"{_configuration.MarkerAttributeName}.g.cs", SourceText.From(_configuration.MarkerAttributeSource, Encoding.UTF8));
    }

    /// <summary>Retrieves the generator options based on the provided context.</summary>
    /// <param name="context">The incremental generator initialization context.</param>
    /// <returns>An <see cref="IncrementalValueProvider{T}" /> for the generator options.</returns>
    private static IncrementalValueProvider<AttributeIncrementalGeneratorOptions> GetGeneratorOptions(IncrementalGeneratorInitializationContext context)
    {
        return context.AnalyzerConfigOptionsProvider.Select((options, cancellationToken) =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            options.GlobalOptions.TryGetValue("build_property.IncludeMarkerAttributeSource", out var value);
            var includeMarkerAttributeSource = IsTrue(value);

            return new AttributeIncrementalGeneratorOptions { IncludeMarkerAttributeSource = includeMarkerAttributeSource };
        });
    }

    /// <summary>Determines whether the provided property value represents a true state.</summary>
    /// <param name="propertyValue">The property value to evaluate.</param>
    /// <returns><c>true</c> if the property value is null, empty, or case-insensitive "true"; otherwise, <c>false</c>.</returns>
    private static bool IsTrue(string? propertyValue)
    {
        return string.IsNullOrWhiteSpace(propertyValue) || StringComparer.InvariantCultureIgnoreCase.Equals("true", propertyValue);
    }

    /// <summary>Determines whether a syntax node should be included based on the filter settings.</summary>
    /// <param name="syntaxNode">The syntax node to filter.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true" /> if the syntax node should be included, otherwise <see langword="false" />.</returns>
    private bool Filter(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var filter = _configuration.SymbolFilter == FilterType.None ? FilterType.All : _configuration.SymbolFilter;

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
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The transformed symbol.</returns>
    private static Symbol Transform(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var targetSymbol = context.TargetSymbol;
        if (targetSymbol is not INamedTypeSymbol && targetSymbol is not IMethodSymbol)
            throw new InvalidOperationException($"{nameof(AttributeIncrementalGeneratorBase)} unexpectedly tried to transform a {nameof(context.TargetSymbol)} that was not an {nameof(INamedTypeSymbol)} or a {nameof(IMethodSymbol)}.");

        var markerAttribute = context.GetMarkerAttribute(cancellationToken);
        var containingDeclarations = targetSymbol.GetContainingDeclarations(cancellationToken);
        var symbolType = targetSymbol.GetSymbolType(cancellationToken);
        var symbolName = targetSymbol.Name;
        EquatableReadOnlyList<string> genericTypeParameters;
        EquatableReadOnlyList<MethodParameter> constructorParameters;
        string returnType;
        switch (targetSymbol)
        {
            case INamedTypeSymbol namedTypeSymbol:
                genericTypeParameters = namedTypeSymbol.GetGenericTypeParameters(cancellationToken);
                constructorParameters = EquatableReadOnlyList<MethodParameter>.Empty;
                returnType = "";
                /*
                TODO: Analyze members.
                var members = namedTypeSymbol.GetMembers();
                foreach (var member in members)
                    switch (member)
                    {
                        case IPropertySymbol propertySymbol:
                            break;
                        case IMethodSymbol methodSymbol:
                            break;
                    }
                */
                break;
            case IMethodSymbol methodSymbol:
                genericTypeParameters = methodSymbol.GetGenericTypeParameters(cancellationToken);
                constructorParameters = methodSymbol.GetMethodParameters(cancellationToken);
                returnType = methodSymbol.ReturnType.ToDisplayString();
                break;
            default:
                genericTypeParameters = EquatableReadOnlyList<string>.Empty;
                constructorParameters = EquatableReadOnlyList<MethodParameter>.Empty;
                returnType = "";
                break;
        }

        var symbol = new Symbol(markerAttribute, containingDeclarations, symbolType, symbolName, genericTypeParameters, constructorParameters, returnType);

        return symbol;
    }

    /// <summary>Generates source code for a given symbol.</summary>
    /// <param name="context">The source production context.</param>
    /// <param name="symbol">The symbol to generate source for.</param>
    private void GenerateSourceForSymbol(SourceProductionContext context, Symbol symbol)
    {
        var sourceText = _configuration.SourceGenerator(symbol);
        context.AddSource($"{symbol.FullyQualifiedName}.g.cs", sourceText);
    }
}
