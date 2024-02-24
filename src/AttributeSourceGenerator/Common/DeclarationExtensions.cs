using System.Text;

// ReSharper disable CheckNamespace

namespace AttributeSourceGenerator.Common;

internal static class DeclarationExtensions
{
    public static string ToNamespace(this EquatableReadOnlyList<Declaration> declarations)
    {
        var builder = new StringBuilder();

        // ReSharper disable once ForCanBeConvertedToForeach
        for (var index = 0; index < declarations.Count; index++)
        {
            var declaration = declarations[index];
            if (declaration.DeclarationType != DeclarationType.Namespace)
                continue;

            if (builder.Length > 0)
                builder.Append('.');
            builder.Append(declaration.Name);
        }

        return builder.ToString();
    }

    public static string ToFullyQualifiedName(this EquatableReadOnlyList<Declaration> declarations)
    {
        var builder = new StringBuilder();

        // ReSharper disable once ForCanBeConvertedToForeach
        for (var index = 0; index < declarations.Count; index++)
        {
            var declaration = declarations[index];

            if (builder.Length > 0)
                builder.Append('.');
            builder.Append(declaration.Name);

            if (declaration.GenericParameters.Count <= 0)
                continue;

            builder.Append('`');
            builder.Append(declaration.GenericParameters.Count);
        }

        return builder.ToString();
    }
}
