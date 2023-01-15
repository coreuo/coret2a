﻿using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain
{
    public class Element : Object
    {
        public Element(Root root, string name, ImmutableDictionary<ISymbol, ImmutableDictionary<ISymbol, string>> dictionary) : base(root, name, dictionary)
        {
        }

        public override string GetCode()
        {
            var inheritance = GetInheritance();

            return $@"using Core.Launcher;
using Core.Launcher.Domain;
using Core.Launcher.Extensions;
using Core.Abstract.Domain;

namespace Launcher.Domain;

public struct {Name} : IElement<{Name}>,
    {string.Join(@",
    ", inheritance)}
{{  
    public Pool Pool {{ get; }}

    public int Id {{ get; set; }}

    public Pointer Pointer {{ get; }}
    {string.Join(Environment.NewLine, PropertyMembers.Select(m => $@"
    public {m.ResolveType()} {m.Name}
    {{
        {m.ResolveGetter()}
        {m.ResolveSetter()}
    }}"))}

    public {Name}(Pool pool, int elementId, int entityId, int entityOffset)
    {{
        Pool = pool;
        Id = elementId;
        Pointer = pool.Pointer.Offset(Schema.Offset + (entityId - 1) * Pool.Schema.Size + entityOffset + (Id - 1) * {Name}.Size);
    }}

    public Pool GetPool() => Pool;
    {string.Join(Environment.NewLine, MethodMembers.Select(m => $@"
    public {m.ResolveDeclaration()}
    {{{string.Join(Environment.NewLine, m.Calls.OrderBy(p => p.Priority).Select(c => $@"
        {(c.Return ? "return " : string.Empty)}{c.Name}({c.Parameters});"))}
    }}"))}
    {string.Join(string.Empty, Properties.Select(p => $@"
    public const int {p.CodeName}Offset = {p.Offset};"))}

    public const int Size = {Size};

    private static Property[] _properties = new Property[]
    {{{string.Join(",", Properties.Select(p => $@"
        new({p.Name}, {p.Size}, {p.Offset})"))}
    }};

    public static {Name} Create(Pool pool, int elementId, int entityId, int entityIndex)
    {{
        return new {Name}(pool, elementId, entityId, entityIndex);
    }}

    public override string ToString()
    {{
        return $""{{Id}} {{base.ToString()}}"";
    }}
}}";
        }
    }
}
