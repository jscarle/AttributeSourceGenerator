namespace AttributeSourceGenerator;

/// <summary>Defines the <see cref="AttributeIncrementalGeneratorBase" /> project-level options.</summary>
public sealed record AttributeIncrementalGeneratorOptions
{
    public bool IncludeMarkerAttributeSource { get; init; }
}
