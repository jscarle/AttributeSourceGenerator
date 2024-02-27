using Microsoft.CodeAnalysis;

namespace AttributeSourceGenerator.Common;

/// <summary>Provides extension methods for working with type symbols.</summary>
internal static class TypeSymbolExtensions
{
    /// <summary>Gets the <see cref="DeclarationType" /> for the given <see cref="ITypeSymbol" /> based on its type kind.</summary>
    /// <param name="symbol">The <see cref="ITypeSymbol" /> to get the <see cref="DeclarationType" /> for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="DeclarationType" /> if the symbol can be mapped to a declaration type, otherwise null.</returns>
    public static DeclarationType? GetDeclarationType(this ITypeSymbol symbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return symbol switch
        {
            { IsReferenceType: true, TypeKind: TypeKind.Interface } => DeclarationType.Interface,
            { IsReferenceType: true, IsRecord: true } => DeclarationType.Record,
            { IsReferenceType: true } => DeclarationType.Class,
            { IsValueType: true, IsRecord: true } => DeclarationType.RecordStruct,
            { IsValueType: true } => DeclarationType.Struct,
            _ => null
        };
    }
}