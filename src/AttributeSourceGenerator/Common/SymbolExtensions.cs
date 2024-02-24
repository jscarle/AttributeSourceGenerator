using Microsoft.CodeAnalysis;

// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator.Common;

internal static class SymbolExtensions
{
    public static EquatableReadOnlyList<Declaration> GetContainingDeclarations(this ISymbol symbol)
    {
        var declarations = new Stack<Declaration>();

        BuildContainingSymbolHierarchy(symbol, in declarations);

        return declarations.ToEquatableReadOnlyList();
    }

    private static void BuildContainingSymbolHierarchy(ISymbol symbol, in Stack<Declaration> declarations)
    {
        if (symbol.ContainingType is not null)
            BuildTypeHierarchy(symbol.ContainingType, in declarations);
        else if (symbol.ContainingNamespace is not null)
            BuildNamespaceHierarchy(symbol.ContainingNamespace, declarations);
    }

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
