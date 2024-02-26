using System.Collections;

namespace AttributeSourceGenerator.Common;

/// <summary>A read-only list that implements <see cref="IEquatable{T}" /> for value-based equality comparisons.</summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public readonly struct EquatableReadOnlyList<T> : IEquatable<EquatableReadOnlyList<T>>, IReadOnlyList<T>
{
    /// <summary>Gets an empty <see cref="EquatableReadOnlyList{T}" />.</summary>
    internal static EquatableReadOnlyList<T> Empty { get; } = new([]);

    /// <summary>Gets the number of elements in the list.</summary>
    public int Count => Collection.Count;

    /// <summary>Gets the element at the specified index.</summary>
    /// <param name="index">The index of the element to get.</param>
    /// <returns>The element at the specified index.</returns>
    public T this[int index] => Collection[index];

    private IReadOnlyList<T> Collection => _collection ?? [];
    private readonly IReadOnlyList<T>? _collection;

    /// <summary>Creates a new <see cref="EquatableReadOnlyList{T}" /> from an existing <see cref="IReadOnlyList{T}" />.</summary>
    /// <param name="collection">The <see cref="IReadOnlyList{T}" /> to wrap.</param>
    internal EquatableReadOnlyList(IReadOnlyList<T>? collection)
    {
        _collection = collection;
    }

    /// <summary>Determines whether this instance and another object are equal.</summary>
    /// <param name="other">The object to compare with this instance.</param>
    /// <returns>True if the objects are equal, false otherwise.</returns>
    public bool Equals(EquatableReadOnlyList<T> other)
    {
        return this.SequenceEqual(other);
    }

    /// <summary>Determines whether this instance and a specified object are equal.</summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>True if the objects are equal, false otherwise.</returns>
    public override bool Equals(object? obj)
    {
        return obj is EquatableReadOnlyList<T> other && Equals(other);
    }

    /// <summary>Compares two <see cref="EquatableReadOnlyList{T}" /> objects for equality.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>True if the objects are equal, false otherwise.</returns>
    public static bool operator ==(EquatableReadOnlyList<T> left, EquatableReadOnlyList<T> right)
    {
        return left.Equals(right);
    }

    /// <summary>Compares two <see cref="EquatableReadOnlyList{T}" /> objects for inequality.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>True if the objects are not equal, false otherwise.</returns>
    public static bool operator !=(EquatableReadOnlyList<T> left, EquatableReadOnlyList<T> right)
    {
        return !left.Equals(right);
    }

    /// <summary>Calculates the hash code for this instance.</summary>
    /// <returns>A hash code based on the contents of the list.</returns>
    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        foreach (var item in Collection)
            hashCode.Add(item);

        return hashCode.ToHashCode();
    }

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that iterates through the collection.</returns>
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return Collection.GetEnumerator();
    }

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that iterates through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return Collection.GetEnumerator();
    }
}