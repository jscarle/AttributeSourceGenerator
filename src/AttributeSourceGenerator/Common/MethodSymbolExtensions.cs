using AttributeSourceGenerator.Models;
using Microsoft.CodeAnalysis;

namespace AttributeSourceGenerator.Common;

/// <summary>Provides extension methods for working with method symbols.</summary>
internal static class MethodSymbolExtensions
{
    /// <summary>Gets the generic type parameters for the given method symbol.</summary>
    /// <param name="symbol">The <see cref="IMethodSymbol" /> to get the type parameters for.</param>
    /// <returns>An <see cref="EquatableReadOnlyList{T}" /> of strings representing the generic type parameter names.</returns>
    public static EquatableReadOnlyList<string> GetGenericTypeParameters(this IMethodSymbol symbol)
    {
        if (!symbol.IsGenericMethod)
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

    /// <summary>Gets a list of constructor parameters for the given method symbol.</summary>
    /// <param name="symbol">The <see cref="IMethodSymbol" /> to get the type parameters for.</param>
    /// <returns>An <see cref="EquatableReadOnlyList{T}" /> of strings representing the constructor parameters.</returns>
    public static EquatableReadOnlyList<ConstructorParameter> GetConstructorParameters(this IMethodSymbol symbol)
    {
        if (symbol.Parameters.Length <= 0)
            return EquatableReadOnlyList<ConstructorParameter>.Empty;

        var constructorParameters = new List<ConstructorParameter>();

        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < symbol.Parameters.Length; index++)
        {
            var constructorParameterSymbol = symbol.Parameters[index];
            var type = constructorParameterSymbol.Type.ToDisplayString();
            var name = constructorParameterSymbol.Name;
            var constructorParameter = new ConstructorParameter(type, name);

            constructorParameters.Add(constructorParameter);
        }

        return constructorParameters.ToEquatableReadOnlyList();
    }
}