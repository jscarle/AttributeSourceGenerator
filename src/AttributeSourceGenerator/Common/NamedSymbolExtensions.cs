using Microsoft.CodeAnalysis;

// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator.Common;

internal static class NamedSymbolExtensions
{
    public static EquatableReadOnlyList<string> GetGenericTypeParameters(this INamedTypeSymbol symbol)
    {
        if (!symbol.IsGenericType)
            return EquatableReadOnlyList<string>.Empty;
        
        var typeParameters = new List<string>();
        
        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < symbol.TypeParameters.Length; index++)
        {
            var typeParameter = symbol.TypeParameters[index];
            typeParameters.Add(typeParameter.Name);
        }

        var genericTypeParameters = new EquatableReadOnlyList<string>(typeParameters);
        return genericTypeParameters;
    }

    public static EquatableReadOnlyList<string> GetGenericTypeArguments(this INamedTypeSymbol symbol)
    {
        if (!symbol.IsGenericType)
            return EquatableReadOnlyList<string>.Empty;
        
        var typeArguments = new List<string>();
        
        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < symbol.TypeArguments.Length; index++)
        {
            var typeArgument = symbol.TypeArguments[index];
            typeArguments.Add(typeArgument.ToDisplayString());
        }

        var genericTypeArguments = new EquatableReadOnlyList<string>(typeArguments);
        return genericTypeArguments;
    }
}
