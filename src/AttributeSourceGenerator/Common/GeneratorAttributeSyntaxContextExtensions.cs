using Microsoft.CodeAnalysis;

// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator.Common;

internal static class GeneratorAttributeSyntaxContextExtensions
{
    public static MarkerAttributeData GetMarkerAttribute(this GeneratorAttributeSyntaxContext context)
    {
        var attribute = context.Attributes.First();

        var attributeClass = attribute.AttributeClass;
        if (attributeClass is null)
            throw new InvalidOperationException($"{nameof(AttributeIncrementalGeneratorBase)} unexpectedly found that {nameof(attribute.AttributeClass)} was null while transforming a {nameof(context.TargetSymbol)}.");

        var genericTypeArguments = attributeClass.GetGenericTypeArguments();

        var attributeConstructor = attribute.AttributeConstructor;
        if (attributeConstructor is null)
            throw new InvalidOperationException($"{nameof(AttributeIncrementalGeneratorBase)} unexpectedly found that {nameof(attribute.AttributeConstructor)} was null while transforming a {nameof(context.TargetSymbol)}.");

        var argumentValues = new List<AttributeValue>();

        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < attributeConstructor.Parameters.Length; index++)
        {
            var parameter = attributeConstructor.Parameters[index];
            var name = parameter.Name;
            var value = attribute.ConstructorArguments[index].Value;
            var argumentValue = new AttributeValue(AttributeValueSource.Constructor, name, value);
            argumentValues.Add(argumentValue);
        }

        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < attribute.NamedArguments.Length; index++)
        {
            var argument = attribute.NamedArguments[index];
            var name = argument.Key;
            var value = argument.Value.Value;
            var argumentValue = new AttributeValue(AttributeValueSource.Named, name, value);
            argumentValues.Add(argumentValue);
        }

        var attributeValues = argumentValues.ToEquatableReadOnlyList();

        var markerAttributeData = new MarkerAttributeData(attributeClass.Name, genericTypeArguments, attributeValues);
        return markerAttributeData;
    }
}
