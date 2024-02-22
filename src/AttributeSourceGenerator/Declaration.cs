using AttributeSourceGenerator.Common;

namespace AttributeSourceGenerator;

/// <summary>Represents a declaration with information about its type, name, and generic parameters.</summary>
public readonly record struct Declaration
{
    /// <summary>Gets the type of declaration.</summary>
    public DeclarationType DeclarationType { get; }

    /// <summary>Gets the name of the declaration.</summary>
    public string Name { get; }

    /// <summary>Gets a read-only list of generic parameter names for generic declarations, or an empty list otherwise.</summary>
    public EquatableReadOnlyList<string> GenericParameters { get; }

    /// <summary>Initializes a new instance of the <see cref="Declaration" /> class with the specified type, name, and generic parameters.</summary>
    /// <param name="declarationType">The type of declaration.</param>
    /// <param name="name">The name of the declaration.</param>
    /// <param name="genericParameters">A read-only list of generic parameter names, or an empty list if not generic.</param>
    internal Declaration(DeclarationType declarationType, string name, EquatableReadOnlyList<string> genericParameters)
    {
        DeclarationType = declarationType;
        Name = name;
        GenericParameters = genericParameters;
    }

    /// <summary>Returns a string representation of the declaration in the appropriate format for its type.</summary>
    /// <returns>A string representation of the declaration.</returns>
    public override string ToString()
    {
        switch (DeclarationType)
        {
            case DeclarationType.Namespace:
                return $"namespace {Name}";
            case DeclarationType.Interface:
            case DeclarationType.Class:
            case DeclarationType.Record:
            case DeclarationType.Struct:
            case DeclarationType.RecordStruct:
                if (GenericParameters.Count > 0)
                    return $"partial {Name}<{string.Join(", ", GenericParameters)}>";
                return $"partial {Name}";
            default:
                return base.ToString();
        }
    }

    /// <summary>Deconstructs the declaration into its constituent parts.</summary>
    /// <param name="declarationType">Receives the declaration type.</param>
    /// <param name="name">Receives the name of the declaration.</param>
    /// <param name="genericParameters">Receives the read-only list of generic parameter names (or empty list if not generic).</param>
    public void Deconstruct(out DeclarationType declarationType, out string name, out EquatableReadOnlyList<string> genericParameters)
    {
        declarationType = DeclarationType;
        name = Name;
        genericParameters = GenericParameters;
    }
}
