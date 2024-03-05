using AttributeSourceGenerator.Models;
using Microsoft.CodeAnalysis;

namespace AttributeSourceGenerator.Common;

/// <summary>Provides extension methods for working with method symbols.</summary>
internal static class MethodSymbolExtensions
{
    /// <summary>Gets the generic type parameters for the given method symbol.</summary>
    /// <param name="symbol">The <see cref="IMethodSymbol" /> to get the type parameters for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="EquatableReadOnlyList{T}" /> of strings representing the generic type parameter names.</returns>
    public static EquatableReadOnlyList<string> GetGenericTypeParameters(this IMethodSymbol symbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

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

    /// <summary>Gets a list of method parameters for the given method symbol.</summary>
    /// <param name="symbol">The <see cref="IMethodSymbol" /> to get the type parameters for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="EquatableReadOnlyList{T}" /> of strings representing the method parameters.</returns>
    public static EquatableReadOnlyList<MethodParameter> GetMethodParameters(this IMethodSymbol symbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (symbol.Parameters.Length <= 0)
            return EquatableReadOnlyList<MethodParameter>.Empty;

        var methodParameters = new List<MethodParameter>();

        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < symbol.Parameters.Length; index++)
        {
            var methodParameterSymbol = symbol.Parameters[index];
            var type = methodParameterSymbol.Type.ToDisplayString();
            var name = methodParameterSymbol.Name;
            var methodParameter = new MethodParameter(type, name);

            methodParameters.Add(methodParameter);
        }

        return methodParameters.ToEquatableReadOnlyList();
    }
}