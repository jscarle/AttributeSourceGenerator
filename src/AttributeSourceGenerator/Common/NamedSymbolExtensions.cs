using Microsoft.CodeAnalysis;

namespace AttributeSourceGenerator.Common;

/// <summary>Provides extension methods for working with named symbols.</summary>
internal static class NamedSymbolExtensions
{
    /// <summary>Gets the generic type parameters for the given named type symbol.</summary>
    /// <param name="symbol">The <see cref="INamedTypeSymbol" /> to get the type parameters for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="EquatableReadOnlyList{T}" /> of strings representing the generic type parameter names.</returns>
    public static EquatableReadOnlyList<string> GetGenericTypeParameters(this INamedTypeSymbol symbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!symbol.IsGenericType)
            return EquatableReadOnlyList<string>.Empty;

        var genericTypeParameters = new List<string>();

        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < symbol.TypeParameters.Length; index++)
        {
            var typeParameter = symbol.TypeParameters[index];
            genericTypeParameters.Add(typeParameter.Name);
        }

        return genericTypeParameters.ToEquatableReadOnlyList();
    }
}