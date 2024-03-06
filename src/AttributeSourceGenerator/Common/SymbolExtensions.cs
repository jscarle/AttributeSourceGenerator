using AttributeSourceGenerator.Models;
using Microsoft.CodeAnalysis;

namespace AttributeSourceGenerator.Common;

/// <summary>Provides extension methods for working with symbols.</summary>
internal static class SymbolExtensions
{
    /// <summary>Gets the <see cref="SymbolType" /> for the given <see cref="ISymbol" /> based on its type kind.</summary>
    /// <param name="symbol">The <see cref="ISymbol" /> to get the <see cref="SymbolType" /> for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="SymbolType" /> if the symbol can be mapped to a symbol type, otherwise null.</returns>
    public static SymbolType GetSymbolType(this ISymbol symbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return symbol switch
        {
            ITypeSymbol typeSymbol => typeSymbol switch
            {
                { IsReferenceType: true, TypeKind: TypeKind.Interface } => SymbolType.Interface,
                { IsReferenceType: true, IsRecord: true } => SymbolType.Record,
                { IsReferenceType: true } => SymbolType.Class,
                { IsValueType: true, IsRecord: true } => SymbolType.RecordStruct,
                { IsValueType: true } => SymbolType.Struct,
                _ => throw new InvalidOperationException($"{nameof(AttributeIncrementalGeneratorBase)} unexpectedly received an {nameof(ISymbol)} that was unsupported.")
            },
            IMethodSymbol => SymbolType.Method,
            _ => throw new InvalidOperationException($"{nameof(AttributeIncrementalGeneratorBase)} unexpectedly received an {nameof(ISymbol)} that was unsupported.")
        };
    }

    /// <summary>Gets a list of declarations representing the hierarchy containing the given symbol.</summary>
    /// <param name="symbol">The <see cref="ISymbol" /> to get the containing declarations for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="EquatableReadOnlyList{T}" /> of <see cref="Declaration" /> objects representing the hierarchy.</returns>
    public static EquatableReadOnlyList<Declaration> GetContainingDeclarations(this ISymbol symbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var declarations = new Stack<Declaration>();

        BuildContainingSymbolHierarchy(symbol, in declarations, cancellationToken);

        return declarations.ToEquatableReadOnlyList();
    }

    /// <summary>Builds the hierarchy of containing symbols starting from the given symbol.</summary>
    /// <param name="symbol">The <see cref="ISymbol" /> to start building the hierarchy from.</param>
    /// <param name="declarations">A <see cref="Stack{T}" /> of <see cref="Declaration" /> objects to store the hierarchy.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private static void BuildContainingSymbolHierarchy(ISymbol symbol, in Stack<Declaration> declarations, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        switch (symbol.ContainingSymbol)
        {
            case INamespaceSymbol namespaceSymbol:
                BuildNamespaceHierarchy(namespaceSymbol, declarations, cancellationToken);
                break;
            case INamedTypeSymbol namedTypeSymbol:
                BuildTypeHierarchy(namedTypeSymbol, declarations, cancellationToken);
                break;
        }
    }

    /// <summary>Builds the hierarchy of containing namespaces starting from the given namespace symbol.</summary>
    /// <param name="symbol">The <see cref="INamespaceSymbol" /> to start building the hierarchy from.</param>
    /// <param name="declarations">A <see cref="Stack{T}" /> of <see cref="Declaration" /> objects to store the hierarchy.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private static void BuildNamespaceHierarchy(INamespaceSymbol symbol, in Stack<Declaration> declarations, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!symbol.IsGlobalNamespace)
        {
            var namespaceDeclaration = new Declaration(DeclarationType.Namespace, symbol.Name, EquatableReadOnlyList<string>.Empty);
            declarations.Push(namespaceDeclaration);
        }

        if (symbol.ContainingNamespace is not null && !symbol.ContainingNamespace.IsGlobalNamespace)
            BuildNamespaceHierarchy(symbol.ContainingNamespace, declarations, cancellationToken);
    }

    /// <summary>Builds the hierarchy of containing types starting from the given type symbol.</summary>
    /// <param name="symbol">The <see cref="INamedTypeSymbol" /> to start building the hierarchy from.</param>
    /// <param name="declarations">A <see cref="Stack{T}" /> of <see cref="Declaration" /> objects to store the hierarchy.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private static void BuildTypeHierarchy(INamedTypeSymbol symbol, in Stack<Declaration> declarations, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var declarationType = symbol.GetDeclarationType(cancellationToken);
        if (declarationType is null)
            return;

        var genericTypeParameters = symbol.GetGenericTypeParameters(cancellationToken);

        var typeDeclaration = new Declaration(declarationType.Value, symbol.Name, genericTypeParameters);
        declarations.Push(typeDeclaration);

        BuildContainingSymbolHierarchy(symbol, declarations, cancellationToken);
    }
}