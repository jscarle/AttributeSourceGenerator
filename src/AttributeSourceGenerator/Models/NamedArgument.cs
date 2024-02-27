namespace AttributeSourceGenerator.Models;

/// <summary>Represents a named argument.</summary>
public readonly record struct NamedArgument
{
    /// <summary>Gets the type of the argument.</summary>
    public string Type { get; }

    /// <summary>Gets the name of the argument.</summary>
    public string Name { get; }

    /// <summary>Gets the value of the argument.</summary>
    public string? Value { get; }

    /// <summary>Initializes a new instance of the <see cref="NamedArgument" /> record with the specified type, name, and value.</summary>
    /// <param name="type">The type of the argument.</param>
    /// <param name="name">The name of the argument.</param>
    /// <param name="value">The value of the argument.</param>
    internal NamedArgument(string type, string name, string? value)
    {
        Type = type;
        Name = name;
        Value = value;
    }
}