using Microsoft.CodeAnalysis;

// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator.Common;

/// <summary>Provides extension methods for working with GeneratorAttributeSyntaxContext.</summary>
internal static class GeneratorAttributeSyntaxContextExtensions
{
    /// <summary>Gets the marker attribute data from the given context.</summary>
    /// <param name="context">The <see cref="GeneratorAttributeSyntaxContext" /> to get the marker attribute from.</param>
    /// <returns>The <see cref="MarkerAttributeData" /> representing the marker attribute.</returns>
    public static MarkerAttributeData GetMarkerAttribute(this GeneratorAttributeSyntaxContext context)
    {
        var attribute = context.Attributes.First();

        var attributeClass = attribute.AttributeClass;
        if (attributeClass is null)
            throw new InvalidOperationException(
                $"{nameof(AttributeIncrementalGeneratorBase)} unexpectedly found that {nameof(AttributeData.AttributeClass)} was null while transforming a {nameof(GeneratorAttributeSyntaxContext.TargetSymbol)}.");

        var attributeName = attributeClass.Name;
        var attributeArguments = attribute.GetAttributeArguments();
        var markerAttributeData = new MarkerAttributeData(attributeName, attributeArguments);

        return markerAttributeData;
    }

    /// <summary>Gets a list of attribute arguments from the given attribute.</summary>
    /// <param name="attribute">The <see cref="AttributeData" /> to get the arguments from.</param>
    /// <returns>An <see cref="EquatableReadOnlyList{T}" /> of <see cref="AttributeArgument" /> representing the attribute arguments.</returns>
    private static EquatableReadOnlyList<AttributeArgument> GetAttributeArguments(this AttributeData attribute)
    {
        var attributeArguments = new List<AttributeArgument>();

        var typeArguments = attribute.GetGenericTypeArguments();
        attributeArguments.AddRange(typeArguments);

        var constructorArguments = attribute.GetConstructorArguments();
        attributeArguments.AddRange(constructorArguments);

        var namedArguments = attribute.GetNamedArguments();
        attributeArguments.AddRange(namedArguments);

        var attributeValues = attributeArguments.ToEquatableReadOnlyList();

        return attributeValues;
    }

    /// <summary>Gets a list of generic type arguments from the given attribute.</summary>
    /// <param name="attribute">The <see cref="AttributeData" /> to get the generic type arguments from.</param>
    /// <returns>A list of <see cref="AttributeArgument" /> representing the generic type arguments.</returns>
    private static List<AttributeArgument> GetGenericTypeArguments(this AttributeData attribute)
    {
        var attributeClass = attribute.AttributeClass;
        if (attributeClass is null)
            throw new InvalidOperationException(
                $"{nameof(AttributeIncrementalGeneratorBase)} unexpectedly found that {nameof(AttributeData.AttributeClass)} was null while transforming a {nameof(GeneratorAttributeSyntaxContext.TargetSymbol)}.");

        if (!attributeClass.IsGenericType)
            return [];

        var typeArguments = new List<AttributeArgument>();

        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < attributeClass.TypeParameters.Length; index++)
        {
            var typeParameter = attributeClass.TypeParameters[index];
            var name = typeParameter.Name;
            var value = attributeClass.TypeArguments[index].ToDisplayString();
            var attributeArgument = new AttributeArgument(AttributeArgumentType.GenericType, name, value);

            typeArguments.Add(attributeArgument);
        }

        return typeArguments;
    }

    /// <summary>Gets a list of constructor arguments from the given attribute.</summary>
    /// <param name="attribute">The <see cref="AttributeData" /> to get the constructor arguments from.</param>
    /// <returns>A list of <see cref="AttributeArgument" /> representing the constructor arguments.</returns>
    private static List<AttributeArgument> GetConstructorArguments(this AttributeData attribute)
    {
        var attributeArguments = new List<AttributeArgument>();

        var attributeConstructor = attribute.AttributeConstructor;
        if (attributeConstructor is null)
            throw new InvalidOperationException(
                $"{nameof(AttributeIncrementalGeneratorBase)} unexpectedly found that {nameof(AttributeData.AttributeConstructor)} was null while transforming a {nameof(GeneratorAttributeSyntaxContext.TargetSymbol)}.");

        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < attributeConstructor.Parameters.Length; index++)
        {
            var constructorParameter = attributeConstructor.Parameters[index];
            var name = constructorParameter.Name;
            var value = attribute.ConstructorArguments[index].Value?.ToString();
            var attributeArgument = new AttributeArgument(AttributeArgumentType.Constructor, name, value);

            attributeArguments.Add(attributeArgument);
        }

        return attributeArguments;
    }

    /// <summary>Gets a list of named arguments from the given attribute.</summary>
    /// <param name="attribute">The <see cref="AttributeData" /> to get the named arguments from.</param>
    /// <returns>A list of <see cref="AttributeArgument" /> representing the named arguments.</returns>
    private static List<AttributeArgument> GetNamedArguments(this AttributeData attribute)
    {
        var attributeArguments = new List<AttributeArgument>();

        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < attribute.NamedArguments.Length; index++)
        {
            var namedArgument = attribute.NamedArguments[index];
            var name = namedArgument.Key;
            var value = namedArgument.Value.Value?.ToString();
            var attributeArgument = new AttributeArgument(AttributeArgumentType.Named, name, value);

            attributeArguments.Add(attributeArgument);
        }

        return attributeArguments;
    }
}
