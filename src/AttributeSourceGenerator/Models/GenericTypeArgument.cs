namespace AttributeSourceGenerator.Models;

/// <summary>Represents a generic type argument.</summary>
public readonly record struct GenericTypeArgument
{
    /// <summary>Gets the name of the argument.</summary>
    public string Name { get; }

    /// <summary>Gets the value of the argument.</summary>
    public string? Value { get; }

    /// <summary>Initializes a new instance of the <see cref="GenericTypeArgument" /> record with the specified name and value.</summary>
    /// <param name="name">The name of the argument.</param>
    /// <param name="value">The value of the argument.</param>
    internal GenericTypeArgument(string name, string? value)
    {
        Name = name;
        Value = value;
    }
}