// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator;

/// <summary>Specifies the source of the attribute value.</summary>
public enum AttributeValueSource
{
    /// <summary>Represents a value obtained as a constructor argument.</summary>
    Constructor = 0,

    /// <summary>Represents a value obtained as a named argument.</summary>
    Named = 1
}
