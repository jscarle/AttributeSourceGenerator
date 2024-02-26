using AttributeSourceGenerator.Models;
using Microsoft.CodeAnalysis;

namespace AttributeSourceGenerator.Common;

/// <summary>Provides extension methods for working with GeneratorAttributeSyntaxContext.</summary>
internal static class GeneratorAttributeSyntaxContextExtensions
{
    /// <summary>Gets the marker attribute data for the given context.</summary>
    /// <param name="context">The <see cref="GeneratorAttributeSyntaxContext" /> to get the marker attribute for.</param>
    /// <returns>The <see cref="MarkerAttributeData" /> representing the marker attribute.</returns>
    public static MarkerAttributeData GetMarkerAttribute(this GeneratorAttributeSyntaxContext context)
    {
        var attribute = context.Attributes.First();

        var attributeClass = attribute.AttributeClass;
        if (attributeClass is null)
            throw new InvalidOperationException(
                $"{nameof(AttributeIncrementalGeneratorBase)} unexpectedly found that {nameof(AttributeData.AttributeClass)} was null while transforming a {nameof(GeneratorAttributeSyntaxContext.TargetSymbol)}.");

        var attributeName = attributeClass.Name;
        var genericTypeArguments = attribute.GetGenericTypeArguments();
        var constructorArguments = attribute.GetConstructorArguments();
        var namedArguments = attribute.GetNamedArguments();
        var markerAttributeData = new MarkerAttributeData(attributeName, genericTypeArguments, constructorArguments, namedArguments);

        return markerAttributeData;
    }


    /// <summary>Gets a list of generic type arguments for the given attribute.</summary>
    /// <param name="attribute">The <see cref="AttributeData" /> to get the generic type arguments for.</param>
    /// <returns>A list of <see cref="GenericTypeArgument" /> representing the generic type arguments.</returns>
    private static EquatableReadOnlyList<GenericTypeArgument> GetGenericTypeArguments(this AttributeData attribute)
    {
        var attributeClass = attribute.AttributeClass;
        if (attributeClass is null)
            throw new InvalidOperationException(
                $"{nameof(AttributeIncrementalGeneratorBase)} unexpectedly found that {nameof(AttributeData.AttributeClass)} was null while transforming a {nameof(GeneratorAttributeSyntaxContext.TargetSymbol)}.");

        if (!attributeClass.IsGenericType)
            return EquatableReadOnlyList<GenericTypeArgument>.Empty;

        var genericTypeArguments = new List<GenericTypeArgument>();

        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < attributeClass.TypeParameters.Length; index++)
        {
            var typeParameterSymbol = attributeClass.TypeParameters[index];
            var name = typeParameterSymbol.Name;
            var value = attributeClass.TypeArguments[index].ToDisplayString();
            var genericTypeArgument = new GenericTypeArgument(name, value);

            genericTypeArguments.Add(genericTypeArgument);
        }

        return genericTypeArguments.ToEquatableReadOnlyList();
    }

    /// <summary>Gets a list of constructor arguments for the given attribute.</summary>
    /// <param name="attribute">The <see cref="AttributeData" /> to get the constructor arguments for.</param>
    /// <returns>A list of <see cref="ConstructorArgument" /> representing the constructor arguments.</returns>
    private static EquatableReadOnlyList<ConstructorArgument> GetConstructorArguments(this AttributeData attribute)
    {
        var attributeConstructor = attribute.AttributeConstructor;
        if (attributeConstructor is null)
            throw new InvalidOperationException(
                $"{nameof(AttributeIncrementalGeneratorBase)} unexpectedly found that {nameof(AttributeData.AttributeConstructor)} was null while transforming a {nameof(GeneratorAttributeSyntaxContext.TargetSymbol)}.");

        if (attributeConstructor.Parameters.Length <= 0)
            return EquatableReadOnlyList<ConstructorArgument>.Empty;

        var constructorArguments = new List<ConstructorArgument>();

        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < attributeConstructor.Parameters.Length; index++)
        {
            var constructorParameterSymbol = attributeConstructor.Parameters[index];
            var constructorArgumentSymbol = attribute.ConstructorArguments[index];
            var type = constructorParameterSymbol.Type.ToDisplayString();
            var name = constructorParameterSymbol.Name;
            var value = constructorArgumentSymbol.Value?.ToString();
            var constructorArgument = new ConstructorArgument(type, name, value);

            constructorArguments.Add(constructorArgument);
        }

        return constructorArguments.ToEquatableReadOnlyList();
    }

    /// <summary>Gets a list of named arguments for the given attribute.</summary>
    /// <param name="attribute">The <see cref="AttributeData" /> to get the named arguments for.</param>
    /// <returns>A list of <see cref="NamedArgument" /> representing the named arguments.</returns>
    private static EquatableReadOnlyList<NamedArgument> GetNamedArguments(this AttributeData attribute)
    {
        if (attribute.NamedArguments.Length <= 0)
            return EquatableReadOnlyList<NamedArgument>.Empty;

        var namedArguments = new List<NamedArgument>();

        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < attribute.NamedArguments.Length; index++)
        {
            var namedArgumentSymbol = attribute.NamedArguments[index].Value;
            var type = namedArgumentSymbol.Type?.ToDisplayString() ?? "";
            var name = attribute.NamedArguments[index].Key;
            var value = namedArgumentSymbol.Value?.ToString();
            var namedArgument = new NamedArgument(type, name, value);

            namedArguments.Add(namedArgument);
        }

        return namedArguments.ToEquatableReadOnlyList();
    }
}