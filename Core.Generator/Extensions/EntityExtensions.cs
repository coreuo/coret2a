using System;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace Core.Generator.Extensions
{
    public static class EntityExtensions
    {
        public static ImmutableDictionary<ISymbol, ImmutableDictionary<ISymbol, string>> ResolveInterfaceDictionary(this IEnumerable<INamedTypeSymbol> interfaces, ImmutableHashSet<string> generalSubjects, string variant = null)
        {
            return interfaces.ToImmutableDictionary(i => i, i => i.TypeParameters.ToImmutableDictionary(p => p, p => p.ResolveEntityParameter(variant, generalSubjects), SymbolEqualityComparer.Default), SymbolEqualityComparer.Default);
        }

        public static string ResolveEntityParameter(this ITypeParameterSymbol parameter, string parentVariant, ImmutableHashSet<string> generalSubjects)
        {
            var @interface = parameter.ConstraintTypes
                .OfType<INamedTypeSymbol>()
                .Single();

            if (@interface.HasEntityAttribute()) return @interface.ResolveEntityParameter(parentVariant, generalSubjects);
            /*if (@interface.IsPool()) return $"Core.Launcher.Domain.Pool<Core.Launcher.Domain.Save, {@interface.TypeArguments.Single()}>";
            if (@interface.IsCache()) return $"Core.Launcher.Domain.Cache<{@interface.TypeArguments.Single()}>";*/

            var nested = (ITypeParameterSymbol)@interface.TypeArguments.Single();

            var resolved = nested.ResolveEntityParameter(parentVariant, generalSubjects);

            if (@interface.IsProducerConsumerCollection())
            {
                if (parameter.Name.EndsWith("ConcurrentQueue")) return $"Core.Launcher.Collections.ConcurrentQueue<{resolved}>";
                else if(parameter.Name.EndsWith("ConcurrentStack")) return $"Core.Launcher.Collections.ConcurrentStack<{resolved}>";
                else if(parameter.Name.EndsWith("Queue")) return $"Core.Launcher.Collections.Queue<{resolved}>";
                else if(parameter.Name.EndsWith("Stack")) return $"Core.Launcher.Collections.Stack<{resolved}>";
            }

            if (@interface.IsCollection())
            {
                if (parameter.Name.EndsWith("Collection")) return $"Core.Launcher.Collections.List<{resolved}>";
            }

            throw new InvalidOperationException("Invalid entity parameter.");
        }

        public static string ResolveEntityParameter(this INamedTypeSymbol @interface, string parentVariant, ImmutableHashSet<string> generalSubjects)
        {
            var subject = @interface.GetEntitySubject();

            var childVariant = @interface.GetEntityVariant();

            var result = generalSubjects.Contains(subject) ? subject : string.IsNullOrEmpty(childVariant) ? $"{parentVariant}{subject}" : $"{childVariant}{subject}";

            return result;
        }

        public static IEnumerable<INamedTypeSymbol> GetEntityInterfaces(this IAssemblySymbol assembly)
        {
            return assembly.GlobalNamespace.FindRecursive(HasEntityAttribute).ToList();
        }

        public static string GetEntitySubject(this INamedTypeSymbol symbol)
        {
            return symbol.GetEntityAttribute().GetEntitySubject();
        }

        public static string GetEntitySubject(this AttributeData attribute)
        {
            if (attribute.ConstructorArguments.Length < 2)
                return (string)attribute.ConstructorArguments[0].Value;
            
            return (string)attribute.ConstructorArguments[1].Value;
        }

        public static string GetEntityVariant(this INamedTypeSymbol symbol)
        {
            return symbol.GetEntityAttribute().GetEntityVariant();
        }

        public static string GetEntityVariant(this AttributeData attribute)
        {
            if (attribute.ConstructorArguments.Length < 2)
                return string.Empty;
            
            return (string)attribute.ConstructorArguments[0].Value;
        }

        public static bool HasEntityVariant(this INamedTypeSymbol symbol)
        {
            return !string.IsNullOrEmpty(symbol.GetEntityVariant());
        }

        public static bool HasEntityAttribute(this INamedTypeSymbol symbol)
        {
            return symbol.GetAttributes().Any(IsEntityAttribute);
        }

        public static bool HasSynchronizedAttribute(this INamedTypeSymbol symbol)
        {
            return symbol.GetAttributes().Any(IsSynchronizedAttribute);
        }

        public static bool HasOrderedAttribute(this INamedTypeSymbol symbol)
        {
            return symbol.GetAttributes().Any(IsOrderedAttribute);
        }

        public static AttributeData GetEntityAttribute(this INamedTypeSymbol symbol)
        {
            return symbol.GetAttributes().Single(IsEntityAttribute);
        }

        public static bool IsEntityAttribute(this AttributeData attribute)
        {
            return attribute.AttributeClass?.Name == "EntityAttribute";
        }

        public static bool IsSynchronizedAttribute(this AttributeData attribute)
        {
            return attribute.AttributeClass?.Name == "SynchronizedAttribute";
        }

        public static bool IsOrderedAttribute(this AttributeData attribute)
        {
            return attribute.AttributeClass?.Name == "OrderedAttribute";
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
    }
}
