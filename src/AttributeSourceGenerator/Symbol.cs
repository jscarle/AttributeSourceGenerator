using AttributeSourceGenerator.Common;

// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator;

/// <summary>Represents a symbol.</summary>
public readonly record struct Symbol
{
    /// <summary>Gets the marker attribute attached to this symbol.</summary>
    public MarkerAttributeData MarkerAttribute { get; }
    
    /// <summary>Gets a read-only list of the declarations that contain this symbol.
    /// <remarks>The list is populated in order, from the outer declaration furthest away to the symbol towards the inner declaration closest to the symbol.</remarks>
    /// </summary>
    public EquatableReadOnlyList<Declaration> ContainingDeclarations { get; init; }

    /// <summary>Gets the name of the symbol.</summary>
    public string Name { get; }

    /// <summary>Gets a read-only list of generic parameters for generic symbols, or an empty list otherwise.</summary>
    public EquatableReadOnlyList<string> GenericParameters { get; }

    /// <summary>Gets the full name of the symbol.</summary>
    public string FullyQualifiedName
    {
        get
        {
            var name = GenericParameters.Count <= 0 ? Name : $"{Name}`{GenericParameters.Count}";

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

    /// <summary>Initializes a new instance of the <see cref="Symbol" /> record with the specified marker attribute, containing declarations, name, and generic parameters.</summary>
    /// <param name="markerAttribute">The marker attribute attached to this symbol.</param>
    /// <param name="containingDeclarations">The read-only list of the declarations that contain this symbol.</param>
    /// <param name="name">The name of the symbol.</param>
    /// <param name="genericParameters">The list of the generic parameters for the symbol.</param>
    internal Symbol(MarkerAttributeData markerAttribute, EquatableReadOnlyList<Declaration> containingDeclarations, string name, EquatableReadOnlyList<string> genericParameters)
    {
        MarkerAttribute = markerAttribute;
        ContainingDeclarations = containingDeclarations;
        Name = name;
        GenericParameters = genericParameters;
    }

    /// <summary>Deconstructs the symbol into its constituent parts.</summary>
    /// <param name="markerAttribute">Receives the marker attribute attached to this symbol.</param>
    /// <param name="containingDeclarations">Receives the read-only list of the declarations that contain this symbol.</param>
    /// <param name="name">Receives the name of the symbol.</param>
    /// <param name="genericParameters">Receives the read-only list of the generic parameters for the symbol.</param>
    public void Deconstruct(out MarkerAttributeData markerAttribute, out EquatableReadOnlyList<Declaration> containingDeclarations, out string name, out EquatableReadOnlyList<string> genericParameters)
    {
        markerAttribute = MarkerAttribute;
        containingDeclarations = ContainingDeclarations;
        name = Name;
        genericParameters = GenericParameters;
    }
}
