using AttributeSourceGenerator.Common;

namespace AttributeSourceGenerator.Models;

/// <summary>Represents a marker attribute.</summary>
public readonly record struct MarkerAttributeData
{
    /// <summary>Gets the name of the attribute.</summary>
    public string Name { get; }

    /// <summary>Gets a read-only list of the generic type arguments for the attribute, or an empty list otherwise.</summary>
    public EquatableReadOnlyList<GenericTypeArgument> GenericTypeArguments { get; }

    /// <summary>Gets a read-only list of the constructor arguments for the attribute, or an empty list otherwise.</summary>
    public EquatableReadOnlyList<ConstructorArgument> ConstructorArguments { get; }

    /// <summary>Gets a read-only list of the named arguments for the attribute, or an empty list otherwise.</summary>
    public EquatableReadOnlyList<NamedArgument> NamedArguments { get; }

    /// <summary>Initializes a new instance of the <see cref="MarkerAttributeData" /> record with the specified name, generic type arguments, constructor arguments, and named arguments.</summary>
    /// <param name="name">The name of the attribute.</param>
    /// <param name="genericTypeArguments">The list of generic type arguments for the attribute.</param>
    /// <param name="constructorArguments">The list of constructor arguments for the attribute.</param>
    /// <param name="namedArguments">The list of named arguments for the attribute.</param>
    internal MarkerAttributeData(string name, EquatableReadOnlyList<GenericTypeArgument> genericTypeArguments, EquatableReadOnlyList<ConstructorArgument> constructorArguments, EquatableReadOnlyList<NamedArgument> namedArguments)
    {
        Name = name;
        GenericTypeArguments = genericTypeArguments;
        ConstructorArguments = constructorArguments;
        NamedArguments = namedArguments;
    }

    /// <summary>Deconstructs the attribute into its constituent parts.</summary>
    /// <param name="name">Receives the name of the attribute.</param>
    /// <param name="genericTypeArguments">The list of generic type arguments for the attribute.</param>
    /// <param name="constructorArguments">The list of constructor arguments for the attribute.</param>
    /// <param name="namedArguments">The list of named arguments for the attribute.</param>
    public void Deconstruct(out string name, out EquatableReadOnlyList<GenericTypeArgument> genericTypeArguments, out EquatableReadOnlyList<ConstructorArgument> constructorArguments, out EquatableReadOnlyList<NamedArgument> namedArguments)
    {
        name = Name;
        genericTypeArguments = GenericTypeArguments;
        constructorArguments = ConstructorArguments;
        namedArguments = NamedArguments;
    }
}