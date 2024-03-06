using AttributeSourceGenerator.Common;
using AttributeSourceGenerator.Models;

namespace AttributeSourceGenerator;

/// <summary>Represents a symbol.</summary>
public readonly record struct Symbol
{
    /// <summary>Gets the marker attribute attached to this symbol.</summary>
    public MarkerAttributeData MarkerAttribute { get; }

    /// <summary>Gets a read-only list of the declarations that contain this symbol.
    /// <remarks>The list is populated in order, from the outer declaration furthest away to the symbol towards the inner declaration closest to the symbol.</remarks>
    /// </summary>
    public EquatableReadOnlyList<Declaration> ContainingDeclarations { get; }

    /// <summary>Gets the type of symbol.</summary>
    public SymbolType SymbolType { get; }

    /// <summary>Gets the name of the symbol.</summary>
    public string Name { get; }

    /// <summary>Gets a read-only list of generic parameters for generic symbols, or an empty list otherwise.</summary>
    public EquatableReadOnlyList<string> GenericTypeParameters { get; }

    /// <summary>Gets a read-only list of method parameters for method symbols, or an empty list otherwise.</summary>
    public EquatableReadOnlyList<MethodParameter> MethodParameters { get; }

    /// <summary>Gets the return type for method symbols, or an empty string otherwise.</summary>
    public string ReturnType { get; }

    /// <summary>Gets the full name of the symbol.</summary>
    public string FullyQualifiedName
    {
        get
        {
            var name = GenericTypeParameters.Count <= 0 ? Name : $"{Name}`{GenericTypeParameters.Count}";

            if (ContainingDeclarations.Count <= 0)
                return name;

            var path = ContainingDeclarations.ToFullyQualifiedName();

            return $"{path}.{name}";
        }
    }

    /// <summary>Gets the namespace of the symbol.</summary>
    public string Namespace
    {
        get
        {
            if (ContainingDeclarations.Count <= 0)
                return "";

            var ns = ContainingDeclarations.ToNamespace();

            return ns;
        }
    }

    /// <summary>Initializes a new instance of the <see cref="Symbol" /> record with the specified marker attribute, containing declarations, symbol type, name, and generic parameters.</summary>
    /// <param name="markerAttribute">The marker attribute attached to this symbol.</param>
    /// <param name="containingDeclarations">The read-only list of the declarations that contain this symbol.</param>
    /// <param name="symbolType">The type of symbol.</param>
    /// <param name="name">The name of the symbol.</param>
    /// <param name="genericTypeParameters">The list of the generic parameters for the symbol.</param>
    /// <param name="methodParameters">The list of the method parameters for the symbol.</param>
    /// <param name="returnType">The return type for the symbol.</param>
    internal Symbol(MarkerAttributeData markerAttribute, EquatableReadOnlyList<Declaration> containingDeclarations, SymbolType symbolType, string name, EquatableReadOnlyList<string> genericTypeParameters,
        EquatableReadOnlyList<MethodParameter> methodParameters, string returnType)
    {
        MarkerAttribute = markerAttribute;
        ContainingDeclarations = containingDeclarations;
        SymbolType = symbolType;
        Name = name;
        GenericTypeParameters = genericTypeParameters;
        MethodParameters = methodParameters;
        ReturnType = returnType;
    }

    /// <summary>Deconstructs the symbol into its constituent parts.</summary>
    /// <param name="markerAttribute">Receives the marker attribute attached to this symbol.</param>
    /// <param name="containingDeclarations">Receives the read-only list of the declarations that contain this symbol.</param>
    /// <param name="symbolType">The type of symbol.</param>
    /// <param name="name">Receives the name of the symbol.</param>
    /// <param name="genericParameters">Receives the read-only list of the generic parameters for the symbol.</param>
    /// <param name="methodParameters">Receives the read-only list of the method parameters for the symbol.</param>
    /// <param name="returnType">Receives the return type for the symbol.</param>
    public void Deconstruct(out MarkerAttributeData markerAttribute, out EquatableReadOnlyList<Declaration> containingDeclarations, out SymbolType symbolType, out string name, out EquatableReadOnlyList<string> genericParameters,
        out EquatableReadOnlyList<MethodParameter> methodParameters, out string returnType)
    {
        markerAttribute = MarkerAttribute;
        containingDeclarations = ContainingDeclarations;
        symbolType = SymbolType;
        name = Name;
        genericParameters = GenericTypeParameters;
        methodParameters = MethodParameters;
        returnType = ReturnType;
    }
}