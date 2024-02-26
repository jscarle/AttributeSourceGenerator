// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator.Models;

/// <summary>Represents a constructor parameter</summary>
public readonly record struct ConstructorParameter
{
    /// <summary>Gets the type of the parameter.</summary>
    public string Type { get; }

    /// <summary>Gets the name of the parameter.</summary>
    public string Name { get; }

    /// <summary>Initializes a new instance of the <see cref="ConstructorParameter" /> record with the specified type and name.</summary>
    /// <param name="type">The type of the argument.</param>
    /// <param name="name">The name of the argument.</param>
    internal ConstructorParameter(string type, string name)
    {
        Type = type;
        Name = name;
    }
}