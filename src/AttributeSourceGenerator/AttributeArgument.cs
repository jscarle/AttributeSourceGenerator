// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator;

/// <summary>Represents an attribute argument.</summary>
public readonly record struct AttributeArgument
{
    /// <summary>Gets the type of argument.</summary>
    public AttributeArgumentType ArgumentType { get; }

    /// <summary>Gets the name of the argument.</summary>
    public string Name { get; }

    /// <summary>Gets the value of the argument.</summary>
    public string? Value { get; }

    /// <summary>Initializes a new instance of the <see cref="AttributeArgument" /> record with the specified type, name, and value.</summary>
    /// <param name="argumentType">The type of argument.</param>
    /// <param name="name">The name of the argument.</param>
    /// <param name="value">The value of the argument.</param>
    internal AttributeArgument(AttributeArgumentType argumentType, string name, string? value)
    {
        ArgumentType = argumentType;
        Name = name;
        Value = value;
    }
}