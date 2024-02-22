namespace AttributeSourceGenerator;

/// <summary>Represents source.</summary>
public readonly record struct Source
{
    /// <summary>Gets the name of the source.
    /// <remarks>'.g.cs' will be appended to this name.</remarks>
    /// </summary>
    public string Name { get; }

    /// <summary>Gets the text of the source.</summary>
    public string Text { get; }

    /// <summary>Initializes a new instance of the <see cref="Source" /> record with the specified name and text.</summary>
    /// <param name="name">The name of the source.</param>
    /// <param name="text">The text of the source.</param>
    public Source(string name, string text)
    {
        Name = name;
        Text = text;
    }
}
