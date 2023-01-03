using System.Collections.Immutable;
using System.Linq;
using Core.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain
{
    public readonly struct ResolvedMethod
    {
        public string Name { get; }

        public string Returns { get; }

        public string Parameters { get; }

        public ResolvedMethod(IMethodSymbol method, ImmutableDictionary<ISymbol, string> genericDictionary, bool handler)
        {
            Name = handler ? method.Name : $"On{method.Name}";

            Returns = $"{method.ReturnType}";

            Parameters = string.Join(", ", method.TypeParameters.Select(genericDictionary.ResolveType));
        }

        public override string ToString()
        {
            return $"{Returns} {Name}({Parameters});";
        }
    }
}
