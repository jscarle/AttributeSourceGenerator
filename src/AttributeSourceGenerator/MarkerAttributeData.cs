using AttributeSourceGenerator.Common;

// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator;

/// <summary>Represents a marker attribute.</summary>
public readonly record struct MarkerAttributeData
{
    /// <summary>Gets the name of the attribute.</summary>
    public string Name { get; }

    /// <summary>Gets a read-only list of arguments for the attribute, or an empty list otherwise.</summary>
    public EquatableReadOnlyList<AttributeArgument> Arguments { get; }

    /// <summary>Initializes a new instance of the <see cref="MarkerAttributeData" /> record with the specified name, generic arguments, and values.</summary>
    /// <param name="name">The name of the attribute.</param>
    /// <param name="arguments">The list of arguments for the attribute.</param>
    internal MarkerAttributeData(string name, EquatableReadOnlyList<AttributeArgument> arguments)
    {
        Name = name;
        Arguments = arguments;
    }

    /// <summary>Deconstructs the attribute into its constituent parts.</summary>
    /// <param name="name">Receives the name of the attribute.</param>
    /// <param name="arguments">Receives the read-only list of arguments for the attribute.</param>
    public void Deconstruct(out string name, out EquatableReadOnlyList<AttributeArgument> arguments)
    {
        name = Name;
        arguments = Arguments;
    }
}
