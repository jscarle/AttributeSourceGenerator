// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator;

/// <summary>Specifies the type of symbol.</summary>
public enum SymbolType
{
    /// <summary>Represents an unknown symbol.</summary>
    Unknown = 0,

    /// <summary>Represents an interface symbol.</summary>
    Interface = 1,

    /// <summary>Represents a class symbol.</summary>
    Class = 2,

    /// <summary>Represents a record symbol.</summary>
    Record = 3,

    /// <summary>Represents a struct symbol.</summary>
    Struct = 4,

    /// <summary>Represents a record struct symbol.</summary>
    RecordStruct = 5,

    /// <summary>Represents a method symbol.</summary>
    Method = 6
}