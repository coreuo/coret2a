using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;

namespace Core.Generator.Extensions
{
    public static class HandlerExtensions
    {
        public static string ResolveHandlers(this INamedTypeSymbol @interface, string name, ImmutableDictionary<ISymbol, string> genericDictionary)
        {
            var root = @interface.Name.Substring(1, @interface.Name.Length - 1);

            var types = new[] { name }.Concat(@interface.TypeParameters.Select(genericDictionary.ResolveType));

            return $"{@interface.ContainingNamespace}.{root}Handlers<{string.Join(", ", types)}>";
        }
    }
}
