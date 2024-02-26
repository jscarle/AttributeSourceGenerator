using Microsoft.CodeAnalysis;

namespace AttributeSourceGenerator.Common;

/// <summary>Provides extension methods for working with type symbols.</summary>
internal static class TypeSymbolExtensions
{
    /// <summary>Gets the <see cref="DeclarationType" /> for the given <see cref="ITypeSymbol" /> based on its type kind.</summary>
    /// <param name="symbol">The <see cref="ITypeSymbol" /> to get the <see cref="DeclarationType" /> for.</param>
    /// <returns>A <see cref="DeclarationType" /> if the symbol can be mapped to a declaration type, otherwise null.</returns>
    public static DeclarationType? GetDeclarationType(this ITypeSymbol symbol)
    {
        if (symbol.IsReferenceType)
        {
            if (symbol.TypeKind == TypeKind.Interface)
                return DeclarationType.Interface;
            if (symbol.IsRecord)
                return DeclarationType.Record;
            return DeclarationType.Class;
        }

        if (symbol.IsValueType)
        {
            if (symbol.IsRecord)
                return DeclarationType.RecordStruct;
            return DeclarationType.Struct;
        }

        return null;
    }
}