using System.Text;
using AttributeSourceGenerator.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AttributeSourceGenerator;

public abstract class AttributeIncrementalGeneratorBase : IIncrementalGenerator
{
    protected abstract string AttributeFullName { get; }
    protected abstract string AttributeSource { get; }
    protected abstract FilterType AttributeFilter { get; }
    protected abstract Func<Symbol, string> GenerateSourceForSymbol { get; }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(AddSource);

        var pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(AttributeFullName, Filter, Transform);

        context.RegisterSourceOutput(pipeline, GenerateSource);
    }

    private void AddSource(IncrementalGeneratorPostInitializationContext ctx)
    {
        ctx.AddSource($"{AttributeFullName}.g.cs", SourceText.From(AttributeSource, Encoding.UTF8));
    }

    private bool Filter(SyntaxNode syntaxNode, CancellationToken _)
    {
        return AttributeFilter switch
        {
            FilterType.Interface => syntaxNode is InterfaceDeclarationSyntax,
            FilterType.Class => syntaxNode is ClassDeclarationSyntax,
            FilterType.Record => syntaxNode is RecordDeclarationSyntax syntax && syntax.Kind() == SyntaxKind.ClassDeclaration,
            FilterType.Struct => syntaxNode is StructDeclarationSyntax,
            FilterType.RecordStruct => syntaxNode is RecordDeclarationSyntax syntax && syntax.Kind() == SyntaxKind.StructDeclaration,
            FilterType.Method => syntaxNode is MethodDeclarationSyntax,
            _ => true,
        };
    }

    private static Symbol Transform(GeneratorAttributeSyntaxContext context, CancellationToken _)
    {
        var symbol = context.TargetSymbol;
        var containingDeclarations = BuildHierarchy(symbol);
        var symbolName = symbol.Name;
        return new Symbol(symbolName, containingDeclarations);
    }

    private void GenerateSource(SourceProductionContext context, Symbol symbol)
    {
        var sourceText = GenerateSourceForSymbol(symbol);
        context.AddSource($"{symbol.FullName}.g.cs", sourceText);
    }

    private static EquatableReadOnlyList<Declaration> BuildHierarchy(ISymbol symbol)
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
        DeclarationType? declarationType = null;


        if (symbol.IsReferenceType)
        {
            if (symbol.TypeKind == TypeKind.Interface)
                declarationType = DeclarationType.Interface;
            else if (symbol.IsRecord)
                declarationType = DeclarationType.Record;
            else
                declarationType = DeclarationType.Class;
        }
        else if (symbol.IsValueType)
        {
            if (symbol.IsRecord)
                declarationType = DeclarationType.RecordStruct;
            else
                declarationType = DeclarationType.Struct;
        }

        if (declarationType is null)
            return;

        var genericParameters = EquatableReadOnlyList<string>.Empty;
        if (symbol.IsGenericType)
        {
            var typeParameters = new List<string>();
            foreach (var typeParameter in symbol.TypeParameters)
                typeParameters.Add(typeParameter.Name);
            genericParameters = new EquatableReadOnlyList<string>(typeParameters);
        }

        var typeDeclaration = new Declaration(declarationType.Value, symbol.Name, genericParameters);
        declarations.Push(typeDeclaration);

        BuildContainingSymbolHierarchy(symbol, declarations);
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
