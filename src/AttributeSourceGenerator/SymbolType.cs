namespace AttributeSourceGenerator;

/// <summary>Specifies the type of symbol.</summary>
public enum SymbolType
{
    /// <summary>Represents an interface symbol.</summary>
    Interface = 0,

    /// <summary>Represents a class symbol.</summary>
    Class = 1,

    /// <summary>Represents a record symbol.</summary>
    Record = 2,

    /// <summary>Represents a struct symbol.</summary>
    Struct = 3,

    /// <summary>Represents a record struct symbol.</summary>
    RecordStruct = 4,

    /// <summary>Represents a method symbol.</summary>
    Method = 5
}