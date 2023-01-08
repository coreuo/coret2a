using System;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace Core.Generator.Extensions
{
    public static class EntityExtensions
    {
        public static ImmutableDictionary<ISymbol, ImmutableDictionary<ISymbol, string>> ResolveInterfaceDictionary(this IEnumerable<INamedTypeSymbol> interfaces, string name, ImmutableHashSet<string> generalSubjects, string variant = null)
        {
            return interfaces.ToImmutableDictionary(i => i, i => i.TypeParameters.ToImmutableDictionary(p => p, p => p.ResolveEntityOrElementParameter(name, variant, generalSubjects), SymbolEqualityComparer.Default), SymbolEqualityComparer.Default);
        }

        public static string ResolveEntityOrElementParameter(this ITypeParameterSymbol parameter, string name, string parentVariant, ImmutableHashSet<string> generalSubjects)
        {
            var @interface = parameter.ConstraintTypes
                .OfType<INamedTypeSymbol>()
                .Single();

            if (@interface.HasEntityAttribute() || @interface.HasElementAttribute()) return @interface.ResolveEntityOrElementParameter(parentVariant, generalSubjects);
            /*if (@interface.IsPool()) return $"Core.Launcher.Domain.Pool<Core.Launcher.Domain.Save, {@interface.TypeArguments.Single()}>";
            if (@interface.IsCache()) return $"Core.Launcher.Domain.Cache<{@interface.TypeArguments.Single()}>";*/

            var nested = (ITypeParameterSymbol)@interface.TypeArguments.Single();

            var resolved = nested.ResolveEntityOrElementParameter(name, parentVariant, generalSubjects);

            if (@interface.IsProducerConsumerCollection())
            {
                if (parameter.Name.EndsWith("ConcurrentQueue")) return $"Core.Launcher.Collections.ConcurrentQueue<{resolved}>";
                else if (parameter.Name.EndsWith("ConcurrentStack")) return $"Core.Launcher.Collections.ConcurrentStack<{resolved}>";
                else if (parameter.Name.EndsWith("Queue")) return $"Core.Launcher.Collections.Queue<{resolved}>";
                else if (parameter.Name.EndsWith("Stack")) return $"Core.Launcher.Collections.Stack<{resolved}>";
            }

            if (@interface.IsCollection())
            {
                if (parameter.Name.EndsWith("Collection")) return $"Core.Launcher.Collections.List<{resolved}>";
            }

            if (@interface.IsArray())
            {
                if (parameter.Name.EndsWith("Array")) return $"IEntity<{name}>.Array<{resolved}>";
            }

            throw new InvalidOperationException("Invalid entity or element parameter.");
        }

        public static string ResolveEntityOrElementParameter(this INamedTypeSymbol @interface, string parentVariant, ImmutableHashSet<string> generalSubjects)
        {
            var subject = @interface.GetSubject();

            var childVariant = @interface.GetVariant();

            var result = generalSubjects.Contains(subject) ? subject : string.IsNullOrEmpty(childVariant) ? $"{parentVariant}{subject}" : $"{childVariant}{subject}";

            return result;
        }

        public static IEnumerable<INamedTypeSymbol> GetEntityAndElementInterfaces(this IAssemblySymbol assembly)
        {
            return assembly.GlobalNamespace.FindRecursive(HasEntityAttribute).Concat(assembly.GlobalNamespace.FindRecursive(HasElementAttribute)).ToList();
        }

        public static string GetSubject(this INamedTypeSymbol symbol)
        {
            return symbol.HasElementAttribute() ? symbol.GetElementAttribute().GetSubject() : symbol.GetEntityAttribute().GetSubject();
        }

        public static string GetSubject(this AttributeData attribute)
        {
            if (attribute.ConstructorArguments.Length < 2)
                return (string)attribute.ConstructorArguments[0].Value;
            
            return (string)attribute.ConstructorArguments[1].Value;
        }

        public static string GetVariant(this INamedTypeSymbol symbol)
        {
            return symbol.HasElementAttribute() ? symbol.GetElementAttribute().GetVariant() : symbol.GetEntityAttribute().GetVariant();
        }

        public static string GetVariant(this AttributeData attribute)
        {
            if (attribute.ConstructorArguments.Length < 2)
                return string.Empty;
            
            return (string)attribute.ConstructorArguments[0].Value;
        }

        public static bool HasVariant(this INamedTypeSymbol symbol)
        {
            return !string.IsNullOrEmpty(symbol.GetVariant());
        }

        public static bool HasEntityAttribute(this INamedTypeSymbol symbol)
        {
            return symbol.GetAttributes().Any(IsEntityAttribute);
        }

        public static bool HasElementAttribute(this INamedTypeSymbol symbol)
        {
            return symbol.GetAttributes().Any(IsElementAttribute);
        }

        public static AttributeData GetEntityAttribute(this INamedTypeSymbol symbol)
        {
            return symbol.GetAttributes().Single(IsEntityAttribute);
        }

        public static AttributeData GetElementAttribute(this INamedTypeSymbol symbol)
        {
            return symbol.GetAttributes().Single(IsElementAttribute);
        }

        public static bool IsEntityAttribute(this AttributeData attribute)
        {
            return attribute.AttributeClass?.Name == "EntityAttribute";
        }

        public static bool IsElementAttribute(this AttributeData attribute)
        {
            return attribute.AttributeClass?.Name == "ElementAttribute";
        }

        public static bool IsSynchronizedAttribute(this AttributeData attribute)
        {
            return attribute.AttributeClass?.Name == "SynchronizedAttribute";
        }

        /*public static bool IsPool(this INamedTypeSymbol symbol)
        {
            return symbol.Name == "IPool";
        }

        public static bool IsCache(this INamedTypeSymbol symbol)
        {
            return symbol.Name == "ICache";
        }*/

        public static bool IsProducerConsumerCollection(this INamedTypeSymbol symbol)
        {
            return symbol.Name == "IProducerConsumerCollection";
        }

        public static bool IsCollection(this INamedTypeSymbol symbol)
        {
            return symbol.Name == "ICollection";
        }

        public static bool IsArray(this INamedTypeSymbol symbol)
        {
            return symbol.Name == "IReadOnlyList";
        }
    }
}
