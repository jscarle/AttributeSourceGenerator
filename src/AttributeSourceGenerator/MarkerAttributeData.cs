using AttributeSourceGenerator.Common;

// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator;

/// <summary>Represents an attribute.</summary>
public readonly record struct MarkerAttributeData
{
    /// <summary>Gets the name of the attribute.</summary>
    public string Name { get; }

    /// <summary>Gets a read-only list of generic arguments for generic attributes, or an empty list otherwise.</summary>
    public EquatableReadOnlyList<string> GenericArguments { get; }

    /// <summary>Gets a read-only list of values of the attribute, or an empty list otherwise.</summary>
    public EquatableReadOnlyList<AttributeValue> Values { get; }

    /// <summary>Initializes a new instance of the <see cref="MarkerAttributeData" /> record with the specified name, generic arguments, and values.</summary>
    /// <param name="name">The name of the attribute.</param>
    /// <param name="genericArguments">The list of the generic arguments for the attribute.</param>
    /// <param name="values">The list of the values of the attribute.</param>
    internal MarkerAttributeData(string name, EquatableReadOnlyList<string> genericArguments, EquatableReadOnlyList<AttributeValue> values)
    {
        Name = name;
        GenericArguments = genericArguments;
        Values = values;
    }

    /// <summary>Deconstructs the attribute into its constituent parts.</summary>
    /// <param name="name">Receives the name of the attribute.</param>
    /// <param name="genericArguments">Receives the read-only list of the generic arguments for the attribute.</param>
    /// <param name="values">The read-only list of the values of the attribute.</param>
    public void Deconstruct(out string name, out EquatableReadOnlyList<string> genericArguments, out EquatableReadOnlyList<AttributeValue> values)
    {
        name = Name;
        genericArguments = GenericArguments;
        values = Values;
    }
}
