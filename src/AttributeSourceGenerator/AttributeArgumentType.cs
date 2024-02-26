// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator;

/// <summary>Specifies the type of the attribute argument.</summary>
public enum AttributeArgumentType
{
    /// <summary>Represents a generic type argument.</summary>
    GenericType = 0,

    /// <summary>Represents a constructor argument.</summary>
    Constructor = 1,

    /// <summary>Represents a named argument.</summary>
    Named = 2
}
