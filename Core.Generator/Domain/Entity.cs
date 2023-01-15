using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain
{
    public class Entity : Object
    {
        public bool Synchronized { get; }

        public Entity(Root root, string name, ImmutableDictionary<ISymbol, ImmutableDictionary<ISymbol, string>> dictionary) : base(root, name, dictionary)
        {
            Synchronized = dictionary.Keys.Any(i => i.GetAttributes().Any(a => a.IsSynchronizedAttribute()));
        }

        protected override IEnumerable<Property> SortProperties(IEnumerable<Property> properties)
        {
            return new[] { new Property(this, null, "Free", "nameof(Free)", "sizeof(int)") }
                .Concat(base.SortProperties(properties));
        }

        public override string GetCode()
        {
            var inheritance = GetInheritance();

            return $@"using Core.Launcher;
using Core.Launcher.Domain;
using Core.Launcher.Extensions;
using Core.Abstract.Domain;

namespace Launcher.Domain;

public struct {Name} : IEntity<Pool<Save, {Name}>, {Name}>,
    {string.Join(@",
    ", inheritance)}
{{  
    public Pool<Save, {Name}> Pool {{ get; }}

    public int Id {{ get; set; }}

    public Pointer Pointer {{ get; }}

    public int Free
    {{
        get => this.GetInt32(FreeOffset);
        set => this.SetInt32(FreeOffset, value);
    }}
    {string.Join(Environment.NewLine, PropertyMembers.Select(m => $@"
    public {m.ResolveType()} {m.Name}
    {{
        {m.ResolveGetter()}
        {m.ResolveSetter()}
    }}"))}

    public {Name}(Pool<Save, {Name}> pool, int id)
    {{
        Pool = pool;
        Id = id;
        Pointer = pool.Pointer.Offset(Schema.Offset + (Id - 1) * {Name}.Size);
    }}
    {string.Join(Environment.NewLine, MethodMembers.Select(m => $@"
    public {m.ResolveDeclaration()}
    {{{string.Join(Environment.NewLine, m.Calls.OrderBy(p => p.Priority).Select(c => $@"
        {(c.Return ? "return " : string.Empty)}{c.Name}({c.Parameters});"))}
    }}"))}

    public Pool GetPool() => Pool;

    public IStore<{Name}> GetStore() => Pool;
    {string.Join(string.Empty, Properties.Select(p => $@"
    public const int {p.CodeName}Offset = {p.Offset};"))}

    public static Property[] GetProperties()
    {{
        return new Property[]
        {{{string.Join(",", Properties.Select(p => $@"
            new({p.Name}, {p.Size}, {p.CodeName}Offset)"))}
        }};
    }}

    public const int Size = {Size};

    public static int GetSize() => Size;

    public static int GetPoolCapacity()
    {{
        return 10000;
    }}

    public static {Name} Create(Pool<Save, {Name}> pool, int id)
    {{
        return new {Name}(pool, id);
    }}

    public override string ToString()
    {{
        return $""{{Id}} {{base.ToString()}}"";
    }}
}}";
        }
    }
}
