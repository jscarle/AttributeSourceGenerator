// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator;

public readonly record struct AttributeValue
{
    /// <summary>Gets the source of the value.</summary>
    public AttributeValueSource Source { get; }

    /// <summary>Gets the name of the value.</summary>
    public string Name { get; }

    /// <summary>Gets the value.</summary>
    public object? Value { get; }

    /// <summary>Initializes a new instance of the <see cref="AttributeValue" /> record with the specified source, name, and value.</summary>
    /// <param name="source">The source of the value.</param>
    /// <param name="name">The name of the value.</param>
    /// <param name="value">The value.</param>
    public AttributeValue(AttributeValueSource source, string name, object? value)
    {
        Source = source;
        Name = name;
        Value = value;
    }
}
