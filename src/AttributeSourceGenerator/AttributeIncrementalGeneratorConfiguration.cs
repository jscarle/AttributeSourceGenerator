namespace AttributeSourceGenerator;

/// <summary>Defines the configuration for an incremental attribute generator.</summary>
public sealed class AttributeIncrementalGeneratorConfiguration
{
    /// <summary>The fully qualified name of the attribute.</summary>
    public required string AttributeFullyQualifiedName { get; init; }

    /// <summary>The source for the attribute.</summary>
    public string? AttributeSource { get; init; }

    /// <summary>The filter to apply to symbols.</summary>
    public FilterType SymbolFilter { get; init; } = FilterType.All;

    /// <summary>The function that generates the source code for the attribute.</summary>
    public required Func<Symbol, string> SourceGenerator { get; init; }

    /// <summary>Initializes a new instance of the <see cref="AttributeIncrementalGeneratorConfiguration" /> class</summary>
    public AttributeIncrementalGeneratorConfiguration()
    {
    }
}