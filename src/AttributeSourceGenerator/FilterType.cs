namespace AttributeSourceGenerator;

/// <summary>Specifies the kind of filter.</summary>
[Flags]
public enum FilterType
{
    /// <summary>Do not filter.</summary>
    None = 0,

    /// <summary>Only filter for attributes that are attached to interfaces.</summary>
    Interface = 1,

    /// <summary>Only filter for attributes that are attached to classes.</summary>
    Class = 2,

    /// <summary>Only filter for attributes that are attached to records.</summary>
    Record = 4,

    /// <summary>Only filter for attributes that are attached to structs.</summary>
    Struct = 8,

    /// <summary>Only filter for attributes that are attached to record structs.</summary>
    RecordStruct = 16,

    /// <summary>Only filter for attributes that are attached to methods.</summary>
    Method = 32,

    /// <summary>Filter for all supported attributes.</summary>
    All = Interface | Class | Record | Struct | RecordStruct | Method
}