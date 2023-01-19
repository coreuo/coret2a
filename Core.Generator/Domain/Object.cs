using Core.Generator.Domain.Members.Methods;
using Core.Generator.Domain.Members.Properties;
using Core.Generator.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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

        public ImmutableDictionary<string, ImmutableList<Call>> Calls { get; set; } = new (string, Call[])[]{}.ToImmutableDictionary(v => v.Item1, v => v.Item2.ToImmutableList());

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
                .GroupBy(m => (m.Merger, m.GroupName, m.ReturnType, m.ReturnTypeName),
                    (k, l) => k.Merger(this, k.GroupName, k.ReturnType, k.ReturnTypeName, l))
                .OrderBy(m => m.Name)
                .ToImmutableList();
        }

        public override string ToString()
        {
            return Name;
        }

        public virtual IEnumerable<Property> MutateProperties(IEnumerable<Property> properties)
        {
            return properties.OrderBy(p => p.Identifier);
        }

        public virtual IEnumerable<Call> MutateCalls(IEnumerable<Call> calls)
        {
            return calls.OrderBy(c => c.Priority);
        }

        public void AssignMetaData()
        {
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

        public abstract string GetCode();

        protected virtual string GetNamespaceCode()
        {
            return $@"using System;
using Core.Launcher;
using Core.Launcher.Domain;
using Core.Launcher.Extensions;
using Core.Launcher.Collections;
using Core.Abstract.Domain;

namespace Launcher.Domain;";
        }

        protected virtual string GetInheritanceCode()
        {
            var inheritance = Dictionary
                .Select(i => ((INamedTypeSymbol)i.Key).ResolveTypeParameters(p => i.Value[p]));

            return $@",
    {string.Join(@",
    ", inheritance)}";
        }

        protected virtual string GetPropertiesCode()
        {
            return !PropertyMembers.Any() ? string.Empty : $@"
{string.Join(Environment.NewLine, PropertyMembers.Select(m => $@"
    public {m.ResolveType()} {m.Name}
    {{
        {m.ResolveGetter()}{(m.ResolveSetter() is var setter && string.IsNullOrEmpty(setter) ? string.Empty : $@"
        {m.ResolveSetter()}")}
    }}"))}";
        }

        protected virtual string GetMethodsCode()
        {
            return !MethodMembers.Any() ? string.Empty : $@"
{string.Join(Environment.NewLine, MethodMembers.Select(m => $@"
    public {m.ResolveDeclaration()}
    {{{GetMethodBodyCode(m)}
    }}"))}";
        }

        protected virtual string GetMethodBodyCode(MethodMember.MethodMerge method)
        {
            if (!Calls.TryGetValue(method.Name, out var calls)) return string.Empty;

            var body = calls
                .GroupBy(c => (c.Priority, c.Case?.subject, c.Case?.property))
                .OrderBy(g => g.Key.Priority)
                .SelectMany(g => g.Key.property == null ? g.Select(c => $@"
        {c.GetCode()}") : g.Select(c => $@"
        switch({g.Key.subject}.{g.Key.property})
        {{
            case {c.Case?.value ?? 0}:
            {{
                {c.GetCode()}

                break;
            }}
        }}"));

            return string.Join(Environment.NewLine, body);
        }

        protected virtual string GetMetaCode()
        {
            return $@"
{string.Join(string.Empty, Properties.Select(p => $@"
    public const int {p.CodeName}Offset = {p.Offset};"))}

    private static Property[] _properties = new Property[]
    {{{string.Join(",", Properties.Select(p => $@"
        new({p.CodeName}Offset, {p.Size}, {p.Name})"))}
    }};

    public static Property[] GetProperties() => _properties;

    public const int Size = {Size};

    public static int GetSize() => Size;";
        }
    }
}
