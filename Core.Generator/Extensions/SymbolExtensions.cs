using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Extensions
{
    public static class SymbolExtensions
    {
        public static IEnumerable<IAssemblySymbol> GetReferencedAssemblies(this Compilation compilation)
        {
            return compilation.References
                .Select(compilation.GetAssemblyOrModuleSymbol)
                .OfType<IAssemblySymbol>();
        }

        public static IEnumerable<INamedTypeSymbol> FindRecursive(this INamespaceOrTypeSymbol source, Func<INamedTypeSymbol, bool> condition)
        {
            if (source is INamedTypeSymbol target && condition(target)) yield return target;

            foreach (var next in source.GetMembers().OfType<INamespaceOrTypeSymbol>())

            foreach (var recursive in FindRecursive(next, condition)) yield return recursive;
        }

        public static string ResolveTypeParameters(this INamedTypeSymbol symbol, Func<ITypeParameterSymbol, string> resolver)
        {
            return !symbol.TypeParameters.Any() ? $"{symbol}" : $"{symbol.ContainingNamespace}.{symbol.Name}<{string.Join(", ", symbol.TypeParameters.Select(resolver))}>";
        }

        public static string ResolveType(this ImmutableDictionary<ISymbol, string> dictionary, ITypeSymbol type)
        {
            return type.Kind == SymbolKind.TypeParameter ? dictionary[type] : $"{type}";
        }
    }
}
