namespace AttributeSourceGenerator;

/// <summary>Defines the configuration for an incremental attribute generator.</summary>
public sealed class AttributeIncrementalGeneratorConfiguration
{
    /// <summary>The fully qualified name of the marker attribute.</summary>
    public required string MarkerAttributeName { get; init; }

    /// <summary>The source for the marker attribute.</summary>
    public Source? MarkerAttributeSource { get; init; }

    /// <summary>The filter to apply to symbols.</summary>
    public FilterType SymbolFilter { get; init; } = FilterType.All;

    /// <summary>The function that generates the source code for the attribute.</summary>
    public required Func<Symbol, IEnumerable<Source>> SourceGenerator { get; init; }

    /// <summary>Initializes a new instance of the <see cref="AttributeIncrementalGeneratorConfiguration" /> class</summary>
    public AttributeIncrementalGeneratorConfiguration()
    {
    }
}