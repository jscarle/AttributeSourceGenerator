using Microsoft.CodeAnalysis;

// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator.Common;

/// <summary>Provides extension methods for working with symbols.</summary>
internal static class SymbolExtensions
{
    /// <summary>Gets a list of declarations representing the hierarchy containing the given symbol.</summary>
    /// <param name="symbol">The <see cref="ISymbol" /> to get the containing declarations for.</param>
    /// <returns>An <see cref="EquatableReadOnlyList{T}" /> of <see cref="Declaration" /> objects representing the hierarchy.</returns>
    public static EquatableReadOnlyList<Declaration> GetContainingDeclarations(this ISymbol symbol)
    {
        var declarations = new Stack<Declaration>();

        BuildContainingSymbolHierarchy(symbol, in declarations);

        return declarations.ToEquatableReadOnlyList();
    }

    /// <summary>Builds the hierarchy of containing symbols starting from the given symbol.</summary>
    /// <param name="symbol">The <see cref="ISymbol" /> to start building the hierarchy from.</param>
    /// <param name="declarations">A <see cref="Stack{T}" /> of <see cref="Declaration" /> objects to store the hierarchy.</param>
    private static void BuildContainingSymbolHierarchy(ISymbol symbol, in Stack<Declaration> declarations)
    {
        if (symbol.ContainingType is not null)
            BuildTypeHierarchy(symbol.ContainingType, in declarations);
        else if (symbol.ContainingNamespace is not null)
            BuildNamespaceHierarchy(symbol.ContainingNamespace, declarations);
    }

    /// <summary>Builds the hierarchy of containing types starting from the given type symbol.</summary>
    /// <param name="symbol">The <see cref="INamedTypeSymbol" /> to start building the hierarchy from.</param>
    /// <param name="declarations">A <see cref="Stack{T}" /> of <see cref="Declaration" /> objects to store the hierarchy.</param>
    private static void BuildTypeHierarchy(INamedTypeSymbol symbol, in Stack<Declaration> declarations)
    {
        var declarationType = symbol.GetDeclarationType();
        if (declarationType is null)
            return;

        var genericTypeParameters = symbol.GetGenericTypeParameters();

        var typeDeclaration = new Declaration(declarationType.Value, symbol.Name, genericTypeParameters);
        declarations.Push(typeDeclaration);

        BuildContainingSymbolHierarchy(symbol, declarations);
    }

    /// <summary>Gets the <see cref="DeclarationType" /> for the given <see cref="ITypeSymbol" /> based on its type kind.</summary>
    /// <param name="symbol">The <see cref="ITypeSymbol" /> to get the <see cref="DeclarationType" /> for.</param>
    /// <returns>A <see cref="DeclarationType" /> if the symbol can be mapped to a declaration type, otherwise null.</returns>
    private static DeclarationType? GetDeclarationType(this ITypeSymbol symbol)
    {
        if (symbol.IsReferenceType)
        {
            if (symbol.TypeKind == TypeKind.Interface)
                return DeclarationType.Interface;
            if (symbol.IsRecord)
                return DeclarationType.Record;
            return DeclarationType.Class;
        }

        if (symbol.IsValueType)
        {
            if (symbol.IsRecord)
                return DeclarationType.RecordStruct;
            return DeclarationType.Struct;
        }

        return null;
    }

    /// <summary>Builds the hierarchy of containing namespaces starting from the given namespace symbol.</summary>
    /// <param name="symbol">The <see cref="INamespaceSymbol" /> to start building the hierarchy from.</param>
    /// <param name="declarations">A <see cref="Stack{T}" /> of <see cref="Declaration" /> objects to store the hierarchy.</param>
    private static void BuildNamespaceHierarchy(INamespaceSymbol symbol, in Stack<Declaration> declarations)
    {
        if (!symbol.IsGlobalNamespace)
        {
            var namespaceDeclaration = new Declaration(DeclarationType.Namespace, symbol.Name, EquatableReadOnlyList<string>.Empty);
            declarations.Push(namespaceDeclaration);
        }

        if (symbol.ContainingNamespace is not null && !symbol.ContainingNamespace.IsGlobalNamespace)
            BuildNamespaceHierarchy(symbol.ContainingNamespace, declarations);
    }
}
