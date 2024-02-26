using Microsoft.CodeAnalysis;

// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator.Common;

/// <summary>Provides extension methods for working with named symbols.</summary>
internal static class NamedSymbolExtensions
{
    /// <summary>Gets the generic type parameters of the given named type symbol.</summary>
    /// <param name="symbol">The <see cref="INamedTypeSymbol" /> to get the type parameters from.</param>
    /// <returns>An <see cref="EquatableReadOnlyList{T}" /> of strings representing the type parameter names.</returns>
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
}
