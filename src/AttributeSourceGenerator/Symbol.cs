using AttributeSourceGenerator.Common;

namespace AttributeSourceGenerator;

/// <summary>Represents a symbol in a codebase.</summary>
public readonly record struct Symbol
{
    /// <summary>Gets the name of the symbol.</summary>
    public string Name { get; }

    /// <summary>Gets the full name of the symbol.</summary>
    public string FullName
    {
        get
        {
            if (ContainingDeclarations.Count <= 0)
                return Name;

            var names = ContainingDeclarations.Select(declaration => declaration.Name).ToArray();
            return $"{string.Join(".", names)}.{Name}";
        }
    }

    /// <summary>Gets a read-only list of the declarations that contain this symbol.
    /// <remarks>The list is populated in order, from the outer declaration furthest away to the symbol towards the inner declaration closest to the symbol.</remarks>
    /// </summary>
    public EquatableReadOnlyList<Declaration> ContainingDeclarations { get; init; }

    /// <summary>Initializes a new instance of the <see cref="Symbol" /> record with the specified name and containing declarations.</summary>
    /// <param name="name">The name of the symbol.</param>
    /// <param name="containingDeclarations">The declarations that contain this symbol.</param>
    internal Symbol(string name, EquatableReadOnlyList<Declaration> containingDeclarations)
    {
        Name = name;
        ContainingDeclarations = containingDeclarations;
    }

    /// <summary>Deconstructs the symbol into its constituent parts.</summary>
    /// <param name="name">Receives the name of the symbol.</param>
    /// <param name="containingDeclarations">Receives the read-only list of the containing declarations.</param>
    public void Deconstruct(out string name, out EquatableReadOnlyList<Declaration> containingDeclarations)
    {
        containingDeclarations = ContainingDeclarations;
        name = Name;
    }
}
