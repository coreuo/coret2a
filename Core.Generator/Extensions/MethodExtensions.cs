using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Extensions
{
    public static class MethodExtensions
    {
        public static IEnumerable<IMethodSymbol> GetNestedMethods(this INamedTypeSymbol type)
        {
            foreach (var method in type.GetMembers().OfType<IMethodSymbol>().Where(m => m.DeclaredAccessibility == Accessibility.Public))
            {
                yield return method;
            }

            foreach (var @interface in type.AllInterfaces)
            {
                foreach (var method in @interface.GetMembers().OfType<IMethodSymbol>().Where(m => m.DeclaredAccessibility == Accessibility.Public))
                {
                    yield return method;
                }
            }
        }

        public static string ResolveEntityMethodHandlerDefinition(this IMethodSymbol method, string rootType, string rootName)
        {
            var parameters = new[]{$"{rootType} {rootName}"}.Concat(method.Parameters.Select(p => $"{p.Type} {p.Name}"));

            var arguments = method.Parameters.Select(p => $"{p.Name}");

            var attributes = method.GetAttributes().Select(a => $@"
    [{a}]");

            return $@"{string.Join(string.Empty, attributes)}
    public static {method.ReturnType} {method.Name}({string.Join(", ", parameters)})
    {{
        {(method.ReturnsVoid ? string.Empty : "return ")}{rootName}.{method.Name}({string.Join(", ", arguments)});
    }}";
        }
    }
}
