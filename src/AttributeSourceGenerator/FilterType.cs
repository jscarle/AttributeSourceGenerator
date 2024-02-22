namespace AttributeSourceGenerator;

/// <summary>Specifies the kind of filter.</summary>
public enum FilterType
{
    /// <summary>Do not filter.</summary>
    None = 0,

    /// <summary>Filter for interfaces only.</summary>
    Interface = 1,

    /// <summary>Filter for classes only.</summary>
    Class = 2,

    /// <summary>Filter for records only.</summary>
    Record = 3,

    /// <summary>Filter for structs only.</summary>
    Struct = 4,

    /// <summary>Filter for record structs only.</summary>
    RecordStruct = 5,

    /// <summary>Filter for methods only.</summary>
    Method = 6
}
