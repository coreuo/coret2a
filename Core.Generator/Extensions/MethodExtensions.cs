using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

            var attributes = method.GetAttributes().SelectMany(ResolveAttribute);

            return $@"{string.Join(string.Empty, attributes.Select(a => $@"
    [{a}]"))}
    public static {method.ReturnType} {method.Name}({string.Join(", ", parameters)})
    {{
        {(method.ReturnsVoid ? string.Empty : "return ")}{rootName}.{method.Name}({string.Join(", ", arguments)});
    }}";
        }

        private static IEnumerable<string> ResolveAttribute(AttributeData attribute)
        {
            return ResolveAttribute(attribute.AttributeClass, attribute.AttributeConstructor, ResolveConstructorArguments(attribute));
        }

        private static IEnumerable<string> ResolveAttribute(ITypeSymbol @class, IMethodSymbol constructor, IEnumerable<string> arguments)
        {
            if (!IsNestedCoreAttribute(@class)) yield break;

            var nestedAttributes = @class.GetAttributes();

            foreach (var nested in nestedAttributes.SelectMany(ResolveAttribute))
            {
                yield return nested;
            }

            if (IsCoreAttribute(@class))
            {
                var result = $"{@class}({string.Join(", ", arguments)})";

                yield return result;
            }
            else
            {
                foreach (var attribute in ResolveAttribute(@class, constructor, ResolveConstructorDictionary(constructor, arguments)))
                {
                    yield return attribute;
                }
            }
        }
        private static IEnumerable<string> ResolveAttribute(ITypeSymbol @class, IMethodSymbol constructor, Dictionary<string, string> incoming)
        {
            if (@class.BaseType == null) return null;

            var outgoing = ResolveBaseConstructorDictionary(constructor, @class.BaseType, out var baseConstructor);

            if(outgoing == null) return null;

            foreach (var argument in incoming)
            {
                outgoing[argument.Key] = argument.Value;
            }

            return ResolveAttribute(@class.BaseType, baseConstructor, outgoing.Values.ToArray());
        }

        private static Dictionary<string, string> ResolveBaseConstructorDictionary(IMethodSymbol constructor, INamedTypeSymbol baseClass, out IMethodSymbol baseConstructor)
        {
            baseConstructor = baseClass.Constructors[0];

            var baseArguments = ResolveConstructorArguments(constructor);

            return baseArguments == null ? null : ResolveConstructorDictionary(baseConstructor, baseArguments);
        }

        private static Dictionary<string, string> ResolveConstructorDictionary(IMethodSymbol constructor, IEnumerable<string> arguments)
        {
            return constructor.Parameters.Zip(arguments, (p, a) => (p, a))
                .ToDictionary(g => g.p.Name, g => g.a);
        }

        private static IEnumerable<string> ResolveConstructorArguments(ISymbol constructor)
        {
            if (constructor.DeclaringSyntaxReferences.Length != 1) return null;

            var syntax = (ConstructorDeclarationSyntax)constructor.DeclaringSyntaxReferences[0].GetSyntax();

            return syntax.Initializer == null ? null : ResolveConstructorArguments(syntax.Initializer.ArgumentList.Arguments);
        }

        private static IEnumerable<string> ResolveConstructorArguments(AttributeData attribute)
        {
            var syntax = attribute.ApplicationSyntaxReference?.GetSyntax() as AttributeSyntax;

            if(syntax?.ArgumentList == null) yield break;

            foreach (var argument in ResolveConstructorArguments(syntax.ArgumentList.Arguments))
            {
                yield return argument;
            }
        }

        private static IEnumerable<string> ResolveConstructorArguments(object arguments)
        {
            return arguments.ToString().Replace(" ", string.Empty).Split(',');
        }

        private static bool IsNestedCoreAttribute(ITypeSymbol attributeClass)
        {
            if (attributeClass == null) return false;

            if (IsCoreAttribute(attributeClass)) return true;

            return attributeClass.BaseType != null && IsNestedCoreAttribute(attributeClass.BaseType);
        }

        private static bool IsCoreAttribute(ISymbol attributeClass)
        {
            if (attributeClass == null) return false;

            return attributeClass.ContainingNamespace.ToString() == "Core.Abstract.Attributes";
        }
    }
}
