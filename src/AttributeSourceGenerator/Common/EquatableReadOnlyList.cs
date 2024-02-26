// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator.Common;

/// <summary>Provides extension methods to convert various collections to an <see cref="EquatableReadOnlyList{T}" />.</summary>
internal static class EquatableReadOnlyList
{
    /// <summary>Converts an <see cref="IReadOnlyList{T}" /> to an <see cref="EquatableReadOnlyList{T}" />.</summary>
    /// <param name="list">The <see cref="IReadOnlyList{T}" /> to convert.</param>
    /// <returns>An <see cref="EquatableReadOnlyList{T}" /> containing the same elements as the original list.</returns>
    public static EquatableReadOnlyList<T> ToEquatableReadOnlyList<T>(this IReadOnlyList<T> list)
    {
        return new EquatableReadOnlyList<T>(list);
    }

    /// <summary>Converts an <see cref="IEnumerable{T}" /> to an <see cref="EquatableReadOnlyList{T}" />.</summary>
    /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to convert.</param>
    /// <returns>An <see cref="EquatableReadOnlyList{T}" /> containing the same elements as the original enumerable.</returns>
    public static EquatableReadOnlyList<T> ToEquatableReadOnlyList<T>(this IEnumerable<T> enumerable)
    {
        return new EquatableReadOnlyList<T>(enumerable.ToArray());
    }
}
