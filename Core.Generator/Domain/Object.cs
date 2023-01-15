using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.Generator.Domain.Members.Methods;
using Core.Generator.Domain.Members.Properties;
using Core.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain
{
    public abstract class Object
    {
        public Root Root { get; }

        public string Name { get; }

        public ImmutableDictionary<ISymbol, ImmutableDictionary<ISymbol, string>> Dictionary { get; }

        public IImmutableList<PropertyMember.PropertyMerge> PropertyMembers { get; set; }

        public IImmutableList<MethodMember.MethodMerge> MethodMembers { get; set; }

        public IImmutableList<Property> Properties { get; set; } = new Property[] { }.ToImmutableArray();

        public string Size { get; set; } = "0";

        protected Object(Root root, string name, ImmutableDictionary<ISymbol, ImmutableDictionary<ISymbol, string>> dictionary)
        {
            Root = root;
            Name = name;
            Dictionary = dictionary;
        }

        public static Object Create(Root root, string name,
            ImmutableDictionary<ISymbol, ImmutableDictionary<ISymbol, string>> dictionary)
        {
            if (dictionary.Keys.Any(i => i.GetAttributes().Any(a => a.IsElementAttribute())))
                return new Element(root, name, dictionary);

            if (dictionary.Keys.Any(i => i.GetAttributes().Any(a => a.IsEntityAttribute())))
                return new Entity(root, name, dictionary);

            throw new InvalidOperationException($"Invalid object {name}.");
        }

        public void AssignMembers()
        {
            PropertyMembers = Dictionary
                .SelectMany(i =>
                    ((INamedTypeSymbol)i.Key).GetNestedProperties()
                    .Select(p => PropertyMember.Create(this, (INamedTypeSymbol)i.Key, p)))
                .Where(m => m != null)
                .GroupBy(m => (m.Merger, m.Original.Name, m.Type, m.TypeName),
                    (k, l) => k.Merger(this, k.Name, k.Type, k.TypeName, l))
                .OrderBy(m => m.Name)
                .ToImmutableList();

            MethodMembers = Dictionary
                .SelectMany(i =>
                    ((INamedTypeSymbol)i.Key).GetNestedMethods()
                    .Select(p => MethodMember.Create(this, (INamedTypeSymbol)i.Key, p)))
                .Where(m => m != null)
                .GroupBy(m => (m.Merger, m.GroupName, m.ReturnType, m.ReturnTypeName, m.Parameters),
                    (k, l) => k.Merger(this, k.GroupName, k.ReturnType, k.ReturnTypeName, k.Parameters, l))
                .OrderBy(m => m.Name)
                .ToImmutableList();
        }

        public override string ToString()
        {
            return Name;
        }

        public void AssignCalls()
        {
            foreach (var member in MethodMembers)
            {
                member.Calls = member.ResolveCalls().OrderBy(c => c.Priority).ToImmutableList();
            }
        }

        public void AssignProperties(IEnumerable<Property> properties)
        {
            Properties = SortProperties(properties).ToImmutableList();

            var index = 0;

            Property last = null;

            foreach (var property in Properties)
            {
                property.Index = index++;

                property.Offset = last == null ? "0" : $"{last.CodeName}Offset + {last.Size}";

                last = property;
            }

            Size = last == null ? "0" : $"{last.CodeName}Offset + {last.Size}";
        }

        protected virtual IEnumerable<Property> SortProperties(IEnumerable<Property> properties)
        {
            return properties.OrderBy(p => p.Identifier);
        }

        public abstract string GetCode();

        protected ImmutableList<string> GetInheritance()
        {
            return Dictionary
                .Select(i => ((INamedTypeSymbol)i.Key).ResolveTypeParameters(p => i.Value[p]))
                .ToImmutableList();
        }
    }
}
