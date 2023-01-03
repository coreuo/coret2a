using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.Generator.Domain;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Extensions
{
    public static class MethodExtensions
    {
        public static IEnumerable<IMethodSymbol> GetNestedMethods(this INamedTypeSymbol type, bool @abstract)
        {
            foreach (var method in type.GetMembers().OfType<IMethodSymbol>().Where(m => m.DeclaredAccessibility == Accessibility.Public && m.IsAbstract == @abstract))
            {
                yield return method;
            }

            foreach (var @interface in type.AllInterfaces)
            {
                foreach (var method in @interface.GetMembers().OfType<IMethodSymbol>().Where(m => m.DeclaredAccessibility == Accessibility.Public && m.IsAbstract == @abstract))
                {
                    yield return method;
                }
            }
        }

        /*public static (double priority, string call) ResolveEntityMethodHandlerCall(this IMethodSymbol method, string rootType, string rootName, ImmutableDictionary<ISymbol, string> genericDictionary)
        {

        }*/

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

        public static string ResolveEntityMethodDefinition(this IMethodSymbol method, ImmutableDictionary<ISymbol, string> genericDictionary, ImmutableDictionary<ResolvedMethod, ImmutableList<(string, IMethodSymbol)>> callDeclarations)
        {
            return $@"
    public {genericDictionary.ResolveType(method.ReturnType)} {method.Name}({string.Join(", ", method.Parameters.Select(p => $"{genericDictionary.ResolveType(p.Type)} {p.Name}"))})
    {{{ResolveEntityMethodBody(method, genericDictionary, callDeclarations)}
    }}";
        }

        public static string ResolveEntityMethodBody(this IMethodSymbol method, ImmutableDictionary<ISymbol, string> genericDictionary, ImmutableDictionary<ResolvedMethod, ImmutableList<(string, IMethodSymbol)>> callDeclarations)
        {
            if (method.Name.StartsWith("Get") && method.Parameters.Length == 1 && method.Parameters.Single() is IParameterSymbol idParameter && idParameter.Type.IsValueType && genericDictionary.TryGetValue(method.ReturnType, out var returnType))
            {
                return $@"
        return Pool.Save.{returnType}Store.GetValue({idParameter.Name});";
            }
            if (method.Name.StartsWith("Lease") && genericDictionary.TryGetValue(method.ReturnType, out var leaseName))
            {
                return $@"
        return Pool.Save.{leaseName}Store.Lease();";
            }
            else if (method.Name.StartsWith("Release") && method.Parameters.Length == 1 && method.Parameters.Single() is IParameterSymbol releaseParameter && genericDictionary.TryGetValue(releaseParameter.Type, out var releaseName))
            {
                return $@"
        Pool.Save.{releaseName}Store.Release({releaseParameter.Name});";
            }
            else if (method.Name.StartsWith("Hold") && method.Parameters.Length == 1 && method.Parameters.Single() is IParameterSymbol parameter && !genericDictionary.ContainsKey(parameter.Type))
            {
                return $@"
        Pool.Save.{parameter.Type.Name}Store.Hold({parameter.Name});";
            }
            else if (callDeclarations.TryGetValue(new ResolvedMethod(method, genericDictionary, false), out var calls))
            {
                var ordered = calls.OrderBy(c => c.Item2.GetAttributes().Single(a => a.AttributeClass?.Name == "PriorityAttribute").ConstructorArguments[0].Value);

                return string.Join(Environment.NewLine, ordered.Select(method.ResolveEntityHandlerCall));
            }
            else return string.Empty;
        }

        public static string ResolveEntityHandlerCall(this IMethodSymbol method, (string handlers, IMethodSymbol method) called)
        {
            var arguments = new[] { "this" }.Concat(method.Parameters.Select(p => $"{p.Name}"));

            var call = $@"
        {called.handlers}.{called.method.Name}({string.Join(", ", arguments)});";

            return call;
        }
    }
}
